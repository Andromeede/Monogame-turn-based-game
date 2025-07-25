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

namespace RPGWithManagers
{
    public class Load2DManager
    {
        public Sprite2D testItem;
        public Texture2D testTexture;

        private ContentManager contentManager;

        public Load2DManager(ContentManager ContentManager) 
        {           
            contentManager = ContentManager;
        }

        public virtual void LoadGlobalAssests()
        {
           testItem = LoadSprite2D("2d/Misc/solid");
           testTexture = LoadTexture2D("2d/Misc/solid");
        }

        public virtual Sprite2D LoadSprite2D(string SpriteName) /// if we need to use a Sprite2D class later on
        {
            Texture2D texture = contentManager.Load<Texture2D>(SpriteName);
            return new Sprite2D(texture);
        }      

        public virtual Texture2D LoadTexture2D(string SpriteName) // for simple textures without shaders ?
        {
            Texture2D texture = contentManager.Load<Texture2D>(SpriteName);
            return texture;
        }


    }
}
