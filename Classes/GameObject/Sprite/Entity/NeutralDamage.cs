using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektRoguelike
{
    /// <summary>
    /// Neutral damage, damaging the player, enemies and environmental objects.
    /// </summary>
    class NeutralDamage : Entity
    {
        public int OwnerID { get; } = 0;

        public int HitValue { get; set; }

        public NeutralDamage(Texture2D texture,
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
            
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
