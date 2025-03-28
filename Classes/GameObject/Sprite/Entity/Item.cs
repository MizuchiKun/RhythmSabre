using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An item
    /// </summary>
    public abstract class Item : Entity
    {
        public Item(Texture2D texture,
                     Vector2? position = null,
                     Rectangle? sourceRectangle = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)
        : base(texture,
               position,
               sourceRectangle,
               rotation,
               effect)
        {  }

        /// <summary>
        /// Will only be used for testing, items will always be on an itemstone.
        /// </summary>
        public override void Update()
        {
            base.Update();
            //if (Globals.GetDistance(Position, Level.Player.Position) < 84)
            if (Level.Player.BumpsInto(this))
            {
                Effect();
                Level.CurrentRoom.Remove(this);
            }
            
        }

        /// <summary>
        /// The effect that aquiring the item has on the player. 
        /// </summary>
        public virtual void Effect() { }
    }
}
