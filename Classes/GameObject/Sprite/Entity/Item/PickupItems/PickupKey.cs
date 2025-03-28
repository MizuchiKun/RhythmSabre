using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class PickupKey : PickupItem
    {
        public PickupKey(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Pickups/Key"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        // is made to increase players amount of keys
        public override void Effect()
        {
            Level.Player.Keys += 1;
        }
    }
}
