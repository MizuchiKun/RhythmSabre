using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{

    /// <summary>
    /// A Door to another <see cref="Level"/>.
    /// </summary>
    public class Trapdoor : Sprite
    {
        /// <summary>
        /// The inner width of a door.
        /// </summary>
        public static byte Width { get; } = 50;

        /// <summary>
        /// A "poof" animation.
        /// </summary>
        private static readonly Animation _poofAnimation = new Animation(Globals.Content.Load<Texture2D>("Sprites/Effects/Poofsheet"),
                                                                         new Vector2(256),
                                                                         TimeSpan.FromMilliseconds(80),
                                                                         repetitions: 0);
        private Sprite _poofAnimationSprite = null;

        /// <summary>
        /// Creates a new <see cref="Trapdoor"/> with the given position, rotation and <see cref="Room"/>.
        /// </summary>
        /// <param name="position">The position of the <see cref="Trapdoor"/>.</param>

        public Trapdoor(Vector2 position)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Trapdoor"),
               position: position,
               origin: new Vector2(0.5f),
               sourceRectangle: null,
               rotation: 0f,
               scale: Tile.Size / new Vector2(256),
               layerDepth: 0.95f)
        {
            _poofAnimationSprite = new Sprite(animation: _poofAnimation,
                                                 position: Position,
                                                 origin: new Vector2(0.5f),
                                                 scale: Tile.Size / new Vector2(256));
            Level.CurrentRoom.Add(_poofAnimationSprite);
        }

        public override void Update()
        {
            // Did the Player go through this Door?
            bool wentThroughDoor = false;
            if (Level.Player.BumpsInto(this))
            {
                wentThroughDoor = true;
            }

                // Initiate the level change if the player went through this trapdoor.
                if (wentThroughDoor)
                {
                    Level.LevelIndex += 1;
                    if (Level.LevelIndex >= 3)
                    {
                        Globals.gamestate = Gamestate.Win;
                        Globals.CurrentScene = new Victoryscreen();
                    }
                    else
                    {
                        Globals.CurrentScene = new Level(Level.LevelIndex);
                        Level.Player.Position = Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale;
                    }
                }

                // Remove _poofAnimationSprite if its animation is over.
                if (_poofAnimationSprite != null
                    && _poofAnimationSprite.CurrentAnimation.HasEnded)
                {
                    Level.CurrentRoom.Remove(_poofAnimationSprite);
                    _poofAnimationSprite = null;
                }
            }
        }
    }
