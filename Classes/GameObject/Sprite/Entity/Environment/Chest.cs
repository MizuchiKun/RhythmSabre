using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    
    /// <summary>
    /// An enemy
    /// </summary>
    public class Chest : Environment
    {
        public Chest(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Kiste"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            Health = 10000;
        }

        public override void Update()
        {
            // if the player has at least 1 key, open the chest by destroying it lul
            if (Level.Player.BumpsInto(this) && Level.Player.Keys > 0)
            {
                Level.Player.Keys -= 1;
                Health = 0;
            }
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
                // Drop Gold
            }
            else if (dropnumber > 4 && dropnumber <= 6)
            {
                for (int i = 0; i < 2; i++)
                {
                    Level.CurrentRoom.Add(new PickupHeart(new Vector2(Position.X - 20, Position.Y)));
                }
                // Drop PickupHeart
            }
            else if (dropnumber > 6 && dropnumber <= 8)
            {
                for (int i = 0; i < 1; i++)
                {
                    Level.CurrentRoom.Add(new PickupBomb(new Vector2(Position.X - 20, Position.Y)));
                }
                // Drop Bomb
            }
            else if (dropnumber > 8 && dropnumber <= 10)
            {
                for (int i = 0; i < 1; i++)
                {
                    Level.CurrentRoom.Add(new PickupKey(new Vector2(Position.X - 20, Position.Y)));
                }
                // Drop Key
            }
        }
    }
}
