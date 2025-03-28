using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class Fly : FlyingEnemy
    {
        protected override Animation[] _walkingAnimations { get; } =
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flysheet"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(1f / 60f))
        };

        public Fly(Vector2? position = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)

        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flysheet"),
               position,
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               rotation,
               effect)
        {
            Speed = 2f;
            Health = 1;

            HitValue = 1;

            Scale = new Vector2(.1f, .1f);

            // Set the _shadowSprite.
            _shadowSprite.Scale = Scale;

            // Set the animation.
            CurrentAnimation = _walkingAnimations[0];
        }

        public override void Update()
        {
            base.Update();
        }

        public override void ChangePosition()
        {
            _velocity = (Level.Player.Position - Position) / 4;
            Move(_velocity);
        }

        public override void AI()
        {
            ChangePosition();

            // if you hit the player in the room, deal damage and disappear
            if (BumpsInto(Level.Player))
            {
                Level.Player.GetHit(1);
                Level.CurrentRoom.Remove(this);
            }
        }
    }
}
