using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    class Bomb : NeutralDamage
    {
        public override Rectangle Hitbox => Rectangle.Empty;

        McTimer timer;

        private const float _maxSpeed = 25;
        private const float _speedIncrease = 10;

        public Bomb(Vector2? position = null,
                    Rectangle? sourceRectangle = null,
                    float rotation = 0f,
                    SpriteEffects effects = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Pickups/Bomb"),
               position: position,
               sourceRectangle: sourceRectangle,
               rotation: rotation,
               effects: effects)
        {
            timer = new McTimer(1500);
        }

        public override void Update()
        {
            // after the time of 1.5 seconds
            timer.UpdateTimer();

            // spawn an explosion and remove this object
            if (timer.Test())
            {
                Level.CurrentRoom.Add(new Explosion(Position));
                Level.CurrentRoom.Remove(this);
            }

            base.Update();
        }
    }
}
