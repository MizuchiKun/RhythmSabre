using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    
    /// <summary>
    /// An enemy
    /// </summary>
    public class Rock : Environment
    {
        public Rock(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Rock"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            Health = 2;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void Dropchance(int dropnumber)
        {
            if (dropnumber <= 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    Level.CurrentRoom.Add(new PickupCoin(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 4 && dropnumber <= 6)
            {
                for (int i = 0; i < 1; i++)
                {
                    Level.CurrentRoom.Add(new PickupHeart(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 6 && dropnumber <= 8)
            {
                for (int i = 0; i < 1; i++)
                {
                    Level.CurrentRoom.Add(new PickupBomb(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 8 && dropnumber <= 9)
            {
                for (int i = 0; i < 1; i++)
                {
                    Level.CurrentRoom.Add(new PickupKey(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else
            {
                Level.CurrentRoom.Add(new Fly(Position));
            }
        }
    }
}
