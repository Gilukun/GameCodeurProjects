using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    internal class Win : ScenesManager
    {
        ContentManager _content = ServiceLocator.GetService<ContentManager>();
        AssetsManager assets = ServiceLocator.GetService<AssetsManager>();
        ScreenManager ResolutionEcran = ServiceLocator.GetService<ScreenManager>();

        Texture2D background;
        private string win;
        private string BackToMenu;

        Vector2 DimensionWin;
        Vector2 DimensionBackToMenu;

        private float fadeSpeed;
        private float currentAlpha;

        private float blinkSpeed;
        private bool textVisible;
        private float blinkTimer;

        public Win()
        {
            background = assets.GetTexture("Backgrounds\\Back_7");
            MediaPlayer.Play(assets.End);
        }

        public override void Load()
        {
            win = "YOU WIN";
            DimensionWin = assets.GetSize(win, assets.Victory);
            currentAlpha = 0;
            fadeSpeed = 0.002f;

            BackToMenu = "Appuyez sur M pour revenir au Menu";
            DimensionBackToMenu = assets.GetSize(BackToMenu, assets.ContextualFont);
            blinkSpeed = 0.05f;
            blinkTimer = 0;
            base.Load();
        }

        public void UpdateTxt()
        {
            if (currentAlpha < 1)
            {
                currentAlpha += fadeSpeed;
                if (currentAlpha >= 1)
                {
                    currentAlpha = 1;
                }
            }

            textVisible = true;
            blinkTimer += blinkSpeed;
            if (blinkTimer >= 2)
            {
                textVisible = false;
            }
            if (blinkTimer >= 4)
            {
                blinkTimer = 0;
            }
        }

        public override void Update()
        {
            UpdateTxt();
            base.Update();
        }
        public override void DrawScene()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            pBatch.Draw(background, new Vector2(0, 0), Color.White);

            Color textColor = new Color(Color.DarkRed, currentAlpha);
            pBatch.DrawString(assets.Victory,
                             win,
                             new Vector2(ResolutionEcran.HalfScreenWidth - DimensionWin.X / 2, ResolutionEcran.HalfScreenHeight - DimensionWin.Y / 2),
                             textColor);

            if (textVisible)
            {
                pBatch.DrawString(assets.ContextualFont,
                                 BackToMenu,
                                 new Vector2(ResolutionEcran.HalfScreenWidth - DimensionBackToMenu.X / 2, ResolutionEcran.HalfScreenHeight + DimensionWin.Y / 2),
                                 Color.DarkCyan);
            }
        }
    }
}
