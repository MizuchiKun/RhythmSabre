using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public abstract class Enemy : Entity
    {
        /// <summary>
        /// The walking animations of the <see cref="Enemy"/>.<br></br>
        /// Use <see cref="Directions"/> as indices.
        /// </summary>
        protected abstract Animation[] _walkingAnimations { get; }

        protected float Speed;
        public int HitValue { get; set; } = 1;

        public Enemy(Texture2D texture,
                     Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture,
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            Speed = 2f;

            // Close all doors.
            // If this is the first Enemy in the current room.
            if (Level.CurrentRoom != null)
            {
                if (Level.CurrentRoom.Enemies.Count == 0)
                {
                    foreach (Door door in Level.CurrentRoom.Doors)
                    {
                        // If there's a door in that direction.
                        if (door != null)
                        {
                            // If the door is not hidden and open.
                            if (!(door.Kind == DoorKind.Hidden)
                                && door.State == DoorState.Open)
                            {
                                door.Close();
                            }
                        }
                    }
                }
            }
        }

        public override void Update()
        {
            AI();

            ChooseAnimation();

            base.Update();
        }

        public virtual void ChangePosition()
        {
            _velocity = Level.Player.Position - Position;
            // move towards the player
            Move(_velocity);
        }

        /// <summary>
        /// What the enemy is supposed to do. the only function in Update() is AI(), so change this to change Update().
        /// Base AI includes ChangePosition and CollidePlayer.
        /// </summary>
        public virtual void AI()
        {
            ChangePosition();
            CollidePlayer();
        }

        public override void GetHit(int hitValue)
        {
            Globals.sounds.PlaySoundEffect("GetHitEnemy");
            base.GetHit(hitValue);
        }

        public virtual bool HitWall()
        {
            // up
            if (Collides(Level.CurrentRoom.Walls[0]))
            {
                return true;
            }
            // right
            else if (Collides(Level.CurrentRoom.Walls[1]))
            {
                return true;
            }
            // down
            else if (Collides(Level.CurrentRoom.Walls[2]))
            {
                return true;
            }
            // left
            else if (Collides(Level.CurrentRoom.Walls[3]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Chooses the current animation.
        /// </summary>
        protected void ChooseAnimation()
        {
            // If there are 4 animations.
            if (_walkingAnimations.GetLength(0) == 4)
            {
                // Choose the proper animation.
                // If the horizontal velocity is dominant.
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
                // Else the vertical velocity is dominant.
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
            }
            // Else if there's only one.
            else if (_walkingAnimations.GetLength(0) == 1)
            {
                // Just choose that animation.
                CurrentAnimation = _walkingAnimations[0];
            }
        }

        public virtual void CollidePlayer()
        {                                                                 
        if (BumpsInto(Level.Player))                                        
            {
                //if (!Level.Player.Collides(Level.CurrentRoom.Walls[1])  
                //|| !Level.Player.Collides(Level.CurrentRoom.Walls[2])   
                //|| !Level.Player.Collides(Level.CurrentRoom.Walls[3])   
                //|| !Level.Player.Collides(Level.CurrentRoom.Walls[4]))
                /*
                //left
                for (int i = 0; i < Level.CurrentRoom.Walls[(byte)Directions.Up].Length; i++)
                {
                    if (Globals.GetDistance(Level.Player.Position, Level.CurrentRoom.Walls[(byte)Directions.Up][i].Position) > (Level.Player.Hitbox.Height * 1.2))
                    {
                        Level.Player.Position += -Globals.RadialMovement(Position, Level.Player.Position, Speed * 10);
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 10);
                    }
                    else
                    {
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 20);
                    }
                }

                //right
                for (int i = 0; i < Level.CurrentRoom.Walls[(byte)Directions.Right].Length; i++)
                {
                    if (Globals.GetDistance(Level.Player.Position, Level.CurrentRoom.Walls[(byte)Directions.Up][i].Position) > (Level.Player.Hitbox.Width * 1.2))
                    {
                        Level.Player.Position += -Globals.RadialMovement(Position, Level.Player.Position, Speed * 10);
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 10);
                    }
                    else
                    {
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 20);
                    }
                }

                //down
                for (int i = 0; i < Level.CurrentRoom.Walls[(byte)Directions.Down].Length; i++)
                {
                    if (Globals.GetDistance(Level.Player.Position, Level.CurrentRoom.Walls[(byte)Directions.Up][i].Position) > (Level.Player.Hitbox.Height * 1.2))
                    {
                        Level.Player.Position += -Globals.RadialMovement(Position, Level.Player.Position, Speed * 10);
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 10);
                    }
                    else
                    {
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 20);
                    }
                }

                //left
                for (int i = 0; i < Level.CurrentRoom.Walls[(byte)Directions.Left].Length; i++)
                {
                    if (Globals.GetDistance(Level.Player.Position, Level.CurrentRoom.Walls[(byte)Directions.Up][i].Position) > (Level.Player.Hitbox.Width * 1.2))
                    {
                        Level.Player.Position += -Globals.RadialMovement(Position, Level.Player.Position, Speed * 10);
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 10);
                    }
                    else
                    {
                        Position += -Globals.RadialMovement(Level.Player.Position, Position, Speed * 20);
                    }
                }
                */
                Level.Player.GetHit(HitValue);
            }
        }
    }
}
