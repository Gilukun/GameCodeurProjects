using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    internal class BriqueBase : Briques
    {
        public BriqueBase(Texture2D pTexture) : base(pTexture)
        {
            scale = 1.0f;
            scalling = false;
            isBreakable = true;
            nbHits = 1;
            points = 100;
            id = ID.Base;
        }
    }
}
