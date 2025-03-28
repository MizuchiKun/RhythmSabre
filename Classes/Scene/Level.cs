using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;  // Causes some issue when I'm trying to run it years later.

namespace ProjektRoguelike
{
    /// <summary>
    /// A level.
    /// </summary>
    public class Level : Scene
    {
        /// <summary>
        /// The generated <see cref="Room"/>s of this Level.
        /// </summary>
        private static Room[,] _rooms = new Room[6, 6];

        /// <summary>
        /// The current room.
        /// </summary>
        public static Room CurrentRoom { get; set; }

        /// <summary>
        /// The next room. <br></br>
        /// Is used for transitions if not null.
        /// </summary>
        private static Room _nextRoom;

        /// <summary>
        /// The direction in which the transition goes. <br></br>
        /// Is used for transitions if not null.
        /// </summary>
        private static Directions? _transitionDirection = null;

        /// <summary>
        /// The <see cref="ProjektRoguelike.Player"/> of all levels.
        /// </summary>
        public static Player Player { get => _player; }
        private static Player _player;

        /// <summary>
        /// The seed that is used to generate the level.
        /// </summary>
        public static int Seed { get; set; }

        /// <summary>
        /// The index of this Level.
        /// </summary>
        public static byte LevelIndex;

        /// <summary>
        /// Creates a new Level by the given level index.
        /// </summary>
        /// <param name="levelIndex">The index of the level you want to create.</param>
        public Level(byte levelIndex, bool loadFromFile = false)
        {
            // Store the levelIndex.
            LevelIndex = levelIndex;

            // Clear the level.
            _rooms = new Room[6, 6];

            // Generate the seed (maybe add "enter seed" feature).
            Seed = new Random().Next();

            // Load the level.
            if (loadFromFile)
            {
                LoadData(Mainmenu.xmlLevel);
            }

            // Get the Random.
            Random random = new Random(Seed);

            // Generate the start room.
            Vector2 startRoomPos = new Vector2(random.Next(_rooms.GetLength(0)),
                                               random.Next(_rooms.GetLength(1)));
            // Set the start room.
            _rooms[(int)startRoomPos.X, (int)startRoomPos.Y] = new Room(0, startRoomPos, RoomKind.Start);
            // Set the camera position.
            if (!loadFromFile)
            {
                Camera.Position = startRoomPos * Globals.WindowDimensions;
            }

            // Generate the path to the boss.
            List<Vector2> roomPositions = new List<Vector2>();
            Vector2 currentRoomPos = startRoomPos;
            for (byte i = 0; i < 6 && HasAdjacentEmptyCell(currentRoomPos); i++)
            {
                // Choose a random, adjacent, empty grid cell.
                byte chosenDirection;
                Vector2 nextRoomPos;
                do
                {
                    chosenDirection = (byte)random.Next(4);
                    nextRoomPos = currentRoomPos + Globals.DirectionVectors[chosenDirection];
                }
                while (!((nextRoomPos.X >= 0 && nextRoomPos.X < _rooms.GetLength(0))
                        && (nextRoomPos.Y >= 0 && nextRoomPos.Y < _rooms.GetLength(1)))
                       || _rooms[(int)nextRoomPos.X, (int)nextRoomPos.Y] != null);

                // The next room becomes the current room.
                currentRoomPos = nextRoomPos;

                // Store it in the List.
                roomPositions.Add(currentRoomPos);

                // Get the number of .room files in the "Normal" directory.
                byte roomCount = (byte)Directory.GetFiles(path: $"..\\..\\..\\Content\\Rooms\\{RoomKind.Normal}",
                                                          searchPattern:"*",
                                                          searchOption: SearchOption.TopDirectoryOnly).Length;

                // Add randomly chosen Room there.
                _rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y] =
                    new Room(roomIndex: (byte)random.Next(roomCount),
                             gridPosition: currentRoomPos,
                             kind: RoomKind.Normal);
            }

            // Remove the last room's position from the List.
            roomPositions.RemoveAt(roomPositions.Count - 1);

            // Store the number of rooms before the boss.
            byte roomsBeforeBoss = (byte)roomPositions.Count;

            // Replace the last room with the boss room.
            _rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y] =
                new Room(roomIndex: LevelIndex,
                         gridPosition: currentRoomPos,
                         kind: RoomKind.Boss);

