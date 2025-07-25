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
    public class LevelDataPacket
    {
        private int turn, currentHeroTurn, currentMobTurn;
        
        private List<Hero> heroes = new List<Hero>();
        private List<Mob> mobs = new List<Mob>();

        #region Accessors
        public int Turn
        {
            get { return turn; }
        }

        public int CurrentHeroTurn
        {
            get { return currentHeroTurn; }
        }

        public int CurrentMobTurn
        {
            get { return currentMobTurn; }
        }
        public List<Hero> Heroes
        {
            get { return heroes; }
        }

        public List<Mob> Mobs
        {
            get { return mobs; }
        }

        #endregion

        public LevelDataPacket(int Turn, List<Hero> Heroes, List<Mob> Mobs, int CurrentHeroTurn, int CurrentMobTurn)
        {
            turn = Turn;
            heroes = Heroes;
            mobs = Mobs;
            currentHeroTurn = CurrentHeroTurn;
            currentMobTurn = CurrentMobTurn;
        }
    }
}
