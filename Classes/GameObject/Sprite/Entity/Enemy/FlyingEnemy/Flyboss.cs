using System;
// using System.Runtime.Remoting.Messaging;  // Causes some issue when I'm trying to run it years later.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// An Bossenemy
    /// </summary>
    public class Flyboss : FlyingEnemy
    {
        protected override Animation[] _walkingAnimations { get; } =
        {
            new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flyboss_wo_shadow"),
                          frameDimensions: new Vector2(256),
                          frameDuration: TimeSpan.FromMilliseconds(1000))
        };

        McTimer AttackTimer, SpawnTimer, MovementTimer;

        const float _maxSpeed = 145;
        const float _speedIncrease = 5;

        Vector2 direction;

        bool bla = true;

        int itemNum, HealthMax;

        Random rand = new Random();

        public override Rectangle Hitbox
        {
            get
            {
                Vector2 actualSize = ((SourceRectangle != null)
                                     ? SourceRectangle.Value.Size.ToVector2()
                                     : Texture.Bounds.Size.ToVector2())
                                     * Scale * Globals.Scale;
                Vector2 absOrigin = Origin * actualSize;
                return new Rectangle(location: ((Position - absOrigin) + new Vector2(0.25f, 0.85f) * actualSize).ToPoint(),
                                     size: (new Vector2(0.5f, 0.15f) * actualSize).ToPoint());
            }
        }

        public Flyboss(Vector2? position = null,
                     float rotation = 0f,
                     SpriteEffects effect = SpriteEffects.None)

        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flyboss_wo_shadow"),
               position,
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               rotation,
               effect)
        {
            Speed = 2f;
            Health = 5;
            HealthMax = Health;

            Scale = new Vector2(1f);

            AttackTimer = new McTimer(1500);
            SpawnTimer = new McTimer(3000);

            direction = Globals.DegreesToVector2(45);

            HitValue = 0;

            // Set the _shadowSprite.
            _shadowSprite.Scale = Scale;

            itemNum = rand.Next(1, 4 + 1);
        }

        public override void Update()
        {
            if (Health <= 0)
            {
                //Level.CurrentRoom.Add(new Trapdoor(Position));
                switch (itemNum)
                {
                    case 1:
                        Level.CurrentRoom.Add(new Itemstone(new Heart(Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale),
                                                                        Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale));
                        break;
                    case 2:
                        Level.CurrentRoom.Add(new Itemstone(new Poopsicle(Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale),
                                                                        Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale));
                        break;
                    case 3:
                        Level.CurrentRoom.Add(new Itemstone(new Shroom(Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale),
                                                                        Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale));
                        break;
                    case 4:
                        Level.CurrentRoom.Add(new Itemstone(new Syringe(Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale),
                                                                        Level.CurrentRoom.Position + (Room.Dimensions / 2) * Tile.Size * Globals.Scale));
                        break;
                    default:
                        break;
                }
            }
            base.Update();
        }

        public override void AI()
        {
            base.AI();

            Attack();
        }

        public override void ChangePosition()
        {
            if (Collides(Level.CurrentRoom.Walls[0])||
                (Collides(Level.CurrentRoom.Walls[2])))
            {
                direction = new Vector2(direction.X, -direction.Y);
            }
            if (Collides(Level.CurrentRoom.Walls[1]) ||
                (Collides(Level.CurrentRoom.Walls[3])))
            {
                direction = new Vector2(-direction.X, direction.Y);
            }


            // DIRECTIONAL LOOP
            //MovementTimer.UpdateTimer();
            //if (MovementTimer.Test())
            //{
            //    // used to easily change the direction, in which the unit moves
            //    bla = !bla;
            //    switch (bla)
            //    {
            //        case true:
            //            direction = Globals.DegreesToVector2(0);
            //            break;
            //        case false:
            //            direction = Globals.DegreesToVector2(180);
            //            break;
            //    }
            //    // to restart the directional loop
            //    MovementTimer.ResetToZero();
            //}

            // SPEED LOOP
            // speed up if youre not at maxspeed yet
            if (Speed < _maxSpeed)
            {
                Speed += _speedIncrease;
            }
            // slow down if you are at or higher than the speedlimit
            if (Speed >= _maxSpeed)
            {
                // if you have not stopped yet, slow down
                if (Speed > 0)
                {
                    Speed -= _speedIncrease;
                }
            }

            // move the unit
            Position += direction * Speed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Attack()
        {
            // FLYSPAWN LOOP
            SpawnTimer.UpdateTimer();
            // Every X seconds, spawn some enemies.
            if (SpawnTimer.Test())
            {
                // Spawn 3 flies
                for (int i = 0; i < 3; i++)
                {
                    Level.CurrentRoom.Add(new Fly(new Vector2(Position.X - 25 + (25 * i), Position.Y)));
                }
                // Restart the thing.
                SpawnTimer.ResetToZero();
            }

            // ATTACK LOOP
            AttackTimer.UpdateTimer();
            // Every X seconds, attack the player
            if (AttackTimer.Test())
            {
                // Spawn 3 projectiles
                for (int i = 0; i < 3; i++)
                {
                    Globals.sounds.PlaySoundEffectOnce("EnemyAttack");
                    Level.CurrentRoom.Add(new EnemyAttack(Globals.Vector2ToDegrees(Level.Player.Position - Position) - 25 + (i * 25), Position));
                }
                // Restart the thing.
                AttackTimer.ResetToZero();
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < HealthMax; i++)
            {
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("Sprites/Items/Heart"),
                    new Rectangle((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / HealthMax * i + (Globals.Graphics.PreferredBackBufferWidth / 4)),
                                  (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 8), 40, 40),
                                   null, Color.Black, 0, new Vector2(.5f), new SpriteEffects(), 0.11f);
            }

            for (int i = 0; i < Health; i++)
            {
                Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>("Sprites/Items/Heart"),
                    new Rectangle((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / HealthMax * i + (Globals.Graphics.PreferredBackBufferWidth / 4)),
                                  (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 8), 40, 40),
                                   null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.1f);
            }

            Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "The Duke of Flies",
                                          new Vector2(Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 155,
                                          (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 8) - 25), Color.White);

            base.Draw();
        }
    }
}
