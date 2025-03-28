using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    public class Flybuddy : PlayerAttack
    {
        /// <summary>
        /// The shadow all Flybuddies have.
        /// </summary>
        private static readonly Texture2D _shadow = Globals.Content.Load<Texture2D>("Sprites/Enemies/Shadow");
        /// <summary>
        /// The shadow sprite of this flybuddy.
        /// </summary>
        private Sprite _shadowSprite;

        public bool isDestroyed;
        public Vector2 Velocity;
        float angle, DistanceToPlayer;

        public Flybuddy(Vector2? position = null,
                        /*Rectangle? sourceRectangle = null,*/
                        float rotation = 0f,
                        SpriteEffects effects = SpriteEffects.None)
        : base(angle: 0f,
               texture: Globals.Content.Load<Texture2D>("Sprites/Enemies/Flysheet"),
               position: position,
               sourceRectangle: new Rectangle(0, 0, 256, 256)/*sourceRectangle*/,
               rotation: rotation,
               effects: effects)
        {
            // Calculate its initial angle towards the player.
            angle = Globals.Vector2ToDegrees(Position - Level.Player.Position);

            Speed = 250.0f;

            DistanceToPlayer = Globals.GetDistance(Position, Level.Player.Position);

            timer = new McTimer(1000);

            OwnerID = 1;

            HitValue = 1;

            isDestroyed = false;

            Scale = new Vector2(.1f, .1f);

            // Initialize _shadowSprite.
            _shadowSprite = new Sprite(texture: _shadow,
                                       position: position,
                                       origin: new Vector2(0.5f, 1f),
                                       scale: Scale,
                                       rotation: rotation,
                                       layerDepth: 0.9999999f,
                                       effects: effects);

            // Set the animation.
            CurrentAnimation = new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Enemies/FLysheet"),
                                             frameDimensions: new Vector2(256),
                                             frameDuration: TimeSpan.FromSeconds(1f / 60f));
        }

        public override void Update(/*List<Enemy> entities*/)
        {
            //if (Collides(Level.CurrentRoom.Enemies))
            //{
            //    isDestroyed = true;
            //    //damage touching enemy
            //}
            if (isDestroyed == false)
            {
                ChangePosition();

                if (Collides(Level.CurrentRoom.Enemies) && (OwnerID == 1 || OwnerID == 0))
                {
                    bool isColliding = false;//NEW
                    for (int i = 0; i < Level.CurrentRoom.Enemies.Count; i++)
                    {
                        if (Collides(Level.CurrentRoom.Enemies[i])//NEW
                            && OwnerID == 1)
                        {
                            Level.CurrentRoom.Enemies[i].GetHit(HitValue);
                            isColliding = true;
                        }
                    }
                    //NEW
                    if (isColliding)
                    {
                        isDestroyed = true;
                    }
                }
            }

            // Update your shadow's position.
            _shadowSprite.Position = Position + new Vector2(0f, 0.5f * ((Texture.Height * Scale.Y >= Tile.Size.Y) ? Texture.Height * Scale.Y : Tile.Size.Y));
        }

        public override void Draw()
        {
            // Draw yourself.
            base.Draw();

            // Draw your shadow.
            _shadowSprite.Draw();
        }

        public override void ChangePosition()
        {
            // Change the angle to the player.
            float rotationSpeed = 270f;
            angle += rotationSpeed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

            // Position it relative to the player's position.
            Position = Level.Player.Position + DistanceToPlayer * Globals.DegreesToVector2(angle);

            ////angle = Globals.Vector2ToDegrees(Level.Player.Position - Position);
            //Position = Globals.RotateAboutOrigin(Position, Level.Player.Position, .05f);

            //float SpeedFix;
            //if ((Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.A) && Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.S)) ||
            //    (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.A) && Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.W)) ||
            //    (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.D) && Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.S)) ||
            //    (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.D) && Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.W)))
            //{
            //    SpeedFix = 3f;
            //}
            //else
            //{
            //    SpeedFix = 4f;
            //}
            //if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.W))
            //{
            //    Position = new Vector2(Position.X, Position.Y - ((Level.Player.speed / Speed) * SpeedFix));
            //    Position.Normalize();
            //}
            //if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.S))
            //{
            //    Position = new Vector2(Position.X, Position.Y + ((Level.Player.speed / Speed) * SpeedFix));
            //    Position.Normalize();
            //}

            //if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.D))
            //{
            //    Position = new Vector2(Position.X + ((Level.Player.speed / Speed) * SpeedFix), Position.Y);
            //    Position.Normalize();
            //}
            //if (Globals.GetKey(Microsoft.Xna.Framework.Input.Keys.A))
            //{
            //    Position = new Vector2(Position.X - ((Level.Player.speed / Speed) * SpeedFix), Position.Y);
            //    Position.Normalize();
            //}
        }

        protected void Move(Vector2 velocity)
        {
            // Multiply the velocity by Globals.Scale.
            velocity *= Globals.Scale;

            // Move horizontally.
            // If it moves right.
            if (velocity.X > 0)
            {
                Move(Directions.Right, (float)Math.Abs(velocity.X));
            }
            // Else it moves left.
            else
            {
                Move(Directions.Left, (float)Math.Abs(velocity.X));
            }

            // Move vertically.
            // If it moves down.
            if (velocity.Y > 0)
            {
                Move(Directions.Down, (float)Math.Abs(velocity.Y));
            }
            // Else it moves up.
            else
            {
                Move(Directions.Up, (float)Math.Abs(velocity.Y));
            }
        }

        private void Move(Directions direction, float speed)
        {
            // Get the Vector of the direction.
            Vector2 directionVector = Globals.DirectionVectors[(byte)direction];

            // Move this Sprite.
            Position += directionVector * speed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        }
        }
    }
