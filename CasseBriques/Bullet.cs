using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CasseBriques
{
    public class Bullet : Sprites
    {
        ContentManager _content = ServiceLocator.GetService<ContentManager>();
        HUD hud = ServiceLocator.GetService<HUD>(); 
        public List<Bullet> ListeBalles;

        public int impact;
        public bool collision;
        private float delay = 0;
        private int timer = 5;
        private bool TimerIsOver;
        public enum State
        {
            NoTActivated,
            Activated,
        }
        public State BulletState { get; set; }
        public State BState
        {
            get
            { return BulletState; }
        }

        public void TimerON(float pIncrement)
        {
            delay += pIncrement;
            if (delay >= timer)
            {
                TimerIsOver = true;
            }
        }
        public Bullet(Texture2D ptexture) : base(ptexture)
        {
            texture = ptexture;
            impact = 1;
            ListeBalles = new List<Bullet>();
            BulletState = State.NoTActivated;
            Speed = 1;
        } 

        public void CreateBullet(string pNom, float pX, float pY, float pSpeed)
        {
            Bullet bullet = new Bullet(_content.Load<Texture2D>("Balls\\bFire"));
            bullet.SetPosition(pX,pY);
            bullet.Vitesse = new Vector2(0, -1);
            bullet.Speed = pSpeed;
            ListeBalles.Add(bullet);
        }

        public void BulletMoves()
        {
            Position += Vitesse * Speed;
        }

        public override void Update()
        {
            BulletMoves();
            if (BulletState == State.Activated) 
            {
                hud.currentState = HUD.State.hasGun;
                if (!TimerIsOver)
                {
                    TimerON(0.01f);
                }
                if (delay >= timer)
                {
                    delay = 0;
                    BulletState = State.NoTActivated;
                    hud.currentState = HUD.State.noGun;
                }
            }
            TimerIsOver = false;
            base.Update();
        }

        public void DrawWeapon()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();  
            foreach (var item in ListeBalles)
            {
                pBatch.Draw(item.texture, item.Position, Color.White);
                //pBatch.DrawRectangle(item.BoundingBox, Color.Red);
            }
        }
    }
}
