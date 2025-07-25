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

using FontStashSharp;
using System.IO;
using System.Reflection.Metadata;
using System;
using RPGWithManagers.EngineClasses;

using System.Collections.Generic;


namespace RPGWithManagers
{
    public class ConfirmationPopup
    {
        private bool isActive;

        private SendFunction confirmFunction;

        private Texture2D background;
        private Texture2D buttonBackground;

        private Button2D confirmButton;
        private Button2D cancelButton;

        private Load2DManager loaderUI;

        private FontSystem fontSystem;

        #region Accessors

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion

        public ConfirmationPopup(SendFunction Confirm) 
        {
            loaderUI = RPGgame.Instance.loadManager;

            confirmFunction = Confirm;

            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts
            SpriteFontBase font18 = fontSystem.GetFont(18);

            isActive = false;

            buttonBackground = loaderUI.LoadTexture2D("2d/Misc/SimpleBtn");
            confirmButton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2 - 70, GlobalUtil.screenHeight / 2 + 45), new Vector2(60, 25), font18, "Yes", ConfirmAction);
            cancelButton = new Button2D(buttonBackground, new Vector2(GlobalUtil.screenWidth / 2 + 70, GlobalUtil.screenHeight / 2 + 45), new Vector2(60, 25), font18, "No", Cancel);
        }

        public void Update()
        {
            confirmButton.Update();
            cancelButton.Update();
        }

        public void Draw(UIDrawer UIDrawer)
        {
            SpriteFontBase font18 = fontSystem.GetFont(18);

            UIDrawer.Draw(loaderUI.testTexture,new Vector2(GlobalUtil.screenWidth / 2, GlobalUtil.screenHeight / 2), new Vector2(250, 150), Color.White);
            UIDrawer.DrawString(font18, "Are you sure ?", new Vector2(GlobalUtil.screenWidth / 2 - 65, GlobalUtil.screenHeight / 2 - 30), Color.Black);
            UIDrawer.DrawButton(confirmButton);
            UIDrawer.DrawButton(cancelButton);
        }

        public void Cancel()
        {
            isActive = false;
        }

        public void ConfirmAction()
        {
            isActive = false;
            confirmFunction();
        }
    }
}
