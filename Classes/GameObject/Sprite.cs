using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// A Sprite in 2D space.
    /// </summary>
    public class Sprite : GameObject
    {
        /// <summary>
        /// This <see cref="Sprite"/>'s texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s current animation.
        /// </summary>
        public Animation CurrentAnimation { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s relative origin.
        /// </summary>
        public Vector2 Origin { get; }

        /// <summary>
        /// This <see cref="Sprite"/>'s source rectangle.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s scale factors.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s rotation (in degrees).
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s layer depth.
        /// </summary>
        public float Layer { get; set; }

        /// <summary>
        /// This <see cref="Sprite"/>'s colour.
        /// </summary>
        public Color Colour { get; }

        /// <summary>
        /// This <see cref="Sprite"/>'s sprite effects.
        /// </summary>
        public SpriteEffects Effects { get; set; }

        /// <summary>
        /// The hitbox of this <see cref="Sprite"/>.
        /// </summary> 

        public override Rectangle Hitbox
        {
            get
            {
                Vector2 actualSize = ((SourceRectangle != null)
                                     ? SourceRectangle.Value.Size.ToVector2()
                                     : Texture.Bounds.Size.ToVector2())
                                     * Scale * Globals.Scale;
                Vector2 absOrigin = Origin * actualSize;
                return new Rectangle(location: (Position - absOrigin).ToPoint(),
                                     size: actualSize.ToPoint());
            }
        }

        /// <summary>
        /// Creates a new Sprite with the given graphical parameters.
        /// </summary>
        /// <param name="texture">Its texture. Is optional if an animation is given.</param>
        /// <param name="animation">Its optional animation. If null, it won't be animated.</param>
        /// <param name="position">Its position. <br></br>If null, it will be <see cref="Vector2.Zero"/>.</param>
        /// <param name="origin">
        /// Its relative origin. <br></br>
        /// It's relative to the <see cref="Sprite"/>'s dimensions.<br></br>
        /// E.g. (0.5, 0.5) corresponds to any <see cref="Sprite"/>'s centre.
        /// </param>
        /// <param name="sourceRectangle">Its source rectangle. <br></br>If null, the whole texture will be drawn.</param>
        /// <param name="scale">Its scale factors. <br></br>If null, it will be <see cref="Vector2.One"/>.</param>
        /// <param name="rotation">Its rotation in degrees. <br></br>It's 0 by default.</param>
        /// <param name="layerDepth">Its layer depth. <br></br>It's 0 by default.</param>
        /// <param name="colour">Its colour. <br></br>If null, it will be <see cref="Color.White"/>.</param>
        /// <param name="effects">Its sprite effects. <br></br>It's <see cref="SpriteEffects.None"/> by default.</param>
        public Sprite(Texture2D texture = null,
                      Animation animation = null,
                      Vector2? position = null,
                      Vector2? origin = null,
                      Rectangle? sourceRectangle = null,
                      Vector2? scale = null,
                      float rotation = 0f,
                      float layerDepth = 0f,
                      Color? colour = null,
                      SpriteEffects effects = SpriteEffects.None)
        {
            // Store the parameters.
            Texture = (texture != null) ? texture : animation.Sheet;
            CurrentAnimation = animation;
            Position = (position != null) ? position.Value : Vector2.Zero;
            Origin = (origin != null) ? origin.Value : new Vector2(0.5f);
            SourceRectangle = (animation != null) ? animation.CurrentFrameRect : (sourceRectangle != null) ? sourceRectangle : texture.Bounds;
            Scale = (scale != null) ? scale.Value : Vector2.One;
            Rotation = rotation;
            Layer = layerDepth;
            Colour = (colour != null) ? colour.Value : Color.White;
            Effects = effects;
        }

        /// <summary>
        /// A <see cref="Sprite"/>'s Update method.<br></br>
        /// Is empty if not overriden.
        /// </summary>
        public override void Update() { }

        /// <summary>
        /// Draws the <see cref="Sprite"/> with its current graphical parameters.
        /// </summary>
        public override void Draw()
        {
            // Apply the animation if it's not null.
            if (CurrentAnimation != null)
            {
                Texture = CurrentAnimation.Sheet;
                SourceRectangle = CurrentAnimation.CurrentFrameRect;
                Effects = CurrentAnimation.Effects;
            }

            // Draw the Sprite with its current graphical parameters.
            Globals.SpriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: SourceRectangle,
                color: Colour,
                rotation: MathHelper.ToRadians(Rotation),
                origin: Origin * ((SourceRectangle != null) ? SourceRectangle.Value.Size.ToVector2() : Texture.Bounds.Size.ToVector2()),
                scale: Scale * Globals.Scale,
                effects: Effects,
                layerDepth: Layer);
        }
    }
}
