using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Reflection.Metadata;
using System;
using FontStashSharp;


namespace RPGWithManagers
{
    public class LobbyMenu
    {
        private ScreenManager screenManager;
        
        private Load2DManager loaderUI;

        private Button2D playbutton;
        private Button2D exitbutton;

        private Sprite2D menuBackground;
        private Texture2D buttonBackground;

        private FontSystem fontSystem;

        public LobbyMenu()
        {
            loaderUI = RPGgame.Instance.loadManager;
            screenManager = RPGgame.Instance.screenManager;
        }

        public virtual void Load()
        {
            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts
            SpriteFontBase font18 = fontSystem.GetFont(18);

            menuBackground = loaderUI.LoadSprite2D("2d/UI/Backgrounds/NewCharBkg");

            buttonBackground = loaderUI.LoadTexture2D("2d/Misc/SimpleBtn");

            playbutton = new Button2D(buttonBackground, new Vector2(325, 600), new Vector2(96, 32), font18, "Play", Play);        
            exitbutton = new Button2D(buttonBackground, new Vector2(325, 645), new Vector2(96, 32), font18, "Main Menu", MainMenu);
        }

        public virtual void Update(GameTime gameTime)
        {
            playbutton.Update();           
            exitbutton.Update();
        }

        public virtual void Draw(UIDrawer UIDrawer)
        {           
            UIDrawer.Draw(menuBackground, new Vector2(GlobalUtil.screenWidth/2, GlobalUtil.screenHeight/2), new Vector2(GlobalUtil.screenWidth, GlobalUtil.screenHeight), Color.White);

            SpriteFontBase font18 = fontSystem.GetFont(18);
            UIDrawer.DrawString(font18, "Lobby Menu", new Vector2(10, 10), Color.Black);

            UIDrawer.DrawButton(playbutton);           
            UIDrawer.DrawButton(exitbutton);
        }

        public virtual void Play()
        {
            screenManager.Push(new WorldScreen());
        }

        public virtual void MainMenu()
        {           
            screenManager.Pop();
        }
    }
}
