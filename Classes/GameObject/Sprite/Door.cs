using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// Contains the different kinds of <see cref="Door"/>s.
    /// </summary>
    public enum DoorKind : byte
    {
        Normal, 
        Boss, 
        Hidden
    }

    /// <summary>
    /// Contains the possible states of a <see cref="Door"/>.
    /// </summary>
    public enum DoorState : byte
    {
        Open, 
        Closed, 
        Locked
    }

    /// <summary>
    /// A Door to another <see cref="Room"/>.
    /// </summary>
    public class Door : Sprite
    {
        /// <summary>
        /// The closing animations of all kinds of rooms.
        /// </summary>
        private Animation[] _closeAnimations = 
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Environment/DoorSheet"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(125),
                          repetitions: 0),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Environment/BossDoorSheet"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(125),
                          repetitions: 0),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Environment/HiddenDoorSheet"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(125),
                          repetitions: 0)
        };
        /// <summary>
        /// The texture of a locked door.
        /// </summary>
        private static Texture2D _lockedDoor = Globals.Content.Load<Texture2D>("Sprites/Environment/LockedDoor");

        /// <summary>
        /// The inner width of a door.
        /// </summary>
        public static byte Width { get; } = 50;

        /// <summary>
        /// The direction in which the <see cref="Door"/> leads.
        /// </summary>
        public Directions Direction { get => _direction; }
        /// <summary>
        /// The direction in which the Door leads.
        /// </summary>
        private Directions _direction;

        /// <summary>
        /// The kind of this <see cref="Door"/>.
        /// </summary>
        public DoorKind Kind { get; }

        /// <summary>
        /// The state of this <see cref="Door"/>.
        /// </summary>
        public DoorState State { get; set; }

        /// <summary>
        /// A "poof" animation.
        /// </summary>
        private static readonly Animation _poofAnimation = new Animation(Globals.Content.Load<Texture2D>("Sprites/Effects/Poofsheet"),
                                                                         new Vector2(256),
                                                                         TimeSpan.FromMilliseconds(80),
                                                                         repetitions: 0);
        private Sprite _poofAnimationSprite = null;

        /// <summary>
        /// Creates a new <see cref="Door"/> with the given position, rotation and <see cref="Room"/>.
        /// </summary>
        /// <param name="position">The position of the <see cref="Door"/>.</param>
        /// <param name="direction">
        /// The direction in which the <see cref="Door"/> leads.
        /// </param>
        /// <param name="kindOfDoor">The kind of this <see cref="Door"/>.</param>
        /// <param name="doorState">
        /// The initial state of this <see cref="Door"/>.<br></br>
        /// Hidden doors are always closed initially.
        /// </param>
        public Door(Vector2 position,
                    Directions direction,
                    DoorKind kindOfDoor = DoorKind.Normal,
                    DoorState doorState = DoorState.Open)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Doorsheet"),
               position: position,
               origin: new Vector2(0.5f),
               sourceRectangle: new Rectangle(new Point(0), new Point(256)),
               rotation: (byte)direction * 90f,
               scale: Tile.Size / new Vector2(256),
               layerDepth: 0.95f)
        {
            // Store the parameters. 
            _direction = direction;
            Kind = kindOfDoor;
            State = (Kind != DoorKind.Hidden) ? doorState : DoorState.Locked;

            // If this door is locked.
            if (State == DoorState.Locked)
            {
                // If it's a normal door.
                if (Kind == DoorKind.Normal)
                {
                    // There's no animation.
                    CurrentAnimation = null;

                    // Set the locked door texture.
                    Texture = _lockedDoor;
                    SourceRectangle = new Rectangle(0, 0, 256, 256);
                }
                // Else if it's a hidden door.
                else if (Kind == DoorKind.Hidden)
                {
                    // The hidden door closed frame.
                    Texture = _closeAnimations[(byte)DoorKind.Hidden].GetFrameTexture(1);
                }
            }
            else
            {
                // Set the initial animation.
                CurrentAnimation = _closeAnimations[(byte)Kind];
                CurrentAnimation.IsReversed = (State == DoorState.Open);
            }
        }

        public override void Update()
        {
            // If this Door is open.
            if (State == DoorState.Open)
            {
                // Did the Player go through this Door?
                bool wentThroughDoor = false;
                switch (_direction)
                {
                    case Directions.Up:
                        if (Level.Player.Position.Y < Position.Y)
                        {
                            wentThroughDoor = true;
                        }
                        break;
                    case Directions.Right:
                        if (Level.Player.Position.X > Position.X)
                        {
                            wentThroughDoor = true;
                        }
                        break;
                    case Directions.Down:
                        if (Level.Player.Position.Y > Position.Y)
                        {
                            wentThroughDoor = true;
                        }
                        break;
                    case Directions.Left:
                        if (Level.Player.Position.X < Position.X)
                        {
                            wentThroughDoor = true;
                        }
                        break;
                }

                // Initiate the room change if the player went through this door.
                if (wentThroughDoor)
                {
                    Level.SwitchRoom(_direction);
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

        /// <summary>
        /// Opens this <see cref="Door"/>.
        /// </summary>
        public void Open()
        {
            // Open the door.
            State = DoorState.Open;
            // Set the animation.
            CurrentAnimation = _closeAnimations[(byte)Kind];
            // Start opening animation.
            CurrentAnimation.IsReversed = true;
            CurrentAnimation.Restart();
        }

        /// <summary>
        /// Closes this <see cref="Door"/>.
        /// </summary>
        public void Close()
        {
            // Close the Door.
            State = DoorState.Closed;
            // Set the animation.
            CurrentAnimation = _closeAnimations[(byte)Kind];
            // Start closing animation.
            CurrentAnimation.IsReversed = false;
            CurrentAnimation.Restart();
        }

        /// <summary>
        /// Unlocks this <see cref="Door"/>.
        /// </summary>
        public void Unlock(bool unlockCounterpart = false)
        {
            // Unlock this door.
            State = DoorState.Open;
            // Set the animation.
            CurrentAnimation = _closeAnimations[(byte)Kind];
            CurrentAnimation.IsReversed = true;
            CurrentAnimation.Restart();

            // If the counterpart shall be unlocked.
            if (unlockCounterpart)
            {
                // Unlock the counterpart in adjacent room.
                Level.UnlockCounterpartDoor(this);
            }

            // If this Door is hidden.
            if (Kind == DoorKind.Hidden)
            {
                // Restart _poofAnimation.
                _poofAnimation.Restart();
                // Set and add _poofAnimationSprite.
                _poofAnimationSprite = new Sprite(animation: _poofAnimation,
                                                  position: Position,
                                                  origin: new Vector2(0.5f),
                                                  scale: Tile.Size / new Vector2(256));
                Level.CurrentRoom.Add(_poofAnimationSprite);
            }
        }

        /// <summary>
        /// Locks this <see cref="Door"/>.
        /// </summary>
        public void Lock(bool lockCounterpart = false)
        {
            // Lock this door.
            State = DoorState.Locked;

            // Set the animation.
            // If it's a normal door.
            if (Kind == DoorKind.Normal)
            {
                // There's no animation.
                CurrentAnimation = null;

                // Set the locked door texture.
                Texture = _lockedDoor;
                SourceRectangle = new Rectangle(0, 0, 256, 256);
            }
            else
            {
                // Set the animation.
                CurrentAnimation = _closeAnimations[(byte)Kind];
            }

            // If the counterpart shall be locked.
            if (lockCounterpart)
            {
                // Lock the counterpart in adjacent room.
                Level.LockCounterpartDoor(this);
            }
        }
    }
}
