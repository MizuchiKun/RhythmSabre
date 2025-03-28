using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    
    /// <summary>
    /// An enemy
    /// </summary>
    public class Itemstone : Environment
    {
        Item Item;
        bool pickedUp = false;

        public Itemstone(Item item,
                         Vector2? position = null,
                         Rectangle? sourceRectangle = null,
                         float rotation = 0f,
                         SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Itemstone"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            // Set the item's position to this position.
            if (position != null)
            {
                item.Position = position.Value;
            }

            this.Item = item;
            Health = 10000;

            //Scale = new Vector2(.3f);
        }

        public override void Update()
        {
            base.Update();

            // if the player bumps into this object and the item is not picked up yet, give object, add it to the list for items and set the flag for being picked up.
            if (Level.Player.BumpsInto(this) && !pickedUp)
            {
                Item.Effect();
                Level.Player.items.Add(Item);
                Level.Player.itemcount += 1;
                pickedUp = true;
                Level.CurrentRoom.Remove(Item);
            }
        }

        public override void Draw()
        {
            // only draw the item, if its not picked up yet, draw the itemstone regardless.
            if (pickedUp == false)
            {
                Item.Draw();
            }
            base.Draw();
        }
    }
}
