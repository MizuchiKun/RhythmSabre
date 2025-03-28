using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;

namespace ProjektRoguelike
{
    /// <summary>
    /// The player.
    /// </summary>
    public class Player : Entity
    {
        McTimer timer, damageImunity;
        public int HealthMax;
        public bool poopsicle = false;

        public static ushort speed;

        public int itemcount;

        /// <summary>
        /// List of all the companions aiding around the player
        /// </summary>
        public static List<Flybuddy> Companions { get; set; } = new List<Flybuddy>();

        /// <summary>
        /// The damage the player deals to entities
        /// </summary>
        public int HitValue { get; set; } = 1;

        /// <summary>
        /// The gold the player owns. Might be used for shops etc. later.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// The bombs the player has at its disposal.
        /// </summary>
        public int Bombs { get; set; }

        /// <summary>
        /// The keys the player owns. Used for certain doors and chests.
        /// </summary>
        public int Keys { get; set; }

        /// <summary>
        /// The items the player has picked up this run.
        /// </summary>
        public List<Item> items = new List<Item>();

        /// <summary>
        /// The walking animations of the Player.<br></br><br></br>
        /// Use <see cref="Directions"/> as indices.
        /// </summary>
        private static Animation[] _walkingAnimations =
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Playable Char/Herosheet_up"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Playable Char/Herosheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150),
                          effects: SpriteEffects.FlipHorizontally),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Playable Char/Herosheet_down"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Playable Char/Herosheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150))
        };

        private static SoundEffectInstance _walkingSound;

        /// <summary>
        /// Creates a Player with the given graphical parameters.
        /// </summary>
        /// <param name="texture">Its texture.</param>
        /// <param name="position">Its position. <br></br>If null, it will be <see cref="Vector2.Zero"/>.</param>
        /// <param name="sourceRectangle">Its source rectangle. <br></br>If null, the whole texture will be drawn.</param>
        /// <param name="layerDepth">Its layer depth. <br></br>It's 0 by default.</param>
        /// <param name="effect">Its sprite effect. <br></br>It's <see cref="SpriteEffects.None"/> by default.</param>
        public Player(Vector2? position = null)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Playable Char/Herosheet_down"),
               position: position,
               sourceRectangle: new Rectangle(0, 0, 256, 256))
        {
            timer = new McTimer(600, true);
            damageImunity = new McTimer(500, true);

            // The players current velocity per second.
            _velocity = Vector2.Zero;

            Health = 15;
            HealthMax = Health;

            speed = 400;

            // Set the initial animation.
            CurrentAnimation = _walkingAnimations[2];

            // Initialize the walking sound effect.
            if (_walkingSound == null)
            {
                _walkingSound = Globals.Content.Load<SoundEffect>("Sounds/SoundEffect/Sound4").CreateInstance();
                _walkingSound.Volume = 0.5f * Globals.sounds.sfxVolume * Globals.sounds.sfxToMusicRatio;
                _walkingSound.IsLooped = false;
            }
        }

        /// <summary>
        /// The Update method of a Player. Handles input and "stuff".
        /// </summary>
        public override void Update()
        {
            // if the players HP is 0, thus is dead, change the gamestate to dead
            if (Health <= 0)
            {
                Globals.gamestate = Gamestate.Dead;
            }

            // pressing the P key will pause the game and thus stop updating the game (unpausing in Game1.Update())
            if (Globals.GetKeyUp(Microsoft.Xna.Framework.Input.Keys.P))
            {
                Globals.gamestate = Gamestate.Paused;
            }

            //activating itemeffects on buttonpress for testing
            if (Globals.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.K))
            {
                poopsicle = true;
            }

            //just boomer things
            if (Globals.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && Level.Player.Bombs > 0)
            {
                //Level.CurrentRoom.Add(new Explosion(Position));
                Level.CurrentRoom.Add(new Bomb(Position));
                Level.Player.Bombs--;

                Globals.sounds.PlaySoundEffect("Sound8");
            }

            //testing items and such
            if (Globals.GetKeyUp(Microsoft.Xna.Framework.Input.Keys.J))
            {
                //Level.CurrentRoom.Add(new Itemstone(new Syringe(Level.CurrentRoom.Position + (Room.Dimensions / 5) * Tile.Size * Globals.Scale),
                //                                               Level.CurrentRoom.Position + (Room.Dimensions / 5) * Tile.Size * Globals.Scale));

                Level.CurrentRoom.Add(new Pot(Level.CurrentRoom.Position + (Room.Dimensions / 5) * Tile.Size * Globals.Scale));
                //Level.Player.Keys += 1;
            }

            //testing environment and enemies
            if (Globals.GetKeyUp(Microsoft.Xna.Framework.Input.Keys.L))
            {
                Level.CurrentRoom.Add(new Flyboss(Level.CurrentRoom.Position + (Room.Dimensions / 3) * Tile.Size * Globals.Scale));
                //Level.CurrentRoom.Add(new Itemstone(new Heart(Level.CurrentRoom.Position + (Room.Dimensions / 3) * Tile.Size * Globals.Scale),
                //                                               Level.CurrentRoom.Position + (Room.Dimensions / 3) * Tile.Size * Globals.Scale));
            }








            // effect used, when the player picks up the Poopsicle item. Spawn 3 flies, that are orbiting around you.
            if (poopsicle)
            {
                Companions.Add(new Flybuddy(new Vector2(Position.X, Position.Y + 55), 0));
                Companions.Add(new Flybuddy(new Vector2(Position.X + 40, Position.Y - 40)));
                Companions.Add(new Flybuddy(new Vector2(Position.X - 40, Position.Y - 40)));
                poopsicle = false;
            }

            // Handle movement input.
            // The movement speed.
            //ushort speed = 350;
            // The velocity.
            _velocity = Vector2.Zero;
            // Up.
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.W))
            {
                // Play the movement sound.
                //Globals.sounds.PlaySoundEffectOnce("Sound3");
                // Move it up.
                _velocity += -Vector2.UnitY * speed;

                // If there's a top door.
                if (Level.CurrentRoom.Doors[(byte)Directions.Up] != null)
                {
                    // If it touches the top door, it's hidden and the player has more than 0 keys.
                    if (!(Level.CurrentRoom.Doors[(byte)Directions.Up].Kind == DoorKind.Hidden)
                        && (BumpsInto(Level.CurrentRoom.Doors[(byte)Directions.Up])
                            && Math.Abs(Level.CurrentRoom.Doors[(byte)Directions.Up].Position.X - Position.X) <= Door.Width * Scale.X)
                        && Level.CurrentRoom.Doors[(byte)Directions.Up].State == DoorState.Locked
                        && Keys > 0)
                    {
                        // The player uses a key.
                        Keys--;
                        // Unlock the door.
                        Level.CurrentRoom.Doors[(byte)Directions.Up].Unlock(true);
                    }
                }
            }
            // Right.
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.D))
            {
                // Play the movement sound.
                //Globals.sounds.PlaySoundEffectOnce("Sound2");
                // Move it right.
                _velocity += Vector2.UnitX * speed;

                // If there's a right door.
                if (Level.CurrentRoom.Doors[(byte)Directions.Right] != null)
                {
                    // If it touches the right door, it's hidden and the player has more than 0 keys.
                    if (!(Level.CurrentRoom.Doors[(byte)Directions.Right].Kind == DoorKind.Hidden)
                        && (BumpsInto(Level.CurrentRoom.Doors[(byte)Directions.Right])
                            && Math.Abs(Level.CurrentRoom.Doors[(byte)Directions.Right].Position.Y - Position.Y) <= Door.Width * Scale.Y)
                        && Level.CurrentRoom.Doors[(byte)Directions.Right].State == DoorState.Locked
                        && Keys > 0)
                    {
                        // The player uses a key.
                        Keys--;
                        // Unlock the door.
                        Level.CurrentRoom.Doors[(byte)Directions.Right].Unlock(true);
                    }
                }
            }
            // Down.
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.S))
            {
                // Play the movement sound.
                //Globals.sounds.PlaySoundEffectOnce("Sound4");
                // Move it down.
                _velocity += Vector2.UnitY * speed;

                // If there's a bottom door.
                if (Level.CurrentRoom.Doors[(byte)Directions.Down] != null)
                {
                    // If it touches the bottom door, it's hidden and the player has more than 0 keys.
                    if (!(Level.CurrentRoom.Doors[(byte)Directions.Down].Kind == DoorKind.Hidden)
                        && (BumpsInto(Level.CurrentRoom.Doors[(byte)Directions.Down])
                            && Math.Abs(Level.CurrentRoom.Doors[(byte)Directions.Down].Position.X - Position.X) <= Door.Width * Scale.X)
                        && Level.CurrentRoom.Doors[(byte)Directions.Down].State == DoorState.Locked
                        && Keys > 0)
                    {
                        // The player uses a key.
                        Keys--;
                        // Unlock the door.
                        Level.CurrentRoom.Doors[(byte)Directions.Down].Unlock(true);
                    }
                }
            }
            // Left.
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.A))
            {
                // Play the movement sound.
                //Globals.sounds.PlaySoundEffectOnce("Sound1");
                // Move it left.
                _velocity += -Vector2.UnitX * speed;

                // If there's a left door.
                if (Level.CurrentRoom.Doors[(byte)Directions.Left] != null)
                {
                    // If it touches the left door, it's hidden and the player has more than 0 keys.
                    if (!(Level.CurrentRoom.Doors[(byte)Directions.Left].Kind == DoorKind.Hidden)
                        && (BumpsInto(Level.CurrentRoom.Doors[(byte)Directions.Left])
                            && Math.Abs(Level.CurrentRoom.Doors[(byte)Directions.Left].Position.Y - Position.Y) <= Door.Width * Scale.Y)
                        && Level.CurrentRoom.Doors[(byte)Directions.Left].State == DoorState.Locked
                        && Keys > 0)
                    {
                        // The player uses a key.
                        Keys--;
                        // Unlock the door.
                        Level.CurrentRoom.Doors[(byte)Directions.Left].Unlock(true);
                    }
                }
            }
            // Choose the proper animation.
            if (Math.Abs(_velocity.X) > Math.Abs(_velocity.Y))
            {
                // If it moves left.
                if (_velocity.X < 0)
                {
                    CurrentAnimation = _walkingAnimations[3];
                }
                // Else it moves right.
                else
                {
                    CurrentAnimation = _walkingAnimations[1];
                }
            }
            // Else it moves up, down or diagonally.
            else
            {
                // If it moves up.
                if (_velocity.Y < 0)
                {
                    CurrentAnimation = _walkingAnimations[0];
                }
                // Else it moves down.
                else if (_velocity.Y > 0)
                {
                    CurrentAnimation = _walkingAnimations[2];
                }
            }
            // Move it.
            if (_velocity != Vector2.Zero)
            {
                //Globals.sounds.PlaySoundEffectOnce("Sound4");
                // Play the walking sound effect.
                if (_walkingSound.State != SoundState.Playing)
                {
                    _walkingSound.Play();
                }
                CurrentAnimation.Resume();
                _velocity = Globals.DegreesToVector2(Globals.Vector2ToDegrees(_velocity)) * speed;
                Move(_velocity);
            }
            else
            {
                CurrentAnimation.Pause();
                CurrentAnimation.SelectFrame(0);
            }


            // handle combat inputs

            damageImunity.UpdateTimer();
            timer.UpdateTimer();
            // up
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.Up) && timer.Test())
            {
                Globals.sounds.PlaySoundEffect("Attack1");
                Level.CurrentRoom.Add(new BasicAttack(0 - 90, Position));
                timer.ResetToZero();
            }
            // right
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.Right) && timer.Test())
            {
                Globals.sounds.PlaySoundEffect("Attack1");
                Level.CurrentRoom.Add(new BasicAttack(90 - 90, Position));
                timer.ResetToZero();
            }
            // down
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.Down) && timer.Test())
            {
                Globals.sounds.PlaySoundEffect("Attack1");
                Level.CurrentRoom.Add(new BasicAttack(180 - 90, Position));
                timer.ResetToZero();
            }
            // left
            if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.Left) && timer.Test())
            {
                Globals.sounds.PlaySoundEffect("Attack1");
                Level.CurrentRoom.Add(new BasicAttack(270 - 90, Position));
                timer.ResetToZero();
            }

            // Draw the list of companions
            for (int i = 0; i < Companions.Count; i++)
            {
                Companions[i].Update();
                if (Companions[i].isDestroyed)
                {
                    Companions.RemoveAt(i);
                }
            }

            // Update the Player's layer and stuff.
            base.Update();
        }

        /// <summary>
        /// The <see cref="Player"/>'s Draw method.
        /// </summary>
        public override void Draw()
        {
            // Draw the list of companions
            for (int i = 0; i < Companions.Count; i++)
            {
                Companions[i].Draw();
            }

            //testing string to look how much damage something does
            //Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Seed.ToString(), new Vector2(Position.X, Position.Y - 75), Color.White);

            base.Draw();
        }

        public override void GetHit(int hitValue)
        {
            // check if you were hit in the last x seconds, and decide if youre ready to be hit again.
            if (damageImunity.Test())
            {
                // receive damage
                base.GetHit(hitValue);
                damageImunity.ResetToZero();
                Globals.sounds.PlaySoundEffect("ONEECHAN");
            }
            else
            {
                //ignore it
            }
        }

        protected override bool CanMove(Directions direction)
        {
            // If it collides with a door and is within the door frame.
            if ((direction == Directions.Up
                 && ((Level.CurrentRoom.Doors[(byte)Directions.Up] != null
                      && Level.CurrentRoom.Doors[(byte)Directions.Up].State == DoorState.Open
                      && Math.Abs(Position.X - Level.CurrentRoom.Doors[(byte)Directions.Up].Position.X) <= Door.Width * Scale.X)))
                || (direction == Directions.Right
                    && ((Level.CurrentRoom.Doors[(byte)Directions.Right] != null
                      && Level.CurrentRoom.Doors[(byte)Directions.Right].State == DoorState.Open
                         && Math.Abs(Position.Y - Level.CurrentRoom.Doors[(byte)Directions.Right].Position.Y) <= Door.Width * Scale.Y)))
                || (direction == Directions.Down
                 && ((Level.CurrentRoom.Doors[(byte)Directions.Down] != null
                      && Level.CurrentRoom.Doors[(byte)Directions.Down].State == DoorState.Open
                      && Math.Abs(Position.X - Level.CurrentRoom.Doors[(byte)Directions.Down].Position.X) <= Door.Width * Scale.X)))
                || (direction == Directions.Left
                    && ((Level.CurrentRoom.Doors[(byte)Directions.Left] != null
                      && Level.CurrentRoom.Doors[(byte)Directions.Left].State == DoorState.Open
                         && Math.Abs(Position.Y - Level.CurrentRoom.Doors[(byte)Directions.Left].Position.Y) <= Door.Width * Scale.Y))))
            {
                // It's allowed to move.
                return true;
            }
            // Else if it collides with the top wall.
            else if (Collides(Level.CurrentRoom.Walls[(byte)direction]))
            {
                // Adjust the position.
                Vector2 position = Position;
                switch (direction)
                {
                    case Directions.Up:
                        position.Y = Level.CurrentRoom.Walls[(byte)Directions.Up].Location.Y + (1f * Tile.Size.Y);
                        break;
                    case Directions.Right:
                        position.X = Level.CurrentRoom.Walls[(byte)Directions.Right].Location.X - (0.5f * Tile.Size.X);
                        break;
                    case Directions.Down:
                        position.Y = Level.CurrentRoom.Walls[(byte)Directions.Down].Location.Y - (0.5f * Tile.Size.Y);
                        break;
                    case Directions.Left:
                        position.X = Level.CurrentRoom.Walls[(byte)Directions.Left].Location.X + (1.5f * Tile.Size.X);
                        break;
                }
                Position = position;
                // Then it's allowed to move.
                return true;
            }
            // Else if it collides with another Entity or the frame of a door.
            else if (Collides(Level.CurrentRoom.Entities)
                     || ((byte)direction % 2 == 0
                         && ((Level.CurrentRoom.Doors[1] != null && Collides(Level.CurrentRoom.Doors[1]))
                              || (Level.CurrentRoom.Doors[3] != null && Collides(Level.CurrentRoom.Doors[3])))
                         && ((Level.CurrentRoom.Doors[1] != null && Math.Abs(Position.Y - Level.CurrentRoom.Doors[1].Position.Y) > Door.Width * Scale.Y)
                             || (Level.CurrentRoom.Doors[3] != null && Math.Abs(Position.Y - Level.CurrentRoom.Doors[3].Position.Y) > Door.Width * Scale.Y)))
                     || ((byte)direction % 2 == 1
                         && ((Level.CurrentRoom.Doors[0] != null && Collides(Level.CurrentRoom.Doors[0]))
                              || (Level.CurrentRoom.Doors[2] != null && Collides(Level.CurrentRoom.Doors[2])))
                         && ((Level.CurrentRoom.Doors[0] != null && Math.Abs(Position.X - Level.CurrentRoom.Doors[0].Position.X) > Door.Width * Scale.X)
                             || (Level.CurrentRoom.Doors[2] != null && Math.Abs(Position.X - Level.CurrentRoom.Doors[2].Position.X) > Door.Width * Scale.X))))
            {
                // It's not allowed to move.
                return false;
            }

            // Default return.
            return true;
        }

        public void SaveData()
        {
            XDocument xml = new XDocument(new XElement("Root",
                                                        new XElement("Stats", "")));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "HealthMax"),
                                            new XElement("amount", HealthMax)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                                        new XElement("name", "Health"),
                                                        new XElement("amount", Health)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                                        new XElement("name", "speed"),
                                                        new XElement("amount", speed)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "HitValue"),
                                            new XElement("amount", HitValue)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "Bombs"),
                                            new XElement("amount", Bombs)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "Keys"),
                                            new XElement("amount", Keys)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "Gold"),
                                            new XElement("amount", Gold)));

            xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                            new XElement("name", "itemcount"),
                                            new XElement("amount", itemcount)));

            //xml.Element("Root").Element("Stats").Add(new XElement("Stat",
            //                    new XElement("name", "Seed"),
            //                    new XElement("amount", Level.Seed)));

            for (int i = 0; i < items.Count; i++)
            {
                xml.Element("Root").Element("Stats").Add(new XElement("Stat",
                                new XElement("name", "Item"),
                                new XElement("amount", items[i].GetType())));
            }

            items.Clear();

            Globals.save.HandleSaveFormates(xml, "stats.xml");
        }

        public void LoadData(XDocument data)
        {
            if (data != null)
            {
                List<XElement> statList = (from t in data.Element("Root").Element("Stats").Descendants("Stat")
                                            //where t.Element("name").Value ==
                                            select t).ToList<XElement>();

                HealthMax = Convert.ToInt32(statList[0].Element("amount").Value, Globals.culture);
                Health = Convert.ToInt32(statList[1].Element("amount").Value, Globals.culture);
                speed = Convert.ToUInt16(statList[2].Element("amount").Value, Globals.culture);
                HitValue = Convert.ToInt32(statList[3].Element("amount").Value, Globals.culture);
                Bombs = Convert.ToInt32(statList[4].Element("amount").Value, Globals.culture);
                Keys = Convert.ToInt32(statList[5].Element("amount").Value, Globals.culture);
                Gold = Convert.ToInt32(statList[6].Element("amount").Value, Globals.culture);
                itemcount = Convert.ToInt32(statList[7].Element("amount").Value, Globals.culture);
                //Level.Seed = Convert.ToInt32(statList[8].Element("amount").Value, Globals.culture);
                for (int i = 8; i < itemcount + 8; i++)
                {
                    if (statList[i].Element("amount").Value == "ProjektRoguelike.Syringe")
                    {
                        items.Add(new Syringe());
                    }
                    if (statList[i].Element("amount").Value == "ProjektRoguelike.Heart")
                    {
                        items.Add(new Heart());
                    }
                    if (statList[i].Element("amount").Value == "ProjektRoguelike.Poopsicle")
                    {
                        items.Add(new Poopsicle());
                    }
                    if (statList[i].Element("amount").Value == "ProjektRoguelike.Shroom")
                    {
                        items.Add(new Shroom());
                    }
                }
            }
        }
    }
}
