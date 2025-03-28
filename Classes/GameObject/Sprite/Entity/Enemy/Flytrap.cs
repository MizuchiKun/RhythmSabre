using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class Flytrap : Enemy
    {
        protected override Animation[] _walkingAnimations { get; } =
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flytrapsheet_up"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flytrapsheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150),
                          effects: SpriteEffects.FlipHorizontally),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flytrapsheet_down"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flytrapsheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150))
        };

        public Flytrap(Vector2? position = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)

        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flytrapsheet_down"),
               position,
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               rotation,
               effect)
        {
            Speed = 2f;
            Health = 3;

            HitValue = 1;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void AI()
        {
            base.AI();

            // if you touch the player 
            if (BumpsInto(Level.Player))
            {
                SpawnFlyAndDie();
            }
            // or if your HP is 0, spawn flies and disappear.
            if (Health <= 0)
            {
                SpawnFlyAndDie();
            }
        }

        private void SpawnFlyAndDie()
        {
            // Spawn 3 flies
            for (int i = 0; i < 3; i++)
            {
                Level.CurrentRoom.Add(new Fly(new Vector2(Position.X, Position.Y - 20 + (20 * i))));
            }
            // and disappear
            Level.CurrentRoom.Remove(this);
        }
    }
}
