using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// Contains the different kinds of rooms.
    /// </summary>
    public enum RoomKind : byte
    {
        Start,
        Normal,
        Hidden,
        //Arcade,
        Boss
    }

    /// <summary>
    /// A room of a <see cref="Level"/>.
    /// </summary>
    public class Room : GameObjectContainer
    {
        /// <summary>
        /// The dimensions of a room (measured in sprites).
        /// </summary>
        public static Vector2 Dimensions { get; } = new Vector2(15, 9);

        /// <summary>
        /// The position of the top-left corner of this <see cref="Room"/>.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The position of the top-left corner.
        /// </summary>
        private Vector2 _topLeftCorner;

        /// <summary>
        /// The Texture of a Room of this Level.
        /// </summary>
        private static Texture2D _roomTexture;

        /// <summary>
        /// The Sprite of this Room's background.
        /// </summary>
        private Sprite _roomSprite;

        /// <summary>
        /// The kind of this <see cref="Room"/>.
        /// </summary>
        public RoomKind Kind { get; }

        /// <summary>
        /// The hitboxes of the walls of this <see cref="Room"/>.<br></br>
        /// The first index specifies the direction (0=top, 1=right, 2=bottom, 3=left).
        /// </summary>
        public Rectangle[] Walls { get; } = new Rectangle[4];

        /// <summary>
        /// The doors of this <see cref="Room"/>.<br></br>
        /// The index specifies the <see cref="Door"/>'s <see cref="Directions"/>.
        /// </summary>
        public Door[] Doors
        {
            get
            {
                return _doors;
            }
            set
            {
                // Store the value in _doors.
                for (byte i = 0; i < value.Length; i++)
                {
                    _doors[i] = value[i];
                }

                // Remove all Doors from _gameObjects.
                for (ushort i = 0; i < _gameObjects.Count; i++)
                {
                    if (_gameObjects[i].GetType().IsSubclassOf(typeof(Door)))
                    {
                        _gameObjects.Remove(_gameObjects[i]);
                    }
                }

                // Add the new Doors.
                for (byte i = 0; i < _doors.Length; i++)
                {
                    if (_doors[i] != null)
                    {
                        _gameObjects.Add(_doors[i]);
                    }
                }
            }
        }
        /// <summary>
        /// The doors of this room.
        /// </summary>
        private Door[] _doors = new Door[4];

        /// <summary>
        /// The entities of this <see cref="Room"/>.
        /// </summary>
        public List<Entity> Entities
        {
            get
            {
                // The entities.
                List<Entity> entities = new List<Entity>();

                // Add all Entities to entities.
                foreach (GameObject gameObject in _gameObjects)
                {
                    if (gameObject.GetType().IsSubclassOf(typeof(Entity)))
                    {
                        entities.Add((Entity)gameObject);
                    }
                }

                // Return the entities.
                return entities;
            }
        }

        /// <summary>
        /// The enemies of this <see cref="Room"/>.
        /// </summary>
        public List<Enemy> Enemies
        {
            get
            {
                // The enemies.
                List<Enemy> enemies = new List<Enemy>();

                // Add all Enemies to enemies.
                foreach (GameObject gameObject in _gameObjects)
                {
                    if (gameObject.GetType().IsSubclassOf(typeof(Enemy)))
                    {
                        enemies.Add((Enemy)gameObject);
                    }
                }

                // Return the enemies.
                return enemies;
            }
        }

        /// <summary>
        /// Creates a new room by the given paramters.
        /// </summary>
        /// <param name="roomIndex">The room's index which is used to load the room.</param>
        /// <param name="position">The room's grid position in the level.</param>
        /// <param name="kind">The kind of the door.</param>
        public Room(byte roomIndex, Vector2 gridPosition, RoomKind kind)
        {
            // Store the parameters.
            Position = gridPosition * Globals.WindowDimensions;
            Kind = kind;

            // Initialize the wall hitboxes.
            Walls[0] = new Rectangle((Position + new Vector2(0.5f, 0f) * Tile.Size * Globals.Scale).ToPoint(), (new Vector2(Dimensions.X, 1) * Tile.Size * Globals.Scale).ToPoint());
            Walls[1] = new Rectangle((Position + new Vector2(0.5f + (Dimensions.X - 1), 0f) * Tile.Size * Globals.Scale).ToPoint(), (new Vector2(1, Dimensions.Y) * Tile.Size * Globals.Scale).ToPoint());
            Walls[2] = new Rectangle((Position + new Vector2(0.5f, Dimensions.Y - 1) * Tile.Size * Globals.Scale).ToPoint(), (new Vector2(Dimensions.X, 1) * Tile.Size * Globals.Scale).ToPoint());
            Walls[3] = new Rectangle((Position + new Vector2(0.5f, 0f) * Tile.Size * Globals.Scale).ToPoint(), (new Vector2(1, Dimensions.Y) * Tile.Size * Globals.Scale).ToPoint());

            // Initialize _topLeftCorner.
            _topLeftCorner = Position + new Vector2(1, 0.5f) * Tile.Size * Globals.Scale;

            // Initialize the room texture.
            if (_roomTexture == null)
            {
                // Create the texture.
                Vector2 textureSize = new Vector2(15, 9) * Tile.TextureSize;
                _roomTexture = new Texture2D(Globals.Graphics.GraphicsDevice, (int)textureSize.X, (int)textureSize.Y);

                // Declare the colour data arrays.
                Color[] ground, wallT, wallR, wallB, wallL, cornerTL, cornerTR, cornerBR, cornerBL;
                int pixelCount = (int)(Tile.TextureSize.X * Tile.TextureSize.Y);
                Rectangle sourceRect = new Rectangle(Vector2.Zero.ToPoint(), Tile.TextureSize.ToPoint());
                ground = new Color[pixelCount]; wallT = new Color[pixelCount];
                wallR = new Color[pixelCount]; wallB = new Color[pixelCount];
                wallL = new Color[pixelCount]; cornerTL = new Color[pixelCount];
                cornerTR = new Color[pixelCount]; cornerBR = new Color[pixelCount];
                cornerBL = new Color[pixelCount];

                // Get the colour data of the textures.
                Globals.Content.Load<Texture2D>("Sprites/Environment/Boden").GetData(0, sourceRect, ground, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/WandT").GetData(0, sourceRect, wallT, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/WandR").GetData(0, sourceRect, wallR, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/WandB").GetData(0, sourceRect, wallB, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/WandL").GetData(0, sourceRect, wallL, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/EckeTL").GetData(0, sourceRect, cornerTL, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/EckeTR").GetData(0, sourceRect, cornerTR, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/EckeBR").GetData(0, sourceRect, cornerBR, 0, pixelCount);
                Globals.Content.Load<Texture2D>("Sprites/Environment/EckeBL").GetData(0, sourceRect, cornerBL, 0, pixelCount);

                // Create the texture.
                Vector2 topLeftTextureCorner = new Vector2(0f) * Tile.TextureSize;
                Rectangle destinationRect = new Rectangle(topLeftTextureCorner.ToPoint(), Tile.TextureSize.ToPoint());
                // The corners.
                _roomTexture.SetData(0, destinationRect, cornerTL, 0, pixelCount);
                destinationRect.Location = (topLeftTextureCorner + new Vector2(Dimensions.X - 1, 0) * Tile.TextureSize).ToPoint();
                _roomTexture.SetData(0, destinationRect, cornerTR, 0, pixelCount);
                destinationRect.Location = (topLeftTextureCorner + (Dimensions - Vector2.One) * Tile.TextureSize).ToPoint();
                _roomTexture.SetData(0, destinationRect, cornerBR, 0, pixelCount);
                destinationRect.Location = (topLeftTextureCorner + new Vector2(0, Dimensions.Y - 1) * Tile.TextureSize).ToPoint();
                _roomTexture.SetData(0, destinationRect, cornerBL, 0, pixelCount);
                // The walls.
                // Top and bottom.
                for (byte x = 1; x < Dimensions.X - 1; x++)
                {
                    // Top.
                    destinationRect.Location = (topLeftTextureCorner + new Vector2(x, 0) * Tile.TextureSize).ToPoint();
                    _roomTexture.SetData(0, destinationRect, wallT, 0, pixelCount);

                    // Bottom.
                    destinationRect.Location = (topLeftTextureCorner + new Vector2(x, Dimensions.Y - 1) * Tile.TextureSize).ToPoint();
                    _roomTexture.SetData(0, destinationRect, wallB, 0, pixelCount);
                }
                // Left and right.
                for (byte y = 1; y < Dimensions.Y - 1; y++)
                {
                    // Left.
                    destinationRect.Location = (topLeftTextureCorner + new Vector2(0, y) * Tile.TextureSize).ToPoint();
                    _roomTexture.SetData(0, destinationRect, wallL, 0, pixelCount);

                    // Right.
                    destinationRect.Location = (topLeftTextureCorner + new Vector2(Dimensions.X - 1, y) * Tile.TextureSize).ToPoint();
                    _roomTexture.SetData(0, destinationRect, wallR, 0, pixelCount);
                }
                // The ground.
                for (byte x = 1; x < Dimensions.X - 1; x++)
                {
                    for (byte y = 1; y < Dimensions.Y - 1; y++)
                    {

                        destinationRect.Location = (topLeftTextureCorner + new Vector2(x, y) * Tile.TextureSize).ToPoint();
                        _roomTexture.SetData(0, destinationRect, ground, 0, pixelCount);
                    }
                }
            }

            // Set the room sprite.
            _roomSprite = new Sprite(texture: _roomTexture,
                                     position: Position + new Vector2(0.5f, 0) * Tile.Size,
                                     origin: Vector2.Zero,
                                     scale: Tile.Size / Tile.TextureSize,
                                     layerDepth: 1f);

            // Add the room sprite.
            _gameObjects.Add(_roomSprite);

            // Add the room's content.
            switch (Kind)
            {
                case RoomKind.Start:
                    if (Level.LevelIndex == 0)
                    {
                        // Add controls instructions in the centre of the room.
                        Texture2D controlsInstructions = Globals.Content.Load<Texture2D>("Sprites/Misc/ControlsInstructions");
                        _gameObjects.Add(new Sprite(texture: controlsInstructions,
                                                    position: Position + (Dimensions / 2 + new Vector2(0.5f, 0)) * Tile.Size,
                                                    scale: new Vector2(5) * (Tile.Size / controlsInstructions.Bounds.Size.ToVector2()) * Globals.Scale,
                                                    layerDepth: 0.99999f));
                    }
                    break;
                case RoomKind.Normal:
                    // Load the content from a file by using the roomIndex.
                    _gameObjects.AddRange(RoomfileToList(RoomKind.Normal, roomIndex));
                    break;
                case RoomKind.Hidden:
                    // Load the content from a file by using the roomIndex.
                    _gameObjects.AddRange(RoomfileToList(RoomKind.Hidden, roomIndex));
                    break;
                case RoomKind.Boss:
                    // Load the content from a file by using the roomIndex.
                    _gameObjects.AddRange(RoomfileToList(RoomKind.Boss, roomIndex));
                    break;
            }
        }

        /// <summary>
        /// A method to add a <see cref="GameObject"/> to this <see cref="Room"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> that will be added.</param>
        public void Add(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes the given <see cref="GameObject"/> from this Room.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> that will be removed.</param>
        public void Remove(GameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }

        /// <summary>
        /// Converts the .room file of the given <see cref="RoomKind"/> and number to a List of <see cref="GameObject"/>s.
        /// </summary>
        /// <param name="roomKind">The given room kind.</param>
        /// <param name="roomIndex">The given room index</param>
        /// <returns>A List of all GameObjects, that are specified in the .room file.</returns>
        private List<GameObject> RoomfileToList(RoomKind roomKind, byte roomIndex)
        {
            // The list of GameObjects.
            List<GameObject> gameObjects = new List<GameObject>();

            // Get the lines of the file.
            string[] lines = File.ReadAllLines($"..\\..\\..\\Content\\Rooms\\{roomKind}\\{roomKind.ToString().ToLower()}_{roomIndex}.room");

            // Go through all lines.
            for (byte y = 0; y < 9; y++)
            {
                // Split the line in substrings at every space (' ').
                string[] splittedLine = lines[y].Split(' ');

                // Get and add all GameObjects of this line.
                byte index, metadata;
                GameObject loadedGameObject;
                for (byte x = 0; x < 15; x++)
                {
                    // Reset the loaded game object.
                    loadedGameObject = null;

                    // Get the index and metadata.
                    // If the metadata is defined.
                    if (splittedLine[x].Contains(":"))
                    {
                        // Split the string again.
                        string[] values = splittedLine[x].Split(':');

                        // Get the index and metadata.
                        index = Byte.Parse(values[0]);
                        metadata = Byte.Parse(values[1]);
                    }
                    // Else the metadata isn't defined.
                    else
                    {
                        // Get the index and set the metadata to 0.
                        index = Byte.Parse(splittedLine[x]);
                        metadata = 0;
                    }

                    // Get the corresponding GameObject.
                    switch (index)
                    {
                        // Nothing.
                        case 0:
                            // Add nothing.
                            break;
                        // Enemy.
                        case 1:
                            switch (metadata)
                            {
                                // Exploder.
                                case 0:
                                    loadedGameObject = new Exploder(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Floater.
                                case 1:
                                    loadedGameObject = new Floater(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Fly.
                                case 2:
                                    loadedGameObject = new Fly(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Flyboss.
                                case 3:
                                    loadedGameObject = new Flyboss(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Flytrap.
                                case 4:
                                    loadedGameObject = new Flytrap(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Screamer.
                                case 5:
                                    loadedGameObject = new Screamer(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                            }
                            break;
                        // Environment.
                        case 2:
                            switch (metadata)
                            {
                                // Campfire.
                                case 0:
                                    loadedGameObject = new Campfire(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Chest.
                                case 1:
                                    loadedGameObject = new Chest(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Hole.
                                case 2:
                                    loadedGameObject = new Hole(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Poop.
                                case 3:
                                    loadedGameObject = new Poop(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Pot.
                                case 4:
                                    loadedGameObject = new Pot(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Rock.
                                case 5:
                                    loadedGameObject = new Rock(_topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                            }
                            break;
                        // Itemstone.
                        case 3:
                            switch (metadata)
                            {
                                // Bomb.
                                case 0:
                                    loadedGameObject = new Itemstone(new PickupBomb(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Coin.
                                case 1:
                                    loadedGameObject = new Itemstone(new PickupCoin(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Heart.
                                case 2:
                                    loadedGameObject = new Itemstone(new PickupHeart(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Key.
                                case 3:
                                    loadedGameObject = new Itemstone(new PickupKey(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Poopsicle.
                                case 4:
                                    loadedGameObject = new Itemstone(new Poopsicle(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Shroom.
                                case 5:
                                    loadedGameObject = new Itemstone(new Shroom(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                                // Syringe.
                                case 6:
                                    loadedGameObject = new Itemstone(new Syringe(), _topLeftCorner + new Vector2(x, y) * Tile.Size * Globals.Scale);
                                    break;
                            }
                            break;
                    }

                    // Add the GameObject to gameObjects.
                    if (loadedGameObject != null)
                    {
                        gameObjects.Add(loadedGameObject);
                    }
                }
            }

            // Return the GameObject List.
            return gameObjects;
        }

        public static void SaveRoom()
        {
            XDocument xml = new XDocument(new XElement("Root",
                                                        new XElement("Room", "")));

                // 9 lines
            for (int i = 0; i < 9; i++)
            {
                // 15 columns
                for (int j = 0; j < 15; j++)
                {
                    //Level.CurrentRoom._gameObjects[i][j]
                }

            }

            Globals.save.HandleSaveFormates(xml, Level.LevelIndex.ToString() + Level.CurrentRoom.Position.ToString() + ".xml");
        }
    }
}