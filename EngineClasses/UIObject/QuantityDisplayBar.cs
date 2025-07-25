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

namespace RPGWithManagers
{
    public class QuantityDisplayBar
    {
        private int border;
        private float size;
        private Vector2 pos,dims;
        private Sprite2D bar, barBKG;
        private Color color;

        #region Accessors

        public int Border
        {
            get { return border; }
        }
        public float Size
        {
            get { return size; }           
        }

        public Vector2 Dims
        {
            get { return dims; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Color Color
        {
            get { return color; }
        }
        #endregion
        public QuantityDisplayBar(Sprite2D Bar, Sprite2D BarBKG, Vector2 Dims ,int Border, Color Color) //class constructor
        {
            border = Border;
            color = Color;

            dims = Dims;
            bar = Bar; 
            barBKG = BarBKG; 
        }

        public virtual void Update(float Current, float Max) //update function of the class
        {
            size = Current / Max;
        }
    }
}
