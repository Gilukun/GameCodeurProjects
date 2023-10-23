using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class PersonnageIce : Personnages
    {
        public PersonnageIce(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            spawnDelay = 0;
            spawnTimer = 20;
            CurrentState = State.Idle;
            isSpawn = false;
        }

        public override void Tombe()
        {
            Vitesse = new Vector2(Vitesse.X, 1);
        }

        public override void SetPosition(float pX, float pY)
        {
            Position = new Vector2(pX, pY);
        }

        public override void Moving()
        {
            Vitesse = new Vector2(1, Vitesse.Y);
            Position += Vitesse;
        }

        public override void TimerON()
        {
            spawnDelay += 0.01f;
            if (spawnDelay > spawnTimer)
            {
                TimerIsOver = true;
            }
        }

        public override void Update()
        {
            if (CurrentState == State.Idle)
            {
                if (!TimerIsOver)
                {
                    TimerON();
                }

                if (TimerIsOver && !isSpawn)
                {
                    Random rnd = new Random();
                    SetPosition(200, 400);
                    spawnDelay = 0;
                    isSpawn = true;
                    CurrentState = State.Spawn;
                    TimerIsOver = false;
                }
            }
            else if (CurrentState == State.Spawn)
            {
                CurrentState = State.Moving;
            }
            else if (CurrentState == State.Moving)
            {
                Moving();
            }
            else if (CurrentState == State.Falling)
            {
                Tombe();
            }
            else if (CurrentState == State.Catch)
            {
                CurrentState = State.Idle;
                isSpawn = false;
            }
            base.Update();
        }
    }
}
