using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasseBriques.Personnages;

namespace CasseBriques
{
    public class PersonnageFire : Personnages
    { 
        public PersonnageFire(Texture2D pTexture) : base(pTexture)
        {
            texture = pTexture;
            spawnDelay = 0;
            spawnTimer = 5;
            CurrentState = State.Idle;
            isSpawn = false;
        }

        public override void SetPosition(float pX, float pY)
        {
            Position = new Vector2(pX, pY);
        }

        public override void Moving()
        {
          
            if (Position.Y >= 600)
            {
                float Up = 1.5f;
                Vitesse = new Vector2(-1, Vitesse.Y - Up);
                Position += Vitesse;
            }
            else if (Position.Y <= 500) 
            {
                float Down = 1.5f;
                Vitesse = new Vector2(-1, Vitesse.Y + Down);
                Position += Vitesse;
            }
        }

        public override void TimerON()
        {
            isSpawn = false;
            spawnDelay += 0.002f;
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
                    SetPosition(200, 600);
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
            base.Update();
        }
    }
}

