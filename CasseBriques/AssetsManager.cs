using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace CasseBriques
{
    public class AssetsManager
    {
        public SpriteFont TitleFont { get; private set; }
        public SpriteFont MenuFont { get; private set; }
        public SpriteFont HUDFont { get; private set; }
        public SpriteFont GameOverFont { get; private set; }
        public SpriteFont ContextualFont { get; private set; }
        public SpriteFont PopUpFont { get; private set; }
        public SpriteFont Victory { get; private set; }
        public Song Intro { get; private set; }
        public Song End { get; private set; }
        public Song InGame { get; private set; }
        public SoundEffect PadRebound { get; private set; }
        public SoundEffect CatchLife { get; private set; }
        public SoundEffect Select { get; private set; }
        public SoundEffect hitBricks { get; private set; }
        public SoundEffect hitWalls { get; private set; }
        public SoundEffect shoot { get; private set; }
        public SoundEffect enlarge { get; private set; }
        public SoundEffect hitMonster { get; private set; }
        public SoundEffect bulletHit { get; private set; }
        public SoundEffect ballLost { get; private set; }

        public void Load()
        {
            //Fonts
            TitleFont = GetFont("TitleFont");
            MenuFont = GetFont("MenuFont");
            HUDFont = GetFont("HUD1Font");
            GameOverFont = GetFont("GameOver");
            ContextualFont = GetFont("PopUpFont");
            PopUpFont = GetFont("PopUps");
            Victory = GetFont("Victory");

            // Soundtracks
            Intro = GetSong("Musics\\Intro");
            InGame = GetSong("Musics\\GamePlay");
            End = GetSong("Musics\\End");

            // SFX
            PadRebound = GetSFX("Musics\\HitMetal");
            CatchLife = GetSFX("Musics\\CatchPersonnage");
            Select = GetSFX("Musics\\Selection");
            hitBricks = GetSFX("Musics\\HitFreeze");
            hitWalls = GetSFX("Musics\\hitcadre");
            shoot = GetSFX("Musics\\shoot");
            enlarge = GetSFX("Musics\\enlarge");
            hitMonster = GetSFX("Musics\\hitMonster");
            bulletHit = GetSFX("Musics\\BulletHits");
            ballLost = GetSFX("Musics\\Dead");

        }
        public Vector2 GetSize(string pText, SpriteFont pFont)
        {
            Vector2 textsize = pFont.MeasureString(pText);
            return textsize;
        }
        public  Rectangle getBoundingBox(string pText, SpriteFont pFont, Vector2 position)
        {
            Vector2 textsize = pFont.MeasureString(pText);
            Rectangle boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)textsize.X, (int)textsize.Y);
            return boundingBox;
        }

        public void PlaySong(Song pSong)
        {
            MediaPlayer.Play(pSong);
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }
        public  SoundEffectInstance PlaySFX(SoundEffect pSound)
        {
            SoundEffectInstance  instance = pSound.CreateInstance();
            instance.Play();
            return instance;
        }

        public Texture2D GetTexture(string pName)
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            Texture2D texture = _content.Load<Texture2D>(pName);
            return texture;
        }

        public SpriteFont GetFont(string pName) 
        
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            SpriteFont font = _content.Load<SpriteFont>(pName);
            return font;
        }
        public Song GetSong(string pName)

        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            Song song = _content.Load<Song>(pName);
            return song;
        }

        public SoundEffect GetSFX(string pName)
        {
            ContentManager _content = ServiceLocator.GetService<ContentManager>();
            SoundEffect sfx = _content.Load<SoundEffect>(pName);
            return sfx;
        }
    }  
}
