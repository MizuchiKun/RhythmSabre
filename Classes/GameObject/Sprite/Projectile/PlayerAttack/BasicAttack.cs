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
    public class BasicAttack : PlayerAttack
    {

        public BasicAttack(float angle,
                      Vector2? position = null,
                      Rectangle? sourceRectangle = null,
                      float rotation = 0f,
                      SpriteEffects effects = SpriteEffects.None)
        : base(texture: Globals.Content.Load<Texture2D>("Sprites/Misc/Playerattack"),
               position: position,
               angle : angle,
               sourceRectangle: sourceRectangle,
               rotation: rotation,
               effects: effects)
        {

            Speed = 250.0f;

            Scale = new Vector2(0.1f);

            timer = new McTimer(1000);

            HitValue = Level.Player.HitValue;
        }
    }
}
