using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using FontStashSharp;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Reflection.Metadata;
using System;
using System.Collections.Generic;

namespace RPGWithManagers
{
    public class ScreenManager
    {
        public Screen activeScreen => screens.Peek(); 

        private Stack<Screen> screens;

        #region Accessors

        public Stack<Screen> Screens
        {
            get { return screens; }
        }
        #endregion

        public ScreenManager() 
        { 
            screens = new Stack<Screen>();
            //activeScreen = screens.Peek();
        }

        public void Push(Screen Screen)
        {
            Screen.Initialize();
            Screen.Load();
            screens.Push(Screen);
        }

        public Screen Pop() 
        {
            var screen = screens.Pop();
            screen.Unload();

            screens.Peek().Load();

            return screen;
        }

        public void PopInOrderFromId(int ID)
        {
            int i = 0;

            foreach (Screen screen in screens)
            {
                if (screen.screenId > ID)
                {
                    i++;
                }
            }

            for (int j = 0; j < i; j++)
            {
                screens.Pop();
            }
        }

        public void Update(GameTime gameTime)
        {
           // activeScreen = screens.Peek();
            activeScreen.Update(gameTime);
            //activeScreen?.Update(gameTime); // ? in case of a null activeScreen
        }

        public void Draw() 
        {
            activeScreen.Draw();
            //activeScreen?.Draw(); // ? in case of a null activeScreen
        }

        
    }
}
