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
    public class MainScreen : Screen
    {
        private UIDrawer drawUI;

        private MainMenu mainMenu;

        public MainScreen()
        {
            
        }

        public override void Initialize()
        {
            drawUI = new UIDrawer(GraphicsDevice);

            mainMenu = new MainMenu();
            mainMenu.Load();

            base.Initialize();
        }

        public override void Load()
        {

        }

        public override void Update(GameTime gameTime)
        {
            mainMenu.Update(gameTime);
        }

        public override void Unload()
        {
            mainMenu = null;
        }

        public override void Draw()
        {
            drawUI.Begin();
            mainMenu.Draw(drawUI);
            drawUI.End();
        }
    }
}
