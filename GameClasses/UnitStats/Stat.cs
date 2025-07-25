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
    public class Stat
    {
        private string name;
        private float statValue;

        #region Accessors

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float StatValue
        {
            get { return statValue; }
            set { statValue = value; }
        }
        #endregion

        public Stat(string Name, float Value)
        {
            name = Name;
            statValue = Value;
        }

        public virtual void AddToValue(float Change)
        {
            statValue += Change;
        }
    }
}
