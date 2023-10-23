using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CasseBriques
{
    public class Gameplay : ScenesManager
    {
        ScreenManager screen = ServiceLocator.GetService<ScreenManager>();
        ContentManager _content = ServiceLocator.GetService<ContentManager>();
        GameState status = ServiceLocator.GetService<GameState>();
        AssetsManager assets = ServiceLocator.GetService<AssetsManager>();

        Texture2D hudBarre; 
        HUD hud = ServiceLocator.GetService<HUD>();
        Bullet bullet = ServiceLocator.GetService<Bullet>();

        Raquette pad;
        Balle ball;
        PopUp pop;
        Bonus life;
        Bonus bigBall;
        Bonus gun;

        LevelManager level = new LevelManager();

        List<Bonus> listeBonus = new List<Bonus>();
        List<PopUp> listePopUp = new List<PopUp>();
        private Texture2D background;
        public bool stick;
        public bool isKeyboardPressed;
        KeyboardState oldKbState;
        KeyboardState newKbState;

        private int currentBackground;
        private Random rnd;

        public void LoadBackground()
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            background = assets.GetTexture("Backgrounds\\Back_" + currentBackground);
        }

        public Gameplay()
        {
            currentBackground = 1;
            LoadBackground();

            //Soundtracks
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            assets.PlaySong(assets.InGame);
            
            // RAQUETTE
            pad = new Raquette(_content.Load<Texture2D>("Pad_2"));
            pad.SetPosition(screen.HalfScreenWidth, screen.Height - pad.HalfHeitgh);
           
            // BALLE
            ball = new Balle(_content.Load<Texture2D>("Balls\\bNormal"));
            ball.SetPosition(pad.Position.X, pad.Position.Y - pad.HalfHeitgh - ball.HalfHeitgh);
            
            stick = true;

            // Texture de la barre HUD
            hudBarre = assets.GetTexture("HUD2");
            pop = new PopUp();

            // Etats du clavier
            oldKbState = Keyboard.GetState();

            // Chargement du premier niveau lors de de la création de Gameplay
            hud.level = 1;
            level.InitializeLevel(4);
            level.LoadLevel(hud.level);

            rnd = new Random(); 

        }

        public override void Update()
        {
            newKbState = Keyboard.GetState();
            if (newKbState.IsKeyDown(Keys.Space) && !oldKbState.IsKeyDown(Keys.Space))
            {
                stick = false;
            }
            // Création et tirs des balles
            if (bullet.BulletState == Bullet.State.Activated)
            {
                if (newKbState.IsKeyDown(Keys.P) && !oldKbState.IsKeyDown(Keys.P))
                {
                    bullet.CreateBullet("Rifle", pad.Position.X, pad.Position.Y,9);
                    assets.PlaySFX(assets.shoot);
                }
            }
            oldKbState = newKbState;
          
            pad.Update();

            // COLLISIONS BALLE ET BRIQUES
            CollisionBricks();
          
           
            if (!ball.collision)
            {
                ball.Update();
            }

            // COLLISIONS BALLE ET PERSONNAGES
            for (int p = level.listPerso.Count - 1; p >= 0; p--)
            {
                ball.collision = false;
                Personnages mesPerso = level.listPerso[p];
                mesPerso.Update();
                if (mesPerso.BoundingBox.Intersects(ball.NextPositionY()))
                {
                    ball.collision = true;
                    assets.PlaySFX(assets.hitBricks);
                    ball.InverseVitesseY();
                }
                else if (mesPerso.BoundingBox.Intersects(ball.NextPositionX()))
                {
                    ball.collision = true;
                    assets.PlaySFX(assets.hitBricks);
                    ball.InverseVitesseX();
                }

                if (ball.collision)
                {
                    mesPerso.CurrentState = Personnages.State.Falling;
                    mesPerso.Tombe();
                }

                if (mesPerso.Position.Y > screen.Height)
                {
                    level.listPerso.Remove(mesPerso);
                }
            }
            
            // RESET DE LA BALLE SUR LA RAQUETTE
            if (stick)
            {
                ball.SetPosition(pad.Position.X, pad.Position.Y - pad.HalfHeitgh - ball.HalfHeitgh);
            }
            // COLLISIONS BULLETS ET BRIQUES 
            CollisionBulletsBricks();

            // Collisions aves les briques metals
            CollisionSolidBricks();

            // COLLISIONS AVEC LA RAQUETTE
            ColPadPerso();
            CollectBonus();
            PadRebound();

            // BALLE QUI SORT DE L'ECRAN
            LoseLife();

            // CHARGEMENT DU NIVEAU SUIVANT
            NextLevel();

            // Updates de l'écran et des bullets
            screen.Update();
            bullet.Update(); 
            base.Update();
        }

        public void PadRebound()
        {
            if (pad.BoundingBox.Intersects(ball.BoundingBox))
            {
                assets.PlaySFX(assets.PadRebound);
                ball.InverseVitesseY();
                ball.SetPosition(ball.Position.X, pad.Position.Y - ball.SpriteHeight);
            }
        }

        public void NextLevel()
        {
            if (!level.listBriques.Any(brique => brique.isBreakable)) // comme count mais avec de meilleur performance/ Proposé par VisualStudio
            {
                level.listPerso.Clear();
                listeBonus.Clear();
                bullet.ListeBalles.Clear();
                currentBackground++;
                hud.level++;

                if (hud.level > level.LevelMax)
                {
                    status.ChangeScene(GameState.Scenes.Win);
                    screen.currentState = ScreenManager.State.Basic;
                    hud.level = 1;
                    currentBackground = 1;
                }
                else
                {
                    stick = true;
                    LoadBackground();
                    level.LoadLevel(hud.level);
                }
            }
        }
        public void CollectBonus()
        {
            for (int p = listeBonus.Count - 1; p >= 0; p--)
            {
                Bonus mesitems = listeBonus[p];
                mesitems.Update();

                if (pad.BoundingBox.Intersects(mesitems.NextPositionY()))
                {
                    if (mesitems is BonusVie Vie)
                    {
                        mesitems.currentState = Bonus.BonusState.Catch;
                        hud.Vie += mesitems.addlife;
                        assets.PlaySFX(assets.CatchLife);
                        listeBonus.RemoveAt(p);
                    }
                    else if (mesitems is BonusImpact BigBall)
                    {
                        mesitems.currentState = Bonus.BonusState.Catch;
                        ball.CurrentBallState = Balle.BallState.Big;
                        assets.PlaySFX(assets.CatchLife);
                        listeBonus.RemoveAt(p);
                    }
                    else  if (mesitems is BonusGun gun)
                    {
                        mesitems.currentState = Bonus.BonusState.Catch;
                        bullet.BulletState = Bullet.State.Activated;
                        assets.PlaySFX(assets.CatchLife);
                        listeBonus.RemoveAt(p);
                    }
                }
            }
        }

        public void ColPadPerso()
        {
            for (int p = level.listPerso.Count - 1; p >= 0; p--)
            {
                Personnages mesPerso = level.listPerso[p];
                mesPerso.Update();

                if (pad.BoundingBox.Intersects(mesPerso.NextPositionY()))
                {
                    if (mesPerso is PersonnageFire pFire)
                    {
                        pFire.CurrentState = Personnages.State.Catch;
                        ball.CurrentBallState = Balle.BallState.SpeedUp;
                        assets.PlaySFX(assets.CatchLife);
                        screen.currentState = ScreenManager.State.Narrow;
                        level.listPerso.Remove(pFire);
                    }
                    if (mesPerso is PersonnageIce pIce)
                    {
                        pIce.CurrentState = Personnages.State.Catch;
                        ball.CurrentBallState = Balle.BallState.SlowDown;
                        assets.PlaySFX(assets.CatchLife);
                        screen.currentState = ScreenManager.State.Wide;
                        level.listPerso.Remove(pIce);
                    }
                }
            }
        }


        public void CollisionBulletsBricks()
        {
            for (int bl = bullet.ListeBalles.Count - 1; bl >= 0; bl--)
            {
                bullet.collision = false;
                Bullet bullets = bullet.ListeBalles[bl];
                for (int b = level.listBriques.Count - 1; b >= 0; b--)
                {
                    Briques mesBriques = level.listBriques[b];
                    mesBriques.Update();
                    if (mesBriques.IsScalling == false)
                    {
                        if (bullets.BoundingBox.Intersects(mesBriques.BoundingBox))
                        {
                            bullet.collision = true;
                            mesBriques.nbHits -= bullets.impact;
                            bullet.ListeBalles.Remove(bullets);
                        }

                        if (mesBriques.nbHits <= 0)
                        {
                            switch (mesBriques.id)
                            {
                                case Briques.ID.Feu:
                                    mesBriques.rotate = true;
                                    break;
                                default:
                                    break;
                            }
                            mesBriques.scalling = true;
                            hud.globalScore += mesBriques.points;
                            pop.SetPosition(mesBriques.Position.X, mesBriques.Position.Y);
                            listePopUp.Add(pop);
                            break;
                        }
                        if (mesBriques.scale <= 0)
                        {
                            level.listBriques.Remove(mesBriques);
                            listePopUp.Remove(pop);
                        }
                    }
                }

                if (bullets.Position.Y <= hud.Hudhauteur)
                {
                    bullet.ListeBalles.Remove(bullets);
                }

                if (!bullet.collision)
                {
                    bullets.Update();
                }

            }
        }
        public void CollisionBricks()
        {
            for (int b = level.listBriques.Count - 1; b >= 0; b--)
            {
                ball.collision = false;
                Briques mesBriques = level.listBriques[b];
                mesBriques.Update();

                if (mesBriques.IsScalling == false)
                {
                    if (mesBriques.BoundingBox.Intersects(ball.NextPositionY()))
                    {
                        ball.collision = true;
                        assets.PlaySFX(assets.hitBricks);
                        mesBriques.nbHits -= ball.Impact;
                        ball.InverseVitesseY();
                    }
                    else if (mesBriques.BoundingBox.Intersects(ball.NextPositionX()))
                    {
                        ball.collision = true;
                        assets.PlaySFX(assets.hitBricks);
                        mesBriques.nbHits -= ball.Impact;
                        ball.InverseVitesseX();
                    }

                    if (ball.collision && mesBriques.isBreakable == true)
                    {
                        if (mesBriques.nbHits <= 0)
                        {
                            if (mesBriques is BriqueFeu Fire)
                            {
                                Fire.rotate = true;
                                Fire.scalling = true;
                                hud.globalScore += Fire.points;
                                Fire.currentState = Briques.State.Broken;

                                int dice = rnd.Next(1, 11);
                                if (dice >= 1 && dice <= 5)
                                {
                                    life = new BonusVie(_content.Load<Texture2D>("Heart"));
                                    life.SetPositionBonus(Fire.Position.X, Fire.Position.Y);
                                    life.currentState = Bonus.BonusState.Free;
                                    listeBonus.Add(life);
                                }
                                else if (dice >= 6 && dice <= 10)
                                {
                                    gun = new BonusGun(_content.Load<Texture2D>("Gun"));
                                    gun.SetPositionBonus(Fire.Position.X, Fire.Position.Y);
                                    gun.currentState = Bonus.BonusState.Free;
                                    listeBonus.Add(gun);
                                }

                                pop.SetPosition(Fire.Position.X, Fire.Position.Y);
                                listePopUp.Add(pop);
                            }
                            if (mesBriques is BriqueGlace Glace)
                            {
                                Glace.scalling = true;
                                hud.globalScore += Glace.points;
                                Glace.currentState = Briques.State.Broken;

                                int dice = rnd.Next(1, 11);
                                if (dice >= 1 && dice <= 5)
                                {
                                    bigBall = new BonusImpact(_content.Load<Texture2D>("bMenu"));
                                    bigBall.SetPositionBonus(Glace.Position.X, Glace.Position.Y);
                                    bigBall.currentState = Bonus.BonusState.Free;
                                    listeBonus.Add(bigBall);
                                }
                                pop.SetPosition(Glace.Position.X, Glace.Position.Y);
                                listePopUp.Add(pop);
                            }
                            else
                            {
                                mesBriques.scalling = true;
                                hud.globalScore += mesBriques.points;
                                pop.SetPosition(mesBriques.Position.X, mesBriques.Position.Y);
                                listePopUp.Add(pop);
                            }
                        }
                    }
                    if (mesBriques.scale <= 0)
                    {
                        level.listBriques.Remove(mesBriques);
                        listePopUp.Remove(pop);
                    }
                }
            }
        }

        public void CollisionSolidBricks()
        {
            for (int bricks = level.listSolidBricks.Count - 1; bricks >= 0; bricks--)
            {
                Briques solidB = level.listSolidBricks[bricks];
                solidB.Update();
                if (solidB.BoundingBox.Intersects(ball.NextPositionY()))
                {
                    ball.collision = true;
                    CamShake = 30;
                    assets.PlaySFX(assets.hitBricks);
                    ball.InverseVitesseY();
                }
                else if (solidB.BoundingBox.Intersects(ball.NextPositionX()))
                {
                    ball.collision = true;
                    CamShake = 30;
                    assets.PlaySFX(assets.hitBricks);
                    ball.InverseVitesseX();
                }

                for (int b = bullet.ListeBalles.Count - 1; b >= 0; b--)
                {
                    Bullet bullets = bullet.ListeBalles[b];
                    if (bullets.BoundingBox.Intersects(solidB.BoundingBox))
                    {
                        hud.globalScore += solidB.points;
                        assets.PlaySFX(assets.hitBricks);
                        level.listSolidBricks.Remove(solidB);
                        bullet.ListeBalles.Remove(bullets);
                    }
                }
            }          
        }

        public void LoseLife()
        {
            if (ball.Position.Y > screen.Height)
            {
                hud.Vie--;
                //audio.PlaySFX(audio.ballLost);
                stick = true;
                if (hud.Vie <= 0)
                {
                    ball.CurrentBallState = Balle.BallState.Dead;
                    status.ChangeScene(GameState.Scenes.GameOver);
                    screen.currentState = ScreenManager.State.Basic;
                    hud.Vie = 3;
                }
            }
        }
        public override void DrawBackground()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            pBatch.Draw(background, new Vector2(0, 0), Color.White);
        }
        public override void DrawScene()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            bullet.DrawWeapon();
            pad.Draw();
            ball.DrawBall();

            pBatch.Draw(hudBarre, new Vector2(0, 0), Color.White);
            hud.DrawData();

            foreach (var Bonus in listeBonus)
            {
                pBatch.Draw(Bonus.texture,
                            Bonus.Position,
                            null,
                            Color.White,
                            0,
                            new Vector2(Bonus.HalfWidth, Bonus.HalfHeitgh),
                            1.0f,
                            SpriteEffects.None,
                            0);
            }

            level.DrawLevel();

            foreach (var PopUp in listePopUp)
            {
                foreach (var Briques in level.listBriques)
                {
                    if (Briques.nbHits <= 0)
                    {
                        string points = "+" + Briques.points.ToString();
                        PopUp.DrawPopUp(points);
                    }
                }
            }
        }
    }
}