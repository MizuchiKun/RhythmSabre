using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An enemy
    /// </summary>
    public class Screamer : Enemy
    {
        protected override Animation[] _walkingAnimations { get; } =
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Screamersheet_up"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Screamersheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150),
                          effects: SpriteEffects.FlipHorizontally),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Screamersheet_down"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150)),
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Screamersheet_left"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(150))
        };

        McTimer timer;

        public Screamer(Vector2? position = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)

        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Enemies/Screamersheet_down"),
               position,
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               rotation,
               effect)
        {
            Speed = 2;
            timer = new McTimer(1000, true);

            HitValue = 1;

            Health = 3;
        }

        public override void Update()
        {
            base.Update();

            timer.UpdateTimer();

            // if your attackcooldown is over, shoot towards the player
            if (timer.Test())
            {
                Globals.sounds.PlaySoundEffectOnce("EnemyAttack");
                Level.CurrentRoom.Add(new EnemyAttack(Globals.Vector2ToDegrees(Level.Player.Position - Position), Position, null, Globals.Vector2ToDegrees(Level.Player.Position - Position), SpriteEffects.None));
                timer.ResetToZero();
            }
        }

        public override void ChangePosition()
        {
            //if you are not in range of the player, move towards them.
            if (Globals.GetDistance(Position, Level.Player.Position) >= 201)
            {
                Speed = 2;
                Position += Globals.RadialMovement(Level.Player.Position, Position, Speed);
            }
            //if the player closes in, move away from them
            else if (Globals.GetDistance(Position, Level.Player.Position) < 199)
            {
                Position -= Globals.RadialMovement(Level.Player.Position, Position, Speed);
                if (HitWall())
                {
                    Speed = 0;
                }
            }
        }
    }
}
