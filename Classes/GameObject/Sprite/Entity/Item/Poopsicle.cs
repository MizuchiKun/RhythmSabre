using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class Poopsicle: Item
    {
        public Poopsicle(Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Items/Poopice"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        // is made to create 3 flies orbiting the player
        public override void Effect()
        {
            Level.Player.poopsicle = true;
        }
    }
}
