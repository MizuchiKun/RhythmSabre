using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class PickupCoin : PickupItem
    {
        public PickupCoin(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Pickups/Coin"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        // is made to increase players amount of gold
        public override void Effect()
        {
            Level.Player.Gold += 1;
        }
    }
}
