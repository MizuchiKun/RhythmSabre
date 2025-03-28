using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// Represents a tile, the ground and walls of a room.
    /// </summary>
    public class Tile : Sprite
    {
        /// <summary>
        /// The size of a Tile.
        /// </summary>
        public static Vector2 Size
        {
            get
            {
                return Globals.BaseWindowDimensions / new Vector2(16, 9);
            }
        }

        /// <summary>
        /// The size of a tile texture.
        /// </summary>
        public static Vector2 TextureSize { get; } = new Vector2(256f);

        /// <summary>
        /// Creates a Tile with the given graphical parameters.
        /// </summary>
        /// <param name="texture">Its texture.</param>
        /// <param name="position">Its position. <br></br>If null, it will be <see cref="Vector2.Zero"/>.</param>
        /// <param name="sourceRectangle">Its source rectangle. <br></br>If null, the whole texture will be drawn.</param>
        /// <param name="layerDepth">Its layer depth. <br></br>It's 0 by default.</param>
        /// <param name="effect">Its sprite effect. <br></br>It's <see cref="SpriteEffects.None"/> by default.</param>
        public Tile(Texture2D texture,
                    Vector2? position = null,
                    Rectangle? sourceRectangle = null,
                    float rotation = 0f,
                    SpriteEffects effects = SpriteEffects.None)
        : base(texture: texture, 
               position: position, 
               origin: new Vector2(0.5f), 
               sourceRectangle: sourceRectangle, 
               rotation: rotation,
               layerDepth: 1.0f,
               effects: effects)
        { }

        /// <summary>
        /// Draws the Tile with its current graphical parameters <br></br>
        /// and a scale, relative to the window's dimensions.
        /// </summary>
        public override void Draw()
        {
            // Set the scale relative to the window's dimensions.
            Scale = Size / new Vector2(Texture.Width, Texture.Height);

            // Draw the Tile.
            base.Draw();
        }
    }
}
