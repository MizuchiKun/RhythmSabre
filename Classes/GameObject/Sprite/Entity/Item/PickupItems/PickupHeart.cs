using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class PickupHeart : PickupItem
    {
        Vector2 direction;

        bool healedPlayer;

        public PickupHeart(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Pickups/PickupHeart"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            
        }

        public override void Update()
        {
            if (Level.Player.BumpsInto(this))
            {
                Effect();
                if (healedPlayer == true)
                {
                    Level.CurrentRoom.Remove(this);
                }
            }

            //if (Level.Player.BumpsInto(this))
            //{
            //    _velocity = Level.Player._velocity;
            //}
            //
            //if (Collides(Level.Player))
            //{
            //
            //    if (speed < _maxSpeed)
            //    {
            //        speed += _speedIncrease;
            //    }
            //    Position += speed * direction * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            //}
            //if (speed > 0)
            //{
            //    speed -= _speedIncrease;
            //}
        }

        // is made to increase players current healthpoints
        public override void Effect()
        {
            if (Level.Player.Health < Level.Player.HealthMax)
            {
                Level.Player.Health += 1;
                healedPlayer = true;
            }
            else
            {
                ChangePosition();
                //angle = Globals.Vector2ToDegrees(Position - Level.Player.Position);
                //direction = Globals.DegreesToVector2(-angle);
                //Position += Level.Player.speed * direction * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
