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
    public class PlayerAttack : Projectile
    {

        public PlayerAttack(Texture2D texture,
                            float angle,
                            Vector2? position = null,
                            Rectangle? sourceRectangle = null,
                            float rotation = 0f,
                            SpriteEffects effects = SpriteEffects.None)
        : base(texture: texture,
               position: position,
               sourceRectangle: sourceRectangle,
               rotation: rotation,
               effects: effects)
        {

            Speed = 250.0f;

            timer = new McTimer(1000);

            Direction = Globals.DegreesToVector2(angle);
            Direction.Normalize();

            HitValue = Level.Player.HitValue;

            OwnerID = 1;
        }
    }
}
