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
using RPGWithManagers.EngineClasses;

namespace RPGWithManagers
{
    public class WorldScreen : Screen
    {
                
        private Camera camera;
        private GameDrawer drawer;
        private UIDrawer drawUI;

        private World world;
        private WorldUI worldUI;
        private PauseMenu pauseMenu;

        public override void Initialize()
        {
            camera = new Camera(new Vector2(GlobalUtil.screenWidth, GlobalUtil.screenHeight), true);
            drawer = new GameDrawer(GraphicsDevice);
            drawUI = new UIDrawer(GraphicsDevice);

            world = new World();
            world.Load();

            worldUI = new WorldUI();
            worldUI.Load();

            pauseMenu = new PauseMenu();
            pauseMenu.Load();

            screenId = 2;

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
                world.Update(gameTime);
                worldUI.Update(gameTime);
                camera.UpdateMatrices();

                if (GlobalUtil.keyboard.GetSinglePress("Z"))
                {
                    camera.SetZoomSmooth(2.0f, .1f);
                }
                else if (GlobalUtil.keyboard.GetSinglePress("S"))
                {
                    camera.SetZoomSmooth(.5f, .1f);
                }

                if (GlobalUtil.keyboard.GetSinglePress("R"))
                {
                    camera.ResetCameraValues();
                }
            }
        }

        public override void Unload()
        {
            world = null;
            worldUI = null;
            pauseMenu = null;
        }

        public override void Draw()
        {
            drawer.Begin(camera);
            world.Draw(drawer);
            drawer.End();

            drawUI.Begin();
            worldUI.Draw(drawUI, world.LevelDataPacket, camera.Zoom);

            if (pauseMenu.IsActive)
            {
                pauseMenu.Draw(drawUI);
            }
         
            drawUI.End();
        }
    }
}
