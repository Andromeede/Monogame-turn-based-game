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


namespace RPGWithManagers
{
    public class OptionScreen : Screen
    {
        private UIDrawer drawUI;

        private OptionMenu optionMenu;

        public OptionScreen()
        {
            screenId = 1;
        }

        public override void Initialize()
        {
            drawUI = new UIDrawer(GraphicsDevice);

            optionMenu = new OptionMenu();
            optionMenu.Load();

            base.Initialize();
        }

        public override void Load()
        {

        }

        public override void Update(GameTime gameTime)
        {
            optionMenu.Update(gameTime);
        }

        public override void Unload()
        {
            optionMenu = null;
        }

        public override void Draw()
        {
            drawUI.Begin();
            optionMenu.Draw(drawUI);
            drawUI.End();
        }
    }
}
