using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    public class EnemyAttack : Projectile
    {
        public EnemyAttack(float angle,
                      Vector2? position = null,
                      Rectangle? sourceRectangle = null,
                      float rotation = 0f,
                      SpriteEffects effects = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Misc/enemyattack"),
               position: position,
               sourceRectangle: sourceRectangle,
               rotation: rotation,
               effects: effects)
        {

            Speed = 250.0f;

            Direction = Globals.DegreesToVector2(angle);
            Direction.Normalize();

            HitValue = 1;

            Scale = new Vector2(0.1f);

            timer = new McTimer(1000);

            OwnerID = 2;
        }

        public override void ChangePosition()
        {
            Position += Speed * Direction * (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
