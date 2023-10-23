using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Settings : ScenesManager
    {
        Texture2D background;
        private int id;
        List<Sprites> listIcons = new List<Sprites>();
        List<string> listTexts = new List<string>();
        List<Vector2> listPositionText = new List<Vector2>();
        PopUp text = new PopUp();
        public Settings()
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            background = _content.Load<Texture2D>("Backgrounds\\Back_8");
            int colNb = 10;
            int linNb = 10;
            int gridWidth = Screen.Width / colNb;
            int gridHeight = Screen.Height / linNb;
            int[][] gridNb = new int[linNb][];
            id = 0;
            for (int l = 0; l < linNb; l++)
            {
                gridNb[l] = new int[colNb];
                for (int c = 0; c < colNb; c++)
                {
                    gridNb[l][c] = id;
                    id++;
                }
            }

            for (int l = 0; l < linNb; l++)
            {
                for (int c = 0; c < colNb; c++)
                {
                    int typeIcons = gridNb[l][c];
                    switch (typeIcons)
                    {
                        case 11:
                            Bonus Health = new BonusVie(_content.Load<Texture2D>("Heart"));
                            Health.SetPosition(c * gridWidth, l *gridHeight);
                            listIcons.Add(Health);
                            string vie = "Ajoute une vie supplémentaire";
                            Vector2 viePosition = new Vector2(Health.Position.X + 100, Health.Position.Y);
                            listTexts.Add(vie);
                            listPositionText.Add(viePosition);
                            break;
                        case 21:
                            Bonus BigBall = new BonusImpact(_content.Load<Texture2D>("Balls\\bMenu"));
                            BigBall.SetPosition(c * gridWidth, l * gridHeight);
                            listIcons.Add(BigBall);
                            string bonusBall = "Détruisez les briques en un seul coup";
                            Vector2 bonusBallPosition = new Vector2(BigBall.Position.X + 100, BigBall.Position.Y);
                            listTexts.Add(bonusBall);
                            listPositionText.Add(bonusBallPosition);
                            break;
                        case 31:
                            Bullet Gun = new Bullet(_content.Load<Texture2D>("Gun"));
                            Gun.SetPosition(c * gridWidth, l * gridHeight);
                            listIcons.Add(Gun);
                            string arme = "Appuyez sur la touche Espace pour tirer sur les briques";
                            Vector2 armePosition = new Vector2(Gun.Position.X + 100, Gun.Position.Y);
                            listTexts.Add(arme);
                            listPositionText.Add(armePosition);
                            break;
                        case 41:
                            Personnages pFire = new PersonnageFire(_content.Load<Texture2D>("pFire"));
                            pFire.SetPosition(c * gridWidth, l * gridHeight);
                            listIcons.Add(pFire);
                            string feu = "Réduit la taille de l'écran et accélère la balle";
                            Vector2 feuPosition = new Vector2(pFire.Position.X + 100, pFire.Position.Y);
                            listTexts.Add(feu);
                            listPositionText.Add(feuPosition);

                            break;
                        case 51:
                            Personnages pIce = new PersonnageFire(_content.Load<Texture2D>("pIce"));
                            pIce.SetPosition(c * gridWidth, l * gridHeight);
                            listIcons.Add(pIce);
                            string glace = "Agrandit l'écran et ralentit la balle";
                            Vector2 glacePosition = new Vector2(pIce.Position.X + 100, pIce.Position.Y);
                            listTexts.Add(glace);
                            listPositionText.Add(glacePosition);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override void Load()
        {
        }
        public override void Update()
        {
            base.Update();
        }

        public override void DrawScene()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            AssetsManager assets = ServiceLocator.GetService<AssetsManager>();
            pBatch.Draw(background, new Vector2(0, 0), Color.White);

            //int colNb = 10;
            //int linNb = 10;
            //int gridWidth = Screen.Width / colNb;
            //int gridHeight = Screen.Height / linNb;
            //int[][] gridNb = new int[linNb][];
            //id = 0;
            //for (int l = 0; l < linNb; l++)
            //{
            //    gridNb[l] = new int[colNb];
            //    for (int c = 0; c < colNb; c++)
            //    {
            //        gridNb[l][c] = id;
            //        id++;
            //        pBatch.DrawString(assets.PopUpFont, gridNb[l][c].ToString(), new Vector2(c*gridWidth, l*gridHeight), Color.White) ;
            //    }
            //}
            foreach (var Sprites in listIcons)
            {
                pBatch.Draw(Sprites.texture, Sprites.Position, Color.White);
            }
            SpriteFont description; 
            description = assets.GetFont("SmallTextFont");
            for (int i = 0; i < listTexts.Count; i++) 
            {
                pBatch.DrawString(description, listTexts[i], listPositionText[i], Color.DarkOrchid);
            }

            string back2Menu = "Appuyz sur M pour revenir au Menu";
            pBatch.DrawString(description, back2Menu, new Vector2(Screen.Width / 2 , 800), Color.DarkSlateGray);
        }
    }
}
