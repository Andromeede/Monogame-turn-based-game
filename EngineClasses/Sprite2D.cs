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
    public class Sprite2D
    {  
        private Texture2D texture;
        #region Accessors

        public Texture2D Texture
        {
            get { return texture; }
        }

        #endregion
        public Sprite2D(Texture2D Texture) 
        {
            texture = Texture;
        }
    }
}
