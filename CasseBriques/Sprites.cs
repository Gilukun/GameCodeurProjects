using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CasseBriques
{
    public static class SpriteBatchExtensions // Classe Static qui permet de créer une méthode pour afficher les hitbox
    {
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.Right-1, rectangle.Top, 1, rectangle.Height + 1), color);
        }
    }
    public abstract class Sprites
    {
        public Texture2D texture;
        public Vector2 Position { get; set; }
        public Vector2 Vitesse { get; set; }
        
        public float Speed;
        public int SpriteWidth
        {
            get
            { return texture.Width; }
        }
        public int SpriteHeight
        {
            get
            { return texture.Height; }
        }

        public int HalfWidth
        {
            get
            { return texture.Width/2; }
        }

        public int HalfHeitgh
        {
            get
            { return texture.Height/2; }
        }

        public Rectangle BoundingBox;

        public Sprites(Texture2D pTexture)
        { 
            texture = pTexture;
        }
       

        public virtual void SetPosition(float pX, float pY)
        {
            Position = new Vector2(pX, pY);
        }
        public virtual Rectangle NextPositionX()
        {
            Rectangle NextPosition = BoundingBox;
            NextPosition.Offset(new Point((int)Vitesse.X, 0));
            return NextPosition;
        }
        public virtual Rectangle NextPositionY()
        {
            Rectangle NextPosition = BoundingBox;
            NextPosition.Offset(new Point(0, (int)Vitesse.Y));
            return NextPosition;
        }

        public virtual void InverseVitesseY()
        {
           Vitesse =  new Vector2(Vitesse.X, -Vitesse.Y);
        }

        public virtual void InverseVitesseX()
        {
            Vitesse = new Vector2(-Vitesse.X, Vitesse.Y);
        }
        public virtual void Load()
        { 
        }

        public virtual void Move()
        {
            Position += Vitesse;
        }
        public virtual void Update( )
        {
            Move();
            BoundingBox = new Rectangle((int)(Position.X-HalfWidth),(int)(Position.Y-HalfHeitgh), SpriteWidth, SpriteHeight);
        }

        public virtual void DrawData()
        { 
        }
        public virtual void Draw()
        {
            SpriteBatch pBatch = ServiceLocator.GetService<SpriteBatch>();
           
            pBatch.Draw(texture,
                        Position,
                        null,
                        Color.White,
                        0,
                        new Vector2(HalfWidth, HalfHeitgh),
                        1f,
                        SpriteEffects.None,
                        0);
            //pBatch.DrawRectangle(BoundingBox, Color.Red); // affichage des boundingBox
        }
    }
}
