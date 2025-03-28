using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    class Campfire : NeutralDamage
    {
        public Campfire(Vector2? position = null,
                        float rotation = 0f,
                        SpriteEffects effects = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Firesheet"),
               position: position,
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               rotation: rotation,
               effects: effects)
        {
            Health = 3;
            HitValue = 1;

            // Set the animation.
            CurrentAnimation = new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Environment/Firesheet"),
                                             frameDimensions: new Vector2(256),
                                             frameDuration: TimeSpan.FromMilliseconds(100));
        }

        public override void Update()
        {
            if (Level.Player.BumpsInto(this))
            {
                Level.Player.GetHit(HitValue);
            }
            if (Health <= 0)
            {
                Level.CurrentRoom.Remove(this);
            }
        }
    }
}
