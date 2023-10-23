using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class GUI : Sprites
    {
        public delegate void OnClick(GUI pSender);
        public bool IsHover { get; private set; }
        protected MouseState oldMState;
        protected MouseState newMState;
        protected Point mousePosition;
        public OnClick onClick { get; set; }
        public GUI(Texture2D pTexture) : base(pTexture)
        {
        }
        public bool IsHovering
        {
            get
            {
                return IsHover;
            }
        }

        public override void Update()
        {
            newMState = Mouse.GetState();
            mousePosition = Mouse.GetState().Position;

            if (BoundingBox.Contains(mousePosition))
            {
                if (!IsHover)
                {
                    IsHover = true; 
                }
            }
            else
            {
                if (IsHover)
                {
                    IsHover = false;
                }
            }
            if (IsHover)
            {
                if (newMState.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
                {
                    if (onClick != null)
                    {
                        onClick(this);
                    }
                }
            }
            oldMState = newMState;
            base.Update();
        }
    }
}