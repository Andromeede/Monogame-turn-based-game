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
    public class Screen
    {
        internal int screenId;

        public RPGgame game;
        public ContentManager Content => game.Content;
        public GraphicsDeviceManager Graphics => game.Graphics;
        public GraphicsDevice GraphicsDevice => game.GraphicsDevice;

        #region Accessors
        public int ScreenId
        {
            get { return screenId; }
        }
        #endregion

        public Screen() 
        { 
            game = RPGgame.Instance;
            screenId = 0;
        }

        public virtual void Initialize()
        {

        }

        public virtual void Load()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Unload()
        {

        }

        public virtual void Draw()
        {

        }
    }
}
