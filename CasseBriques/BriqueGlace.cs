using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class BriqueGlace : Briques
    {
        public BriqueGlace(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            scale = 1.0f;
            isBreakable = true;
            nbHits = 3;
            points = 300;
            id = ID.Glace;
        }

        public override void Update()
        { 
            base.Update();
        }

    }
}
