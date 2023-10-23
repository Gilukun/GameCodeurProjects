using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasseBriques.Personnages;

namespace CasseBriques
{
    public class Bonus : Sprites
    {
        public int addBonus;
        public int addlife;
        public enum BonusState
        {
            Idle,
            Free,
            Falling,
            Catch
        }
        public BonusState currentState { get; set; }

        public Bonus(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
        }
        public override void Load()
        {
            base.Load(); 
        }

        public virtual void SetPositionBonus(float pX, float pY)
        {
            Position = new Vector2(pX, pY);
        }
        public virtual void Tombe()
        {
            Vitesse = new Vector2(Vitesse.X, Vitesse.Y + Speed);
        }

        public override void Update()
        {
            if (currentState == BonusState.Free)
            {
                currentState = BonusState.Falling;
            }
            else if (currentState == BonusState.Falling)
            {
                Tombe();
            }
            base.Update();
        }

       
    }
}
