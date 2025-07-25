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
using RPGWithManagers.EngineClasses;

namespace RPGWithManagers
{
    internal class PauseMenu
    {
        private bool isActive;

        private ScreenManager ScreenManager;

        private ConfirmationPopup confirmLobby;
        private ConfirmationPopup confirmMainMenu;

        private Load2DManager loaderUI;

        private Button2D exitButton;
        private Button2D optionsbutton;
        private Button2D lobbymenubutton;
        private Button2D mainmenubutton;

        private Sprite2D menuBackground;
        private Texture2D buttonBackground;

        private FontSystem fontSystem;

        #region Accessors

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion

        public PauseMenu()
        {
            loaderUI = RPGgame.Instance.loadManager;
            ScreenManager = RPGgame.Instance.screenManager;
        }

        public virtual void Load()
        {
            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts
            SpriteFontBase font18 = fontSystem.GetFont(18);

            buttonBackground = loaderUI.LoadTexture2D("2d/Misc/SimpleBtn");
            optionsbutton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2 - 45), new Vector2(100, 25), font18, "Options", OptionsMenu);
            lobbymenubutton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2), new Vector2(100, 25), font18, "Lobby Menu", ConfirmLobby);
            mainmenubutton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2 + 45), new Vector2(100, 25), font18, "Main Menu", ConfirmMainMenu);
            exitButton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2 + 90), new Vector2(100,25), font18, "Exit", CloseMenu);

            confirmLobby = new ConfirmationPopup(LobbyMenu);
            confirmMainMenu = new ConfirmationPopup(MainMenu);
        }

        public virtual void Update(GameTime gameTime)
        {          
            
            if (confirmLobby.IsActive)
            {
                confirmLobby.Update();
            }
            else if (confirmMainMenu.IsActive)
            {
                confirmMainMenu.Update();
            }
            else
            {
                optionsbutton.Update();
                lobbymenubutton.Update();
                mainmenubutton.Update();
                exitButton.Update();
            }
        }

        public virtual void Draw(UIDrawer UIDrawer)
        {          
            UIDrawer.Draw(loaderUI.testItem, new Vector2(GlobalUtil.screenWidth / 2 , GlobalUtil.screenHeight / 2), new Vector2(200, 400), Color.White);

            SpriteFontBase font18 = fontSystem.GetFont(18);
            UIDrawer.DrawString(font18, "Pause", new Vector2(GlobalUtil.screenWidth / 2 - 25, GlobalUtil.screenHeight / 2 - 200), Color.Black);
         
            UIDrawer.DrawButton(optionsbutton);
            UIDrawer.DrawButton(lobbymenubutton);
            UIDrawer.DrawButton(mainmenubutton);
            UIDrawer.DrawButton(exitButton);

            if (confirmLobby.IsActive)
            {
                confirmLobby.Draw(UIDrawer);
            }

            if (confirmMainMenu.IsActive)
            {
                confirmMainMenu.Draw(UIDrawer);
            }
        }

        public virtual void CloseMenu()
        {
            IsActive = false;
        }

        public virtual void ConfirmLobby()
        {
            confirmLobby.IsActive = true;
        }

        public virtual void ConfirmMainMenu()
        {
            confirmMainMenu.IsActive = true;
        }

        public virtual void LobbyMenu()
        {
            IsActive = false;
            ScreenManager.PopInOrderFromId(1);
        }

        public virtual void MainMenu()
        {
            IsActive = false;
            ScreenManager.PopInOrderFromId(0);
        }

        public virtual void OptionsMenu()
        {
            ScreenManager.Push(new OptionScreen());
        }
    }
}
