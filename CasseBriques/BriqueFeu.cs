using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class BriqueFeu : Briques
    {
        public float rotation;
        public bool IsRotating
        {
            get
            {
                return rotate;
            }
        }

     public BriqueFeu(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            rotation = 0.0f;
            rotate = false;
            scale = 1.0f;   
            scalling = false;
            isBreakable = true;
            nbHits = 2;
            points = 200;
            id = ID.Feu;
        }

        public override void Update()
        {
            if (rotate)
            {
                rotation += 10f;
            }
            base.Update();
        }
    }
}
