using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class ScenesManager
    {
        private Texture2D background;
        public Rectangle Screen { get; set; }
        protected CasseBriques casseBriques;
        private KeyboardState newKbState;
        private KeyboardState oldKbState;
        GameState Status = ServiceLocator.GetService<GameState>();
        protected int CamShake;
        private Random rnd;

        // Constructeur 
        public ScenesManager()
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            GraphicsDeviceManager screen = ServiceLocator.GetService<GraphicsDeviceManager>();
            int screenWidth = screen.PreferredBackBufferWidth;
            int screenHeight = screen.PreferredBackBufferHeight;
            Screen = new Rectangle(0, 0, screenWidth, screenHeight);
            background = _content.Load<Texture2D>("background");
            rnd = new Random(); 
        }
         

        public virtual void Load()
        {
            
        }

        public virtual void Unload()
        { }

     
        public virtual void Update()
        {
            newKbState = Keyboard.GetState();

            if (newKbState.IsKeyDown(Keys.M) && !oldKbState.IsKeyDown(Keys.M))
            {
                Status.ChangeScene(GameState.Scenes.Menu);
            }
            oldKbState = newKbState;
  
        }

        public virtual void DrawScene()
        { 
        }
        public virtual void DrawBackground()
        {
        }

        public void Draw()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();

            // premier batch qui affiche le background general
            pBatch.Begin();
            pBatch.Draw(background, new Vector2(0,0), Color.White);
            DrawBackground();
            pBatch.End();

            //second batch qui affiche les éléments qui vibrent lors d'une collision
            if (CamShake > 0)
            {
                int offset = rnd.Next(-2, 2); // décallage de la caméra
                pBatch.Begin(SpriteSortMode.Deferred,
                             null,
                             null,
                             null,
                             null,
                             null,
                             Matrix.CreateTranslation(offset, offset, 0f));
                CamShake--;

            }
            else
            pBatch.Begin();
            DrawScene();
            pBatch.End();
            
        }

    }

    

}
