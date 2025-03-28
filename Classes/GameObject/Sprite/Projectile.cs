using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    public class Projectile : Sprite
    {
        public int OwnerID { get; set; }
        protected int HitValue;
        public float Speed;

        protected McTimer timer;

        protected Vector2 Direction;



        public Projectile(Texture2D texture,
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
        {
            Speed = 10;

            Direction = Vector2.Zero;
            timer = new McTimer(1000);
        }

        public override void Update()
        {
            /*
            Update(Level.CurrentRoom.Enemies);
            base.Update();
            */
            timer.UpdateTimer();
            ChangePosition();
            if (timer.Test())
            {
                Level.CurrentRoom.Remove(this);
            }
            if (HitWall())
            {
                Level.CurrentRoom.Remove(this);
            }
            if (Collides(Level.Player) && (OwnerID == 2 || OwnerID == 0))
            {
                Level.Player.GetHit(HitValue);
                Level.CurrentRoom.Remove(this);
            }
            /*
            for (int i = 0; i < Level.CurrentRoom.Enemies.Count; i++)
            { 
                if (Collides(Level.CurrentRoom.Enemies) && (OwnerID == 1 || OwnerID == 0))
                {
                    Level.CurrentRoom.Enemies[i].GetHit(HitValue);
                    //Level.CurrentRoom.Enemies.ElementAt<Enemy>(i).GetHit(HitValue);
                    Level.CurrentRoom.Remove(this);
                }
            }
            */
            if (Collides(Level.CurrentRoom.Enemies) && (OwnerID == 1 || OwnerID == 0))
            {
                bool isColliding = false;//NEW
                for (int i = 0; i < Level.CurrentRoom.Enemies.Count; i++)
                {
                    if (Collides(Level.CurrentRoom.Enemies[i])//NEW
                        && OwnerID == 1)
                    {
                        //enemies[i].GetHit(HitValue);
                        Level.CurrentRoom.Enemies[i].GetHit(HitValue);
                        /*Level.CurrentRoom.Remove(this);*/
                        isColliding = true;
                    }
                }
                //NEW
                if (isColliding)
                {
                    Level.CurrentRoom.Remove(this);
                }
            }
            if (Collides(Level.CurrentRoom.Entities))
            {
                for (int i = 0; i < Level.CurrentRoom.Entities.Count ; i++)
                {
                    if (Collides(Level.CurrentRoom.Entities[i]) 
                        && (Level.CurrentRoom.Entities[i].GetType().Name == "Poop") 
                        || Level.CurrentRoom.Entities[i].GetType().Name == "Campfire")
                    {
                        Level.CurrentRoom.Entities[i].GetHit(HitValue);
                        Level.CurrentRoom.Remove(this);
                    }
                }
            }
        }
        /*
                private void Update(List<Enemy> enemies)
                {
                    timer.UpdateTimer();
                    ChangePosition();
                    if (timer.Test())
                    {
                        Level.CurrentRoom.Remove(this);
                    }
                    if (HitWall())
                    {
                        Level.CurrentRoom.Remove(this);
                    }
                    if (Collides(Level.Player) && (OwnerID == 2 || OwnerID == 0))
                    {
                        Level.Player.GetHit(HitValue);
                        Level.CurrentRoom.Remove(this);
                    }
                    if (Collides(enemies) && (OwnerID == 1 || OwnerID == 0))
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            if (OwnerID == 1)
                            {
                                Level.CurrentRoom.Enemies[i].GetHit(HitValue);
                                //Level.CurrentRoom.Enemies.ElementAt<Enemy>(i).GetHit(HitValue);
                                Level.CurrentRoom.Remove(this);
                            }
                        }
                    }
                }
        */

        /// <summary>
        /// Move the projectile.
        /// </summary>
        public virtual void ChangePosition()
        {
            Position += Speed * Direction * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Check for collision with the wall.
        /// </summary>
        /// <returns>Returns if a wall was hit. </returns>
        public virtual bool HitWall()
        {
            // up
            if (Collides(Level.CurrentRoom.Walls[0]))

            {
                return true;
            }
            // right
            else if (Collides(Level.CurrentRoom.Walls[1]))
            {
                return true;
            }
            // down
            else if (Collides(Level.CurrentRoom.Walls[2]))
            {
                return true;
            }
            // left
            else if (Collides(Level.CurrentRoom.Walls[3]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
