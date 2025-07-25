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
    public class LobbyScreen : Screen
    {
        private UIDrawer drawUI;

        private LobbyMenu lobbyMenu;
        private PauseMenu pauseMenu;

        public LobbyScreen()
        {
            
        }

        public override void Initialize()
        {
            drawUI = new UIDrawer(GraphicsDevice);
            
            lobbyMenu = new LobbyMenu();
            lobbyMenu.Load();

            pauseMenu = new PauseMenu();
            pauseMenu.Load();

            screenId = 1;

            base.Initialize();
        }

        public override void Load()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GlobalUtil.keyboard.GetSinglePress("P"))
            {
                pauseMenu.IsActive = true;
            }

            if (pauseMenu.IsActive)
            {
                pauseMenu.Update(gameTime);
            }
            else
            {
                lobbyMenu.Update(gameTime);
            }           
        }

        public override void Unload()
        {
            lobbyMenu = null;
            pauseMenu = null;
        }

        public override void Draw()
        {
            drawUI.Begin();
            lobbyMenu.Draw(drawUI);
            
            if (pauseMenu.IsActive)
            {
                pauseMenu.Draw(drawUI);
            }
            
            drawUI.End();
        }
    }
}
