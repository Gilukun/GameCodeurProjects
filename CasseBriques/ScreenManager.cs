using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class ScreenManager
    {
        public GraphicsDeviceManager _graphics;

        public int Width
        {
            get
            {
                return _graphics.PreferredBackBufferWidth;
            }
            set
            { 
            }
        }
        public int Height
        {
            get
            {
                return _graphics.PreferredBackBufferHeight;
            }
        }

        public int HalfScreenWidth
        {
            get
            {
                return _graphics.PreferredBackBufferWidth / 2;
            }
        }
        public int HalfScreenHeight
        {
            get
            {
                return _graphics.PreferredBackBufferHeight / 2;
            }
        }


        public enum State
        {
            Basic, 
            Wide,
            Narrow,
        }

        public State currentState = new State();
        private float delay;
        private float timer;
        private bool TimerIsOver;
        public void TimerON(float pIncrement)
        {
            delay += pIncrement;
            if (delay >= timer)
            {
                TimerIsOver = true;
            }
        }
        public ScreenManager(GraphicsDeviceManager pGraphics)
        {
            _graphics = pGraphics;
            currentState = State.Basic;
            delay = 0;
            timer = 5;
        }
        public Point GetScreenSize()
        {
            return new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }
        public void ChangeResolution(int Width, int Height) // méthode pour changer la taille de l'écran.
        {
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();

        }

        //public void AddSize2()
        //{
        //    int fullSize = 1024;
        //    float ratio = 1.9f;
        //    if (Width< fullSize)
        //    {
        //        int newWidth = (int)(Width+ratio);
        //        ChangeResolution(newWidth, 900);
        //        GetScreenSize();
        //        Trace.WriteLine(newWidth);
        //    }
        //    else if (Width >= 1024)
        //    {
        //        Width = 1024;
        //    }

        //}
        
        public void AddSize()
        {
            ChangeResolution(1024, 900);
            GetScreenSize();
        }

        public void RemoveSize()
        {
            ChangeResolution(800, 900);
            GetScreenSize();
        }

        public void BasicSize()
        {
            ChangeResolution(900, 900);
            GetScreenSize();
        }

        public void Update()
        {
            TimerIsOver = false;
            if (currentState == State.Basic)
            {
                BasicSize();   
            }
            else if (currentState == State.Wide)
            {
                AddSize();
                if (!TimerIsOver)
                {   
                    TimerON(0.005f); 
                }

                if (delay>= timer)
                {
                    delay = 0;
                    currentState = State.Basic;  
                }
                
            }
            else if (currentState == State.Narrow)
            { 
                RemoveSize();
                if (!TimerIsOver)
                {
                    TimerON(0.005f);
                }
                if (delay >= timer)
                {
                    delay = 0;
                    currentState = State.Basic;
                }
                
            }
        }
        public void Draw()
        { 
        }
    }
}