            // Choose a Room randomly from which you can start.
            do
            {
                currentRoomPos = roomPositions[random.Next(roomPositions.Count)];
            }
            while (!HasAdjacentEmptyCell(currentRoomPos));
            // Generate additional, optional Rooms.
            for (byte i = 0; i < 8 && HasAdjacentEmptyCell(currentRoomPos); i++)
            {
                // Choose a random, adjacent, empty grid cell.
                byte chosenDirection;
                Vector2 nextRoomPos;
                do
                {
                    chosenDirection = (byte)random.Next(4);
                    nextRoomPos = currentRoomPos + Globals.DirectionVectors[chosenDirection];
                }
                while (!((nextRoomPos.X >= 0 && nextRoomPos.X < _rooms.GetLength(0))
                        && (nextRoomPos.Y >= 0 && nextRoomPos.Y < _rooms.GetLength(1)))
                       || _rooms[(int)nextRoomPos.X, (int)nextRoomPos.Y] != null);

                // The next room becomes the current room.
                currentRoomPos = nextRoomPos;

                // Store it in the List.
                roomPositions.Add(currentRoomPos);

                // Get the number of .room files in the "Normal" directory.
                byte roomCount = (byte)Directory.GetFiles(path: $"..\\..\\..\\Content\\Rooms\\{RoomKind.Normal}",
                                                          searchPattern: "*",
                                                          searchOption: SearchOption.TopDirectoryOnly).Length;

                // Add randomly chosen Room there.
                _rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y] =
                    new Room(roomIndex: (byte)random.Next(roomCount),
                             gridPosition: currentRoomPos,
                             kind: RoomKind.Normal);
            }


            // Get the number of .room files in the "Hidden" directory.
            byte hiddenRoomCount = (byte)Directory.GetFiles(path: $"..\\..\\..\\Content\\Rooms\\{RoomKind.Hidden}",
                                                            searchPattern: "*",
                                                            searchOption: SearchOption.TopDirectoryOnly).Length;

            // Replace the last room with a randomly chosen, hidden room.
            _rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y] =
                new Room(roomIndex: (byte)random.Next(hiddenRoomCount),
                         gridPosition: currentRoomPos,
                         kind: RoomKind.Hidden);

            // Set the current room.
            currentRoomPos = Convert.ToInt32(loadFromFile) * (Camera.Position / Globals.WindowDimensions) +
                             Convert.ToInt32(!loadFromFile) * startRoomPos;
            CurrentRoom = _rooms[(int)currentRoomPos.X, (int)currentRoomPos.Y];

            if (LevelIndex <= 0)
            {
                // Initialize the player.
                _player = new Player(CurrentRoom.Position + (Room.Dimensions / 2 + new Vector2(0.5f, 0)) * Tile.Size * Globals.Scale);
            }

            // Add the doors.
            for (byte x = 0; x < _rooms.GetLength(0); x++)
            {
                for (byte y = 0; y < _rooms.GetLength(1); y++)
                {
                    // If there's a room at these indices.
                    if (_rooms[x, y] != null)
                    {
                        // Add doors in all directions in which there is an adjacent room.
                        Door[] doors = new Door[4];
                        DoorKind doorKind;
                        // Up.
                        if (y - 1 >= 0
                            && _rooms[x, y - 1] != null)
                        {
                            // Choose the door kind.
                            if (_rooms[x, y].Kind == RoomKind.Boss)
                            {
                                doorKind = DoorKind.Boss;
                            }
                            else if (_rooms[x, y].Kind == RoomKind.Hidden)
                            {
                                doorKind = DoorKind.Hidden;
                            }
                            else
                            {
                                if (_rooms[x, y - 1].Kind == RoomKind.Boss)
                                {
                                    doorKind = DoorKind.Boss;
                                }
                                else if (_rooms[x, y - 1].Kind == RoomKind.Hidden)
                                {
                                    doorKind = DoorKind.Hidden;
                                }
                                else
                                {
                                    doorKind = DoorKind.Normal;
                                }
                            }

                            // Add the Door.
                            doors[(byte)Directions.Up] = new Door(position: (new Vector2(x, y) * Globals.WindowDimensions
                                                                             + new Vector2(1, 0.5f) * Tile.Size * Globals.Scale)
                                                                            + new Vector2((Room.Dimensions.X - 1) / 2,
                                                                                          0)
                                                                              * Tile.Size * Globals.Scale,
                                                                  direction: Directions.Up,
                                                                  kindOfDoor: doorKind);
                        }
                        // Right.
                        if (x + 1 < _rooms.GetLength(0)
                            && _rooms[x + 1, y] != null)
                        {
                            // Choose the door kind.
                            if (_rooms[x, y].Kind == RoomKind.Boss)
                            {
                                doorKind = DoorKind.Boss;
                            }
                            else if (_rooms[x, y].Kind == RoomKind.Hidden)
                            {
                                doorKind = DoorKind.Hidden;
                            }
                            else
                            {
                                if (_rooms[x + 1, y].Kind == RoomKind.Boss)
                                {
                                    doorKind = DoorKind.Boss;
                                }
                                else if (_rooms[x + 1, y].Kind == RoomKind.Hidden)
                                {
                                    doorKind = DoorKind.Hidden;
                                }
                                else
                                {
                                    doorKind = DoorKind.Normal;
                                }
                            }

                            // Add the Door.
                            doors[(byte)Directions.Right] = new Door(position: (new Vector2(x, y) * Globals.WindowDimensions
                                                                                + new Vector2(1, 0.5f) * Tile.Size * Globals.Scale)
                                                                               + new Vector2(Room.Dimensions.X - 1,
                                                                                             (Room.Dimensions.Y - 1) / 2)
                                                                                 * Tile.Size * Globals.Scale,
                                                                     direction: Directions.Right,
                                                                     kindOfDoor: doorKind);
                        }
                        // Down.
                        if (y + 1 < _rooms.GetLength(1)
                            && _rooms[x, y + 1] != null)
                        {
                            // Choose the door kind.
                            if (_rooms[x, y].Kind == RoomKind.Boss)
                            {
                                doorKind = DoorKind.Boss;
                            }
                            else if (_rooms[x, y].Kind == RoomKind.Hidden)
                            {
                                doorKind = DoorKind.Hidden;
                            }
                            else
                            {
                                if (_rooms[x, y + 1].Kind == RoomKind.Boss)
                                {
                                    doorKind = DoorKind.Boss;
                                }
                                else if (_rooms[x, y + 1].Kind == RoomKind.Hidden)
                                {
                                    doorKind = DoorKind.Hidden;
                                }
                                else
                                {
                                    doorKind = DoorKind.Normal;
                                }
                            }

                            // Add the Door.
                            doors[(byte)Directions.Down] = new Door(position: (new Vector2(x, y) * Globals.WindowDimensions
                                                                               + new Vector2(1, 0.5f) * Tile.Size * Globals.Scale)
                                                                              + new Vector2((Room.Dimensions.X - 1) / 2,
                                                                                            Room.Dimensions.Y - 1)
                                                                                * Tile.Size * Globals.Scale,
                                                                    direction: Directions.Down,
                                                                    kindOfDoor: doorKind);
                        }
                        // Left.
                        if (x - 1 >= 0
                            && _rooms[x - 1, y] != null)
                        {
                            // Choose the door kind.
                            if (_rooms[x, y].Kind == RoomKind.Boss)
                            {
                                doorKind = DoorKind.Boss;
                            }
                            else if (_rooms[x, y].Kind == RoomKind.Hidden)
                            {
                                doorKind = DoorKind.Hidden;
                            }
                            else
                            {
                                if (_rooms[x - 1, y].Kind == RoomKind.Boss)
                                {
                                    doorKind = DoorKind.Boss;
                                }
                                else if (_rooms[x - 1, y].Kind == RoomKind.Hidden)
                                {
                                    doorKind = DoorKind.Hidden;
                                }
                                else
                                {
                                    doorKind = DoorKind.Normal;
                                }
                            }

                            // Add the Door.
                            doors[(byte)Directions.Left] = new Door(position: (new Vector2(x, y) * Globals.WindowDimensions
                                                                               + new Vector2(1, 0.5f) * Tile.Size * Globals.Scale)
                                                                              + new Vector2(0,
                                                                                            (Room.Dimensions.Y - 1) / 2)
                                                                                * Tile.Size * Globals.Scale,
                                                                    direction: Directions.Left,
                                                                    kindOfDoor: doorKind);
                        }

                        // Add the doors to the room.
                        _rooms[x, y].Doors = doors;
                    }
                }
            }

            // Add the locked Doors and their keys.
            for (byte i = roomsBeforeBoss; i < roomPositions.Count; i++)
            {
                // Get the current position.
                Vector2 currentPosition = roomPositions[i];

                // There's a 20% chance that a Room contains a locked door.
                if (random.Next(100) < 20)
                {
                    // Choose a random Door.
                    Room currentRoom = _rooms[(int)currentPosition.X, (int)currentPosition.Y];
                    byte direction;
                    do
                    {
                        direction = (byte)random.Next(4);
                    }
                    while (currentRoom.Doors[direction] == null);
                    // Lock that Door.
                    _rooms[(int)currentPosition.X, (int)currentPosition.Y].Doors[direction].Lock(true);

                    // There's a 80% chance that you can find the key in the Level.
                    // (Otherwise you have to bring a key from the previous Level.)
                    if (random.Next(100) < 80)
                    {
                        // Place the key in a random, previous Room.
                        Vector2 chosenRoomPos = roomPositions[random.Next(i)];
                        _rooms[(int)chosenRoomPos.X, (int)chosenRoomPos.Y].Add(
                            new PickupKey((chosenRoomPos + new Vector2(0.5f)) * Globals.WindowDimensions)
                        );
                        Console.WriteLine($"Level.cs:398: Change how / where the keys are added to the Room! (Enemy drop, Pot, etc.)");
                    }
                }
            }
        }

        /// <summary>
        /// Updates the currently active <see cref="Room"/> and the <see cref="Player"/>.
        /// </summary>
        public override void Update()
        {
            // If not transition is happening.
            if (_transitionDirection == null)
            {
                // If the player is in the boss room and all enemies are dead, spawn a trapdoor to the next level.
                if (CurrentRoom.Kind == RoomKind.Boss && CurrentRoom.Enemies.Count == 0)
                {
                    Level.CurrentRoom.Add(new Trapdoor(Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale + new Vector2(0, 150)));
                }

                // Update the current room if no transition is happening.
                CurrentRoom.Update();

                // Update the Player.
                Player.Update();
            }
            // Else a transition is happening.
            else
            {
                // Get the direction.
                Vector2 direction = Globals.DirectionVectors[(byte)_transitionDirection];

                // Place the player in the next room near the corresponding door.
                Player.Position = _nextRoom.Doors[((byte)_transitionDirection + 2) % 4].Position + (direction * 0.5f * Tile.Size);

                // Move the camera.
                ushort durationInMs = 250;
                float speed = ((byte)_transitionDirection % 2 == 0 ? Globals.WindowDimensions.Y : Globals.WindowDimensions.X) / (durationInMs / 1e3f);
                Camera.Position += direction * speed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

                // Move the camera back if it moved too far.
                bool transitionEnded = false;
                switch (_transitionDirection)
                {
                    case Directions.Up:
                        if (Camera.Position.Y <= CurrentRoom.Position.Y - Globals.WindowDimensions.Y)
                        {
                            transitionEnded = true;
                            Camera.Position = CurrentRoom.Position + direction * Globals.WindowDimensions;
                        }
                        break;
                    case Directions.Right:
                        if (Camera.Position.X >= CurrentRoom.Position.X + Globals.WindowDimensions.X)
                        {
                            transitionEnded = true;
                            Camera.Position = CurrentRoom.Position + direction * Globals.WindowDimensions;
                        }
                        break;
                    case Directions.Down:
                        if (Camera.Position.Y >= CurrentRoom.Position.Y + Globals.WindowDimensions.Y)
                        {
                            transitionEnded = true;
                            Camera.Position = CurrentRoom.Position + direction * Globals.WindowDimensions;
                        }
                        break;
                    case Directions.Left:
                        if (Camera.Position.X <= CurrentRoom.Position.X - Globals.WindowDimensions.X)
                        {
                            transitionEnded = true;
                            Camera.Position = CurrentRoom.Position + direction * Globals.WindowDimensions;
                        }
                        break;
                }

                // If the transition ended.
                if (transitionEnded)
                {
                    // The next room becomes the current room.
                    CurrentRoom = _nextRoom;

                    // Set the next room and the transition direction to null.
                    _nextRoom = null;
                    _transitionDirection = null;
                }
            }
        }

        public override void Draw()
        {
            // Draw the current room.
            CurrentRoom.Draw();

            // Draw the next room if it's not null.
            if (_nextRoom != null)
            {
                _nextRoom.Draw();
            }

            // Draw the player.
            Player.Draw();

        }

        /// <summary>
        /// Initiates the room switching transition.
        /// </summary>
        /// <param name="direction">The direction of the next room.</param>
        public static void SwitchRoom(Directions direction)
        {
            // Store the transition direction.
            _transitionDirection = direction;

            // Set the next room.
            Vector2 nextRoomPos = (CurrentRoom.Position / Globals.WindowDimensions) + Globals.DirectionVectors[(byte)direction];
            _nextRoom = _rooms[(int)nextRoomPos.X, (int)nextRoomPos.Y];
        }

        /// <summary>
        /// Unlocks the counterpart of the given door.
        /// </summary>
        /// <param name="originDoor">The given door.</param>
        public static void UnlockCounterpartDoor(Door originDoor)
        {
            // Get the room indices.
            Vector2 currentRoomPos = new Vector2((float)Math.Floor(originDoor.Position.X / Globals.WindowDimensions.X),
                                                 (float)Math.Floor(originDoor.Position.Y / Globals.WindowDimensions.Y));

            // Get the direction vector.
            Vector2 directionVector = Globals.DirectionVectors[(byte)originDoor.Direction];

            // Unlock the counterpart.
            _rooms[(int)(currentRoomPos.X + directionVector.X), (int)(currentRoomPos.Y + directionVector.Y)].Doors[((byte)originDoor.Direction + 2) % 4].Unlock();
        }

        /// <summary>
        /// Locks the counterpart of the given door.
        /// </summary>
        /// <param name="originDoor">The given door.</param>
        public static void LockCounterpartDoor(Door originDoor)
        {
            // Get the room indices.
            Vector2 currentRoomPos = new Vector2((float)Math.Floor(originDoor.Position.X / Globals.WindowDimensions.X),
                                                 (float)Math.Floor(originDoor.Position.Y / Globals.WindowDimensions.Y));

            // Get the direction vector.
            Vector2 directionVector = Globals.DirectionVectors[(byte)originDoor.Direction];

            // Lock the counterpart.
            _rooms[(int)(currentRoomPos.X + directionVector.X), (int)(currentRoomPos.Y + directionVector.Y)].Doors[((byte)originDoor.Direction + 2) % 4].Lock();
        }

        /// <summary>
        /// Gets whether the room at the given position has 1 or more adjacent, empty grid cells.
        /// </summary>
        /// <param name="roomPos">The position of the room.</param>
        /// <returns>True if it has 1 or more adjacent, empty cells, false otherwise.</returns>
        private bool HasAdjacentEmptyCell(Vector2 roomPos)
        {
            // Check for adjacent, empty cells in all directions
            Vector2 adjacentRoomPos;
            for (byte i = 0; i < 4; i++)
            {
                adjacentRoomPos = roomPos + Globals.DirectionVectors[i];
                // If the position is inside the grid.
                if ((adjacentRoomPos.X >= 0 && adjacentRoomPos.X < _rooms.GetLength(0))
                    && (adjacentRoomPos.Y >= 0 && adjacentRoomPos.Y < _rooms.GetLength(1)))
                {
                    // If the adjacent cell is empty.
                    if (_rooms[(int)adjacentRoomPos.X, (int)adjacentRoomPos.Y] == null)
                    {
                        return true;
                    }
                }
            }

            // There seems to be no adjacent, empty cell.
            return false;
        }

        public static void SaveData()
        {
            XDocument xml = new XDocument(new XElement("Root",
                                                        new XElement("Stats", "")));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "Seed"),
                                            new XElement("amount", Seed)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "Position"),
                                            new XElement("amount", Level.CurrentRoom.Position)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "LevelIndex"),
                                            new XElement("amount", LevelIndex)));

            Globals.save.HandleSaveFormates(xml, "level.xml");
        }

        public void LoadData(XDocument data)
        {
            if (data != null)
            {
                List<XElement> statList = (from t in data.Element("Root").Element("Stats").Descendants("Stat")
                                                select t).ToList<XElement>();

                Seed = Convert.ToInt32(statList[0].Element("amount").Value, Globals.culture);

                // Char array of all the possible chars, that are to trim.
                char[] stuff = { ' ', '{', '}', 'X','Y', ':'};

                // The location, where the RoomPositionX substring is supposed to stop.
                int charLocation = statList[1].Element("amount").Value.IndexOf(" ", StringComparison.Ordinal);

                // The substring for the X-Coordinate in the position string.
                // Start at index 3 (after "{X:") and go create the substring until the charLocation of the first white space.
                string RoomPositionX = statList[1].Element("amount").Value.Substring(3, charLocation - 3);
                // The substring for the Y-Coordinate in the position string.
                // Start at the index of the length of RoomPositionX + 6 (for all the "non numbers"). Remove all possible non numbers, still included.
                string RoomPositionY = statList[1].Element("amount").Value.Substring(6 + RoomPositionX.Length).Trim(stuff);

                // Create a new vector2 and use the position values, defined above.
                Camera.Position = new Vector2(Convert.ToInt32(RoomPositionX), Convert.ToInt32(RoomPositionY));

                //LevelIndex = Convert.ToByte(statList[2].Element("amount").Value, Globals.culture);
            }
        }
    }
}
