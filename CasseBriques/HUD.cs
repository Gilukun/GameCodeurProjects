using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriques
{
    public class HUD : GUI
    { 
        AssetsManager assets = ServiceLocator.GetService<AssetsManager>();
        Vector2 DimensionScore;
        Vector2 dimensionLife;
        Vector2 dimensionNiveau;
        public int Hudhauteur
        { get
            { 
                return texture.Height;
            }
        }

        public int level;

        public int globalScore;
        public int Vie { get; set; }
        public enum State
        {
            noGun,
            hasGun,
        }
        public State currentState;
        private string score;
        private string life;
        private string niveau;

        Texture2D gun;
    
        public HUD(Texture2D pTexture) : base(pTexture)
        {
            

            texture = pTexture;
            globalScore = 0;
            Vie = 3;
            level = 1;
            gun = assets.GetTexture("iconGun");
        }

        public override void Load()
        {
            
        }
        public override void DrawData()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
            ScreenManager screen = ServiceLocator.GetService<ScreenManager>();

            score = "SCORE" + " " + globalScore;
            DimensionScore = assets.GetSize(score, assets.HUDFont);
            float posXScore = 10f;
            float posYScore = texture.Height / 2 - DimensionScore.Y / 2;
           // boundingBox = AssetsManager.getBoundingBox(Score, Font.HUDFont, positiontext);
           // pBatch.DrawRectangle(boundingBox, Color.Red);
            pBatch.DrawString(assets.HUDFont,   
                            score,
                            new Vector2 (posXScore, posYScore),
                            Color.White);

            life = "VIE" + " " + Vie;
            dimensionLife = assets.GetSize(life, assets.HUDFont);
            float posXVie = screen.Width - dimensionLife.X;
            float posYVie = texture.Height / 2 - dimensionLife.Y / 2;
            pBatch.DrawString(assets.HUDFont,
                           life,
                           new Vector2(posXVie, posYVie),
                           Color.White);

            niveau = "Lvl : " + " " + level ;
            dimensionNiveau= assets.GetSize(niveau, assets.HUDFont);
            float posXLevel = screen.HalfScreenWidth;
            float posYLevel = texture.Height / 2 - dimensionNiveau.Y / 2;
            pBatch.DrawString(assets.HUDFont,
                            niveau,
                            new Vector2(posXLevel, posYLevel),
                            Color.White);
  
            if (currentState == State.hasGun)
            {
                float gunPositionX = screen.Width/2 - 100;
                float gunPositionY = texture.Height / 2 - gun.Height / 2;
                pBatch.Draw(gun, new Vector2(gunPositionX, gunPositionY), Color.White);
            }
        }
    }
}
