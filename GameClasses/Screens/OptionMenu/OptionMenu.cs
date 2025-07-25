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
    public class OptionMenu
    {
        
        private ScreenManager screenManager;

        private Load2DManager loaderUI;

        private Button2D exitbutton;
     
        private Texture2D buttonBackground;

        private FontSystem fontSystem;

        public OptionMenu()
        {
            loaderUI = RPGgame.Instance.loadManager;
            screenManager = RPGgame.Instance.screenManager;
        }

        public virtual void Load()
        {
            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts
            SpriteFontBase font18 = fontSystem.GetFont(18);
           
            buttonBackground = loaderUI.LoadTexture2D("2d/Misc/SimpleBtn");
            exitbutton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2 + 100), new Vector2(100,25), font18, "Back", ExitMenu);
        }

        public virtual void Update(GameTime gameTime)
        {
            exitbutton.Update();
        }

        public virtual void Draw(UIDrawer UIDrawer)
        {            
            SpriteFontBase font18 = fontSystem.GetFont(18);
            UIDrawer.DrawString(font18, "Options", new Vector2(10, 10), Color.Black);
            UIDrawer.DrawString(font18, "Options List :", new Vector2(750, 250), Color.White);

            UIDrawer.DrawButton(exitbutton);
        }

        public virtual void ExitMenu()
        {
            screenManager.Pop();
        }
    }
}
