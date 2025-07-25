using System;
using System.Collections.Generic;
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
using RPGWithManagers.EngineClasses;



namespace RPGWithManagers
{
    public class RPGgame : Game
    {
        public static RPGgame Instance;

        public Load2DManager loadManager;

        public GraphicsDeviceManager Graphics;

        public ScreenManager screenManager;

        public WorldScreen worldScreen;
        public World gameWorld;

        public RPGgame()
        {
            Instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GlobalUtil.screenWidth = 1600; //width of the screen
            GlobalUtil.screenHeight = 900; //height of the screen

            Graphics.PreferredBackBufferWidth = GlobalUtil.screenWidth;
            Graphics.PreferredBackBufferHeight = GlobalUtil.screenHeight;

            Graphics.ApplyChanges(); //change the screen size 

            screenManager = new ScreenManager();

            loadManager = new Load2DManager(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GlobalUtil.keyboard = new MaKeyboard(); //create keyboard object
            GlobalUtil.mouse = new MaMouseControl(); //create mouse object

            loadManager.LoadGlobalAssests(); //loads Global game assets

            screenManager.Push(new MainScreen());

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed /*|| Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            GlobalUtil.gameTime = gameTime;
            GlobalUtil.keyboard.Update(); //update keyboard
            GlobalUtil.mouse.Update(); //update mouse

            screenManager.Update(gameTime);

            GlobalUtil.keyboard.UpdateOld(); // update previous state of the keyboard
            GlobalUtil.mouse.UpdateOld(); // update previous mouse state

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw();

            base.Draw(gameTime);
        }
    }
}
