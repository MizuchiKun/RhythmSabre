using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    class Explosion : Sprite
    {
        /// <summary>
        /// The blast radius of an explosion.
        /// </summary>
        private static readonly float BlastRadius = 1.25f;

        public int OwnerID { get; }

        public int HitValue { get; } = 2;

        McTimer timer;

        bool SoundHasPlayed;

        public Explosion(Vector2? position = null,
                         float rotation = 0f,
                         SpriteEffects effects = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Effects/Explosionsheet"),
               position: position,
               origin: new Vector2(0.5f),
               sourceRectangle: new Rectangle(0, 0, 256, 256),
               scale: Tile.Size / new Vector2(256),
               rotation: rotation,
               layerDepth: 1.0f,
               effects: effects)
        {
            // Set the animation.
            CurrentAnimation = new Animation(animationSheet: Globals.Content.Load<Texture2D>("Sprites/Effects/Explosionsheet"),
                                             frameDimensions: new Vector2(256),
                                             frameDuration: TimeSpan.FromMilliseconds(100),
                                             repetitions: 0);
            // Restart the animation.
            CurrentAnimation.Restart();

            // Set the layer.
            Layer = 0.9f - (Position.Y / 10e6f);

            OwnerID = 0;

            timer = new McTimer(300);

            SoundHasPlayed = false;
        }

        public override void Update()
        {
            if (!SoundHasPlayed)
            {
                Globals.sounds.PlaySoundEffect("Sound6");
                SoundHasPlayed = true;
            }
            

            // Unlocking / opening doors.
            // Top.
            if ((Level.CurrentRoom.Doors[(byte)Directions.Up] != null
                 && (Position - Level.CurrentRoom.Doors[(byte)Directions.Up].Position).Length() <= (BlastRadius * Tile.Size.X)))
            {
                // If the door is closed.
                if (Level.CurrentRoom.Doors[(byte)Directions.Up].State == DoorState.Closed)
                {
                    // Open the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Up].Open();
                }
                // Else if the door is hidden and locked.
                else if (Level.CurrentRoom.Doors[(byte)Directions.Up].Kind == DoorKind.Hidden
                    && Level.CurrentRoom.Doors[(byte)Directions.Up].State == DoorState.Locked)
                {
                    // Unlock the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Up].Unlock(true);
                }
            }
            // Right.
            if ((Level.CurrentRoom.Doors[(byte)Directions.Right] != null
                 && (Position - Level.CurrentRoom.Doors[(byte)Directions.Right].Position).Length() <= (BlastRadius * Tile.Size.X)))
            {
                // If the door is closed.
                if (Level.CurrentRoom.Doors[(byte)Directions.Right].State == DoorState.Closed)
                {
                    // Open the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Right].Open();
                }
                // Else if the door is hidden and locked.
                else if (Level.CurrentRoom.Doors[(byte)Directions.Right].Kind == DoorKind.Hidden
                    && Level.CurrentRoom.Doors[(byte)Directions.Right].State == DoorState.Locked)
                {
                    // Unlock the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Right].Unlock(true);
                }
            }
            // Bottom.
            if ((Level.CurrentRoom.Doors[(byte)Directions.Down] != null
                 && (Position - Level.CurrentRoom.Doors[(byte)Directions.Down].Position).Length() <= (BlastRadius * Tile.Size.X)))
            {
                // If the door is closed.
                if (Level.CurrentRoom.Doors[(byte)Directions.Down].State == DoorState.Closed)
                {
                    // Open the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Down].Open();
                }
                // Else if the door is hidden and locked.
                else if (Level.CurrentRoom.Doors[(byte)Directions.Down].Kind == DoorKind.Hidden
                    && Level.CurrentRoom.Doors[(byte)Directions.Down].State == DoorState.Locked)
                {
                    // Unlock the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Down].Unlock(true);
                }
            }
            // Left.
            if ((Level.CurrentRoom.Doors[(byte)Directions.Left] != null
                 && (Position - Level.CurrentRoom.Doors[(byte)Directions.Left].Position).Length() <= (BlastRadius * Tile.Size.X)))
            {
                // If the door is closed.
                if (Level.CurrentRoom.Doors[(byte)Directions.Left].State == DoorState.Closed)
                {
                    // Open the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Left].Open();
                }
                // Else if the door is hidden and locked.
                else if (Level.CurrentRoom.Doors[(byte)Directions.Left].Kind == DoorKind.Hidden
                    && Level.CurrentRoom.Doors[(byte)Directions.Left].State == DoorState.Locked)
                {
                    // Unlock the door.
                    Level.CurrentRoom.Doors[(byte)Directions.Left].Unlock(true);
                }
            }

            timer.UpdateTimer();


            // Deal Damage to any entity, that is not the player
            //if (Collides(Level.CurrentRoom.Entities) && (OwnerID == 1 || OwnerID == 0))
            //{
                //bool isColliding = false;//NEW
                for (int i = 0; i < Level.CurrentRoom.Entities.Count; i++)
                {
                    //if (Collides(Level.CurrentRoom.Entities[i])//NEW
                    if (Globals.GetDistance(this.Position, Level.CurrentRoom.Entities[i].Position) <= (BlastRadius * Tile.Size.X)
                        && OwnerID == 0
                        && (Level.CurrentRoom.Entities[i].GetType().IsSubclassOf(typeof(Environment))
                        || Level.CurrentRoom.Entities[i].GetType().IsSubclassOf(typeof(Enemy))
                        || Level.CurrentRoom.Entities[i].GetType().Name == "Campfire")
                        && Level.CurrentRoom.Entities[i].damageDealt == false)
                    {
                    //enemies[i].GetHit(HitValue);
                    Level.CurrentRoom.Entities[i].damageDealt = false;
                    Level.CurrentRoom.Entities[i].GetHit(HitValue);
                    Level.CurrentRoom.Entities[i].damageDealt = true;
                    /*Level.CurrentRoom.Remove(this);*/
                    //isColliding = true;
                }
                }
            //NEW
            //if (isColliding)
            //{
            //    Level.CurrentRoom.Remove(this);
            //}
            //}


            // Deal damage to the player
            //if (Collides(Level.Player))
            if (Globals.GetDistance(this.Position, Level.Player.Position) <= (BlastRadius * Tile.Size.X))
            {
                Level.Player.GetHit(HitValue);
            }
            if (timer.Test())
            {
                Level.CurrentRoom.Remove(this);
            }
        }
    }
}
