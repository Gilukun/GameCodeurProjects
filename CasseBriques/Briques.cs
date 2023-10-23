using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Briques : Sprites
    {
        public float scale { get; set; }
        public int nbHits { get; set; }
        public int points { get; set; }
        public int iD { get; set; }
        public bool scalling;
        public bool IsScalling
        {
            get
            {
                return scalling;
            }
        }
        public bool rotate { get; set; }
        public bool isBreakable;

        public enum State
        {
            Idle,
            Hit,
            Broken,
        }
        public State currentState { get; set; }

        public enum ID
        {
            Base,
            Feu,
            Glace,
        }

        public ID id { get; set; }
        public Briques(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
        }
        public override void Update()
        {
            if (scalling)
            {
                scale -= 0.05f;
                if (scale <= 0)
                {
                    scale = 0f;
                    scalling = false;
                }
            }
          base.Update();
        } 
    }
}
