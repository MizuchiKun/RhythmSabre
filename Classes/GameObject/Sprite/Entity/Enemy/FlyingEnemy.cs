using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An flying enemy
    /// </summary>
    public class FlyingEnemy : Enemy
    {
        protected override Animation[] _walkingAnimations { get; }
        /// <summary>
        /// The shadow all flying enemies have.
        /// </summary>
        private static readonly Texture2D _shadow = Globals.Content.Load<Texture2D>("Sprites/Enemies/Shadow");
        /// <summary>
        /// The shadow sprite of this flying enemy.
        /// </summary>
        protected Sprite _shadowSprite;

        public FlyingEnemy(Texture2D texture,
                           Vector2? position = null,
                           Rectangle? sourceRectangle = null,
                           float rotation = 0f,
                           SpriteEffects effect = SpriteEffects.None)
        : base(texture,
               position,
               sourceRectangle,
               rotation,
               effect)
        {
            // Initialize _shadowSprite.
            _shadowSprite = new Sprite(texture: _shadow,
                                       position: position,
                                       origin: new Vector2(0.5f, 1f),
                                       scale: Scale,
                                       rotation: rotation,
                                       layerDepth: 0.9999999f,
                                       effects: effect);
        }

        public override void Update()
        {
            base.Update();

            // Update your shadow's position.
            _shadowSprite.Position = Position + new Vector2(0f, 0.5f * ((Texture.Height * Scale.Y >= Tile.Size.Y) ? Texture.Height * Scale.Y : Tile.Size.Y));

            // Update the Layer.
            Layer = 0.6f - (Position.Y / 10e6f);
        }

        public override void Draw()
        {
            // Draw youself.
            base.Draw();

            // Draw your shadow.
            _shadowSprite.Draw();
        }
    }
}
