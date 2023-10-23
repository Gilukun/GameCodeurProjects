using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CasseBriques
{
    public class LevelManager
    {
        ScreenManager screen = ServiceLocator.GetService<ScreenManager>();
        HUD hud = ServiceLocator.GetService<HUD>();
        AssetsManager textures = ServiceLocator.GetService<AssetsManager>();
        Texture2D Briques;
        public int numero { get; set; } 
        public int[][] Map { get; set; } 
        public int LevelMax;
        
        LevelManager currentLevel;
        //Briques SprBriques;
        public Briques bNormal;
        PersonnageIce iceMan1;
        PersonnageFire fireMan1;
        PersonnageIce iceMan2;
        PersonnageFire fireman2;
       
        public List<Briques> listBriques { get; private set; }
        public List<Personnages> listPerso { get; private set; }
        public List<Personnages> listPerso2 { get; private set; }
        public List<Briques> listSolidBricks { get; private set; }

        public LevelManager() 
        {

        } 
        public LevelManager(int pNumero)
        {
            numero = pNumero;
            LevelMax = 4;
        }
    
         public void InitializeLevel(int pLevelMax)
        {
            LevelMax = pLevelMax;
            for (int i = 1; i <= LevelMax; i++)
            {
                LevelManager level = new LevelManager(i);
                level.RandomLevel();
                level.Save();
            }
        }
        public void RandomLevel()
        {
            int colNb = 10;
            int linNb = 4;
            Random rnd = new Random();
            Map = new int[linNb][]; 
            for (int l = 0; l < linNb; l++)
            {
                Map[l] = new int[colNb];
                for (int c = 0; c < colNb; c++)
                {
                    Map[l][c] = rnd.Next(1, 4);
                }
            }
        }
        public void Save()
        {
            string jsonLevel = JsonSerializer.Serialize(this); // on créer le fichier JSON
            File.WriteAllText("level" + numero + ".json", jsonLevel); // on l'exporte en fichier .Json
        }

        public void LoadLevel(int pLevel)
        {
            listBriques = new List<Briques>();
            listPerso = new List<Personnages>();
            listPerso2 = new List<Personnages>();
            listSolidBricks = new List<Briques>();
          
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            string levelData = File.ReadAllText("level" + pLevel + ".json");
            currentLevel = JsonSerializer.Deserialize<LevelManager>(levelData);

            Briques = textures.GetTexture("Bricks\\Brique_1");

            int NiveauHauteur = currentLevel.Map.GetLength(0);
            int NiveauLargeur = currentLevel.Map[1].Length;
            int largeurGrille = NiveauLargeur * Briques.Width;
            int hauteurGrille = NiveauHauteur * Briques.Height;
            int spacing = (screen.Width - largeurGrille) / 2;

            for (int l = 0; l < NiveauHauteur; l++)
            {
                for (int c = 0; c < NiveauLargeur; c++)
                {
                    int typeBriques = currentLevel.Map[l][c];

                    switch (typeBriques)
                    {
                        case 1:
                            Briques bNormal = new BriqueBase(_content.Load<Texture2D>("Bricks\\Brique_" + typeBriques));
                            bNormal.SetPosition(c * bNormal.SpriteWidth + spacing, l * bNormal.SpriteHeight + bNormal.HalfHeitgh + hud.SpriteHeight);
                            listBriques.Add(bNormal);
                            break;
                        case 2:
                            Briques bGlace = new BriqueGlace(_content.Load<Texture2D>("Bricks\\Brique_" + typeBriques));
                            bGlace.SetPosition(c * bGlace.SpriteWidth + spacing, l * bGlace.SpriteHeight + bGlace.HalfHeitgh + hud.SpriteHeight);
                            listBriques.Add(bGlace);
                            break;

                        case 3:
                            Briques bFeu = new BriqueFeu(_content.Load<Texture2D>("Bricks\\Brique_" + typeBriques));
                            bFeu.SetPosition(c * bFeu.SpriteWidth + spacing, l * bFeu.SpriteHeight + bFeu.HalfHeitgh + hud.SpriteHeight);
                            listBriques.Add(bFeu);
                            break;

                        default:
                            break;
                    }
                }
            }

            if (pLevel == 2)
            {
                iceMan2 = new PersonnageIce(_content.Load<Texture2D>("bIce"));
                listPerso.Add(iceMan2);
                fireman2 = new PersonnageFire(_content.Load<Texture2D>("bTime"));
                listPerso.Add(fireman2);

                int spacingX = 200;
                int firstBrickX = 200;
                int spacingGrille = 200;
                for (int i=1; i < 5; i++)
                { 
                    Briques bMetal= new BriqueMetal(_content.Load<Texture2D>("Brique_4"));
                    int brickX = firstBrickX + (i - 1) * spacingX;
                    int brickY = hauteurGrille + spacingGrille;
                    bMetal.SetPosition(brickX, brickY);
                    listSolidBricks.Add(bMetal);
                }
            }
            else
            {
                iceMan1 = new PersonnageIce(_content.Load<Texture2D>("pIce"));
                listPerso.Add(iceMan1);
                fireMan1 = new PersonnageFire(_content.Load<Texture2D>("pFire"));
                listPerso.Add(fireMan1);
            }  
        }
       
        public void Update()
        {

        }
        public void DrawLevel()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            float rotation;
            foreach (var Briques in listBriques)
            {
                if (Briques is BriqueFeu Fire)
                {
                    rotation = Fire.rotation;
                }
                else
                {
                    rotation = 0;

                }
                pBatch.Draw(Briques.texture,
                               Briques.Position,
                               null,
                               Color.White,
                               rotation,
                               new Vector2(Briques.HalfWidth, Briques.HalfHeitgh),
                               Briques.scale,
                               SpriteEffects.None,
                               0);
                //pBatch.DrawRectangle(Briques.BoundingBox, Color.Red);
            }

            foreach (var Perso in listPerso)
            {
                if (Perso.CurrentState != Personnages.State.Idle)
                {
                    pBatch.Draw(Perso.texture,
                                   Perso.Position,
                                   null,
                                   Color.White,
                                   0,
                                   new Vector2(Perso.HalfWidth, Perso.HalfHeitgh),
                                   1.0f,
                                   SpriteEffects.None,
                                   0);
                  //pBatch.DrawRectangle(Perso.BoundingBox, Color.Yellow);
                }
            }

            foreach (var Briques in listSolidBricks)
            {
           
                    pBatch.Draw(Briques.texture,
                                    Briques.Position,
                                    null,
                                    Color.White,
                                    0,
                                    new Vector2(Briques.HalfWidth, Briques.HalfHeitgh),
                                    1.0f,
                                    SpriteEffects.None,
                                    0);
                   //pBatch.DrawRectangle(Briques.BoundingBox, Color.Yellow);
            }
        }
    
    }
}

