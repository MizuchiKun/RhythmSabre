using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    
    /// <summary>
    /// An enemy
    /// </summary>
    public class Poop : Environment
    {
        public Poop(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Poop"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            Health = 3;
        }

        public override void Update()
        {

            base.Update();
        }

        protected override void Dropchance(int dropnumber)
        {
            if (dropnumber <= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    Level.CurrentRoom.Add(new PickupHeart(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 3 && dropnumber <= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Level.CurrentRoom.Add(new PickupCoin(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 5 && dropnumber <= 6)
            {
                for (int i = 0; i < 2; i++)
                {
                    Level.CurrentRoom.Add(new PickupBomb(new Vector2(Position.X - (20 * i), Position.Y)));
                }
            }
            else if (dropnumber > 6 && dropnumber <= 7)
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
