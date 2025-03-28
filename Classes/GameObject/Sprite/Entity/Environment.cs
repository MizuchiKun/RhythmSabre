using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An environmental object
    /// </summary>
    public abstract class Environment : Entity
    {
        Random rand = new Random();
        protected int Dropnumber;

        public Environment(Texture2D texture,
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

        public override void Update()
        {
            base.Update();

            if (Health <= 0)
            {
                // Use Random rand to check what drops
                 Dropnumber = rand.Next(1, 10 +1);
                // Drops item X with X% chance
                Dropchance(Dropnumber);

                Globals.sounds.PlaySoundEffect("Destroyed");

                // And disappear once youre done.
                Level.CurrentRoom.Remove(this);
            }
        }

        /// <summary>
        /// Destroyable environmental items spawn pickupitems with a certain dropchance on destruction.
        /// </summary>
        /// <param name="dropnumber"> The dropchance with which certain items spawn. </param>
        protected virtual void Dropchance(int dropnumber) { }
    }
}
