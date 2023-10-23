using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Menu : ScenesManager
    {
        ScreenManager screen = ServiceLocator.GetService<ScreenManager>();
        GameState status = ServiceLocator.GetService<GameState>();
        ContentManager _content = ServiceLocator.GetService<ContentManager>();
        AssetsManager assets = ServiceLocator.GetService<AssetsManager>();

        private GUI boutonEnter;
        private GUI boutonSettings;
        private List<GUI> listeBoutons;

        Texture2D background;

        private string titre;
        private Vector2 dimensionTitre;
        private string start;
        private string settings;
        private Vector2 dimensionStart;
        private Vector2 dimensionSettings;
        private int spacing;

        Balle mouseIcon;

        private float delay;
        private float timer;
        private bool timerIsOn;
        public Menu()
        { 
            background = _content.Load<Texture2D>("BckMenu");
            mouseIcon = new Balle (_content.Load<Texture2D>("bMenu"));
            timer = 2;
            delay = 0;
            assets.PlaySong(assets.Intro);
        }

        public void TimerON(float pIncrement)
        {
            delay += pIncrement;
        }

        public void OnClick(GUI pSender)
        {
            if (pSender == boutonEnter)
            {
                assets.PlaySFX(assets.Select);
                timerIsOn = true;  
            }
            else if (pSender == boutonSettings)
            {
                assets.PlaySFX(assets.Select);
                status.ChangeScene(GameState.Scenes.Setting);
                assets.StopSong();
            }

        }

        public override void Load()
        {
            
            listeBoutons = new List<GUI>();
            boutonEnter = new GUI(_content.Load<Texture2D>("Bouton_2"));
            boutonEnter.SetPosition(screen.HalfScreenWidth, screen.HalfScreenHeight);

            int LargeurBouton = boutonEnter.SpriteWidth;
            int HauteurBouton = boutonEnter.SpriteHeight;
            spacing = HauteurBouton / 2;

            boutonEnter.onClick = OnClick;
            listeBoutons.Add(boutonEnter);

            boutonSettings = new GUI(_content.Load<Texture2D>("Bouton_2"));
            boutonSettings.SetPosition(screen.HalfScreenWidth, boutonEnter.Position.Y + HauteurBouton + spacing);
            boutonSettings.onClick = OnClick;
            listeBoutons.Add(boutonSettings);

            titre = "FANTASOID";
            dimensionTitre = assets.GetSize(titre, assets.TitleFont);

            start = "START";
            dimensionStart = assets.GetSize(start, assets.HUDFont);

            settings = "SETTINGS";
            dimensionSettings = assets.GetSize(settings, assets.HUDFont);
        }

        public override void Update()
        {
            mouseIcon.SetPosition((Mouse.GetState().X - mouseIcon.HalfHeitgh), (Mouse.GetState().Y - mouseIcon.HalfHeitgh));
            if (timerIsOn)
            {
                TimerON(0.03f);
            }
            if (delay > timer)
            {
                status.ChangeScene(GameState.Scenes.Gameplay);
                delay = 0;
                timerIsOn = false;
            }

            boutonEnter.Update();
            boutonSettings.Update();
            base.Update();
        }

        public override void DrawScene()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            pBatch.Draw(background, new Vector2(0, 0), Color.White);

            // Affichage du nom du jeu 
            pBatch.DrawString(assets.TitleFont,
                              "FANTASOID",
                              new Vector2(screen.HalfScreenWidth - dimensionTitre.X / 2, 100 - dimensionTitre.Y / 2),
                              Color.DarkSlateBlue);

            // Affichage des boutons
            boutonEnter.Draw();
            boutonSettings.Draw();

            // Affichage des textes au dessus des boutons
            Color color;
            foreach (GUI item in listeBoutons)
            {
                if (item.IsHover)
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.DarkMagenta;
                }
                if (item == boutonEnter)
                {
                    pBatch.DrawString(assets.HUDFont,
                                             "START",
                                             new Vector2(boutonEnter.Position.X - dimensionStart.X / 2, boutonEnter.Position.Y - dimensionStart.Y / 2),
                                             color);
                }
                else if (item == boutonSettings)
                {
                    pBatch.DrawString(assets.HUDFont,
                                     "SETTINGS",
                                     new Vector2(boutonSettings.Position.X - dimensionSettings.X / 2, boutonSettings.Position.Y - dimensionSettings.Y / 2),
                                     color);
                }
            }

            // Icone de la souris
            pBatch.Draw(mouseIcon.texture, new Vector2(mouseIcon.Position.X, mouseIcon.Position.Y), Color.White);
        }
    }
}