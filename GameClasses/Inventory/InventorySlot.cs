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
using System.Collections.Generic;


namespace RPGWithManagers
{
    public class InventorySlot
    {
        private Vector2 pos,dims;
        private Sprite2D sprite;

        #region Accessors
        public Sprite2D Sprite
        {
            get { return sprite; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Vector2 Dims
        {
            get { return dims; }
        }
        #endregion

        public InventorySlot(Sprite2D Sprite,Vector2 Pos, Vector2 Dims) 
        { 
            pos = Pos;
            dims = Dims;
            sprite = Sprite;
        }

        public virtual void Update()
        {

        }
    }
}
