using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// Represents an entity.<br></br>
    /// (Entities include the Player, Enemies, Chest and other interactable objects.)
    /// </summary>
    public class Entity : Sprite
    {
        public int Health { get; set; } = 1;

        public bool damageDealt = false;

        /// <summary>
        /// The hitbox of this <see cref="Entity"/>.
        /// </summary>
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
                return new Rectangle(location: ((Position - absOrigin) + new Vector2(0f, 0.5f) * actualSize).ToPoint(),
                                     size: (new Vector2(1f, 0.5f) * actualSize).ToPoint());
            }
        }

        public Vector2 _velocity { get; set; }

        /// <summary>
        /// Creates an Entity with the given graphical parameters.
        /// </summary>
        /// <param name="texture">Its texture.</param>
        /// <param name="position">Its position. <br></br>If null, it will be <see cref="Vector2.Zero"/>.</param>
        /// <param name="sourceRectangle">Its source rectangle. <br></br>If null, the whole texture will be drawn.</param>
        /// <param name="layerDepth">Its layer depth. <br></br>It's 0 by default.</param>
        /// <param name="effects">Its sprite effects. <br></br>It's <see cref="SpriteEffects.None"/> by default.</param>
        public Entity(Texture2D texture,
                      Vector2? position = null,
                      Rectangle? sourceRectangle = null,
                      float rotation = 0f,
                      SpriteEffects effects = SpriteEffects.None)
        : base(texture: texture,
               position: position,
               origin: new Vector2(0.5f),
               sourceRectangle: sourceRectangle,
               scale: Tile.Size / ((sourceRectangle != null) ? sourceRectangle.Value.Size.ToVector2() : texture.Bounds.Size.ToVector2()),
               rotation: rotation,
               effects: effects)
        { }

        /// <summary>
        /// An Entity's Update method.
        /// </summary>
        public override void Update()
        {

            if (Health <= 0)
            {
                // If this is an Enemy.
                if (this.GetType().IsSubclassOf(typeof(Enemy)))
                {
                    // If this is the last Enemy in the current room.
                    if (Level.CurrentRoom.Enemies.Count == 1)
                    {
                        // Open all doors.
                        foreach (Door door in Level.CurrentRoom.Doors)
                        {
                            // If there's a door in that direction.
                            if (door != null)
                            {
                                // If the door is not hidden and closed.
                                if (!(door.Kind == DoorKind.Hidden)
                                    && door.State == DoorState.Closed)
                                {
                                    door.Open();
                                }
                            }
                        }
                    }
                }

                Level.CurrentRoom.Remove(this);
            }

            // Update the Layer.
            Layer = 0.9f - (Position.Y / 10e6f);
        }

        /// <summary>
        /// Moves the Entity with the given velocity if possible.
        /// </summary>
        /// <param name="velocity">The given velocity.</param>
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

        /// <summary>
        /// Moves the Entity in the given direction with the given speed if possible.
        /// </summary>
        /// <param name="direciton">
        /// The direction in which the Entity shall move.
        /// </param>
        /// <param name="speed">The given speed.</param>
        private void Move(Directions direction, float speed)
        {
            // Get the Vector of the direction.
            Vector2 directionVector = Globals.DirectionVectors[(byte)direction];

            // Move this Sprite.
            Position += directionVector * speed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

            // If it collides with the top wall or another Entity.
            if (!CanMove(direction))
            {
                // It's not allowed to move up.
                // Revert its position.
                Position -= directionVector * speed * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Returns whether the Entity can move to the current position.
        /// </summary>
        /// <param name="direction">The direction in which it moved.</param>
        /// <returns></returns>
        protected virtual bool CanMove(Directions direction)
        {
            // If it collides with an Enemy but itself.
            List<Enemy> otherEnemies = Level.CurrentRoom.Enemies;
            otherEnemies.Remove((Enemy)this);
            if (Collides(otherEnemies))
            {
                // It can't move.
                return false;
            }
            // Else if it collides with the wall or player.
            else if (Collides(Level.CurrentRoom.Walls[(byte)direction])
                     || Collides(Level.Player))
            {
                // It can't move.
                return false;
            }

            // Default return.
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hitValue">The amount of damage the entity is supposed to receive. </param>
        public virtual void GetHit(int hitValue)
        {
            Health -= hitValue;
        }

        /// <summary>
        /// Gets whether the <see cref="Player"/> "bumps into" one of the given <see cref="GameObject"/>s.
        /// </summary>
        /// <param name="otherGameObjects">The other <see cref="GameObject"/>s.</param>
        /// <returns>True if it bumps into (at least) one of them, false otherwise.</returns>
        public bool BumpsInto(IEnumerable<GameObject> otherGameObjects)
        {
            foreach (GameObject gameObject in otherGameObjects)
            {
                if (BumpsInto(gameObject))
                {
                    // It bumped into one of the objects.
                    return true;
                }
            }

            // It didn't bump into anything.
            return false;
        }

        /// <summary>
        /// Gets whether the <see cref="Player"/> would "bumps into" the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="otherGameObject">The other <see cref="GameObject"/>.</param>
        /// <returns>True if it bumps into the given <see cref="GameObject"/>, false otherwise.</returns>
        public bool BumpsInto(GameObject otherGameObject)
        {
            // Move the Player (temporarily).
            Position += _velocity * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

            // Get whether it bumps into otherGameObject.
            bool bumpsInto = base.Collides(otherGameObject);

            // Restore the Player's previous position.
            Position -= _velocity * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;

            // Return whether it bumped into otherGameObject.
            return bumpsInto;
        }
    }
}
