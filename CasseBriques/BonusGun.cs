using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CasseBriques
{
    public class BonusGun : Bonus
    {
        public BonusGun(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            currentState = BonusState.Idle;
            Speed = 0.07f;
            Vitesse = new Vector2(0, 1);
        }
        public override void Update()
        {
            //if (currentState == BonusState.Free)
            //{
            //    currentState = BonusState.Falling;
            //}
            //else if (currentState == BonusState.Falling)
            //{
            //    Tombe();
            //}
            base.Update();
        }
    }
}
