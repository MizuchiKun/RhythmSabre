using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// Text in 2D space.
    /// </summary>
    public class Text : GameObject
    {
        /// <summary>
        /// The <see cref="SpriteFont"/> of this <see cref="Text"/>.
        /// </summary>
        public SpriteFont Font { get; set; }
        /// <summary>
        /// The message of this <see cref="Text"/>.
        /// </summary>
        public StringBuilder Message { get; set; }
        /// <summary>
        /// The position of this <see cref="Text"/>.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The relative origin of this <see cref="Text"/>.
        /// </summary>
        public Vector2 Origin { get; set; }
        /// <summary>
        /// The colour of this <see cref="Text"/>.
        /// </summary>
        public Color Colour { get; set; }
        /// <summary>
        /// The scale factors of this <see cref="Text"/>.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// The rotation of this <see cref="Text"/>.
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// The layer of this <see cref="Text"/>.
        /// </summary>
        public float Layer { get; set; }
        /// <summary>
        /// The sprite effects of this <see cref="Text"/>.
        /// </summary>
        public SpriteEffects Effects { get; set; }
        /// <summary>
        /// The hitbox of this <see cref="Text"/>.
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                Vector2 actualSize = Font.MeasureString(Message)
                                     * Scale;
                Vector2 absOrigin = Origin * actualSize;
                return new Rectangle(location: (Position - absOrigin).ToPoint(),
                                     size: actualSize.ToPoint());
            }
        }

        /// <summary>
        /// Creates a new Text with the given graphical parameters.
        /// </summary>
        /// <param name="font">Its font.</param>
        /// <param name="message">The message it will display.</param>
        /// <param name="position">Its position. <br></br>If null, it will be <see cref="Vector2.Zero"/>.</param>
        /// <param name="origin">
        /// Its relative origin. <br></br>
        /// It's relative to the <see cref="Sprite"/>'s dimensions.<br></br>
        /// E.g. (0.5, 0.5) corresponds to any <see cref="Sprite"/>'s centre.
        /// </param>
        /// <param name="colour">Its colour. <br></br>If null, it will be <see cref="Color.White"/>.</param>
        /// <param name="scale">Its scale factors. <br></br>If null, it will be <see cref="Vector2.One"/>.</param>
        /// <param name="rotation">Its rotation in degrees. <br></br>It's 0 by default.</param>
        /// <param name="layerDepth">Its layer depth. <br></br>It's 0 by default.</param>
        /// <param name="effects">Its sprite effects. <br></br>It's <see cref="SpriteEffects.None"/> by default.</param>
        public Text(SpriteFont font,
                    StringBuilder message,
                    Vector2? position = null,
                    Vector2? origin = null,
                    Color? colour = null,
                    Vector2? scale = null,
                    float rotation = 0f,
                    float layerDepth = 0f,
                    SpriteEffects effects = SpriteEffects.None)
        {
            // Store the parameters.
            Font = font;
            Message = message;
            Position = (position != null) ? position.Value : Vector2.Zero;
            Origin = (origin != null) ? origin.Value : Vector2.Zero;
            Colour = (colour != null) ? colour.Value : Color.White;
            Scale = (scale != null) ? scale.Value : Vector2.One;
            Rotation = rotation;
            Layer = layerDepth;
            Effects = effects;
        }

        /// <summary>
        /// A <see cref="Text"/>'s Update method.<br></br>
        /// Is empty if not overriden.
        /// </summary>
        public override void Update() { }

        /// <summary>
        /// Draws the <see cref="Text"/> with its current graphical parameters.
        /// </summary>
        public override void Draw()
        {
            // Draw the Sprite with its current graphical parameters.
            Globals.SpriteBatch.DrawString(
                spriteFont: Font,
                text: Message,
                position: Position,
                color: Color.Navy,
                rotation: MathHelper.ToRadians(Rotation),
                origin: Origin * Font.MeasureString(Message),
                scale: Scale * Globals.Scale,
                effects: Effects,
                layerDepth: Layer
            );
        }
    }
}
