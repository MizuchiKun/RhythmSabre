using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    
    /// <summary>
    /// An enemy
    /// </summary>
    public class Hole : Environment
    {
        public override Rectangle Hitbox
        {
            get
            {
                // Hitbox is bottom half of sprite.
                Vector2 actualSize = ((SourceRectangle != null)
                                     ? SourceRectangle.Value.Size.ToVector2()
                                     : Texture.Bounds.Size.ToVector2())
                                     * Scale * Globals.Scale;
                Vector2 absOrigin = Origin * actualSize;
                return new Rectangle(location: ((Position - absOrigin) + new Vector2(0f, 0f) * actualSize).ToPoint(),
                                     size: (new Vector2(1f, 1f) * actualSize).ToPoint());
            }
        }

        public Hole(Vector2? position = null,
                    Rectangle? sourceRectangle = null,
                    float rotation = 0f,
                    SpriteEffects effect = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Environment/Hole"),
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            Health = 10000;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            // Place it on the ground.
            Layer = 0.9999999f;

            base.Draw();
        }
    }
}
