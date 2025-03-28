using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class PickupBomb : PickupItem
    {
        public PickupBomb(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Pickups/Bomb"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        // is made to increase players amount of bombs
        public override void Effect()
        {
            Level.Player.Bombs += 1;
        }
    }
}
