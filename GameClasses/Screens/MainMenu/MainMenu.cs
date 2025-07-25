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
    public class MainMenu
    {
        private ScreenManager screenManager;
        
        private Load2DManager loaderUI;

        private Button2D playbutton;
        private Button2D optionsbutton;
        private Button2D exitbutton;

        private Sprite2D menuBackground;
        private Texture2D buttonBackground;

        private FontSystem fontSystem;

        public MainMenu()
        {
            loaderUI = RPGgame.Instance.loadManager;
            screenManager = RPGgame.Instance.screenManager;
        }

        public virtual void Load()
        {
            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts
            SpriteFontBase font18 = fontSystem.GetFont(18);

            menuBackground = loaderUI.LoadSprite2D("2d/UI/Backgrounds/MainMenuRPG");

            buttonBackground = loaderUI.LoadTexture2D("2d/Misc/SimpleBtn");
            playbutton = new Button2D(buttonBackground, new Vector2(325, 600), new Vector2(96, 32), font18, "Play", Play);
            optionsbutton = new Button2D(buttonBackground, new Vector2(325, 645), new Vector2(96, 32), font18, "Options", Options);
            exitbutton = new Button2D(buttonBackground, new Vector2(325, 690), new Vector2(96, 32), font18, "Exit", Exit);
        }

        public virtual void Update(GameTime gameTime)
        {
            playbutton.Update();
            optionsbutton.Update();
            exitbutton.Update();
        }

        public virtual void Draw(UIDrawer UIDrawer)
        {
            
            UIDrawer.Draw(menuBackground, new Vector2(GlobalUtil.screenWidth/2, GlobalUtil.screenHeight/2), new Vector2(GlobalUtil.screenWidth, GlobalUtil.screenHeight), Color.White);

            SpriteFontBase font18 = fontSystem.GetFont(18);
            UIDrawer.DrawString(font18, "Main Menu", new Vector2(10, 10), Color.Black);

            UIDrawer.DrawButton(playbutton);
            UIDrawer.DrawButton(optionsbutton);
            UIDrawer.DrawButton(exitbutton);
        }

        public virtual void Play()
        {
            screenManager.Push(new LobbyScreen());
        }

        public virtual void Options()
        {
            screenManager.Push(new OptionScreen());
        }

        public virtual void Exit()
        {
           RPGgame.Instance.Exit();
        }
    }
}
