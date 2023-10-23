using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasseBriques.Bonus;

namespace CasseBriques
{
    internal class BonusImpact : Bonus
    {
        public BonusImpact(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            addBonus = 2;
            currentState = BonusState.Idle;
            Speed = 0.05f;
            Vitesse = new Vector2(0, 1);
        }
        public override void Update()
        {
        //    if (currentState == BonusState.Free)
        //    {
        //        currentState = BonusState.Falling;
        //    }
        //    else if (currentState == BonusState.Falling)
        //    {
        //        Tombe();
        //    }

            base.Update();
        }
    }

}
