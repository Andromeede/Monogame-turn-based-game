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
    public class UnitStats
    {
        private List<Stat> stats = new List<Stat>();

        #region Accessors

        public List<Stat> Stats
        {
            get { return stats; }
        }

        #endregion

        public UnitStats() 
        {

        }

        public virtual void AddStatValue(string Name, float Value) //adds a value to a stat, if the stat is new creates the stat and adds the value
        {
            bool found = false;

            for (int i = 0; i < stats.Count; i++)
            {
                if (stats[i].Name == Name)
                {
                    stats[i].AddToValue(Value);
                    found = true;
                }
            }
            if (!found)
            {
                stats.Add(new Stat(Name, Value));
            }
        }

        public virtual void Clear() //clears the StatPack
        {
            stats.Clear();
        }


        public virtual float GetValueFromName(string Name) //returns the stat value from the stat name
        {
            return GetStatFromName(Name).StatValue;
        }

        public virtual Stat GetStatFromName(string StatName) //returns the stat from stat name
        {
            for (int i = 0; i < stats.Count; i++) //find the stat and return it
            {
                if (stats[i].Name == StatName)
                {
                    return stats[i];
                }
            }
            //if stat doesn't exist add it
            AddStatValue(StatName, 1); //1 by defaut to avoid div/0
            //return the added stat

            return stats[stats.Count - 1];
        }

        public virtual void SetStatValue(string Name, float Value) //sets a value to a stat
        {
            Stat temp = GetStatFromName(Name);

            temp.StatValue = Value;
        }
    }
}
