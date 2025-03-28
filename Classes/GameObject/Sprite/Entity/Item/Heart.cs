using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class Heart : Item
    {
        public Heart(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Items/Heart"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        // is made to increase players max healthpoints
        public override void Effect()
        {
            Level.Player.HealthMax += 1;
            Level.Player.Health += 1;
        }
    }
}
