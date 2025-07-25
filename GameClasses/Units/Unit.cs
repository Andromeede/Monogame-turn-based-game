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
using System.Collections;

namespace RPGWithManagers
{
    public class Unit
    {
        private Load2DManager unitLoader;

        private bool isDead, isTurn, turnDone, actionTaken, isHero, hovering;
        private float maxHealth;
        private string name;

        private Vector2 pos, dims;

        private Sprite2D sprite;

        private Sprite2D bar;
        private Sprite2D barBKG;

        private QuantityDisplayBar hpBar;

        private UnitStats unitStats;

        private Skill activeSkill;

        private List<Skill> skills = new List<Skill>();

        #region Accessors
        public bool ActionTaken
        {
            get { return actionTaken; }
            set { actionTaken = value; }
        }

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        public bool TurnDone
        {
            get { return turnDone; }
            set { turnDone = value; }
        }

        public bool Ishero
        {
            get { return isHero; }
            set { isHero = value; }
        }

        public bool IsTurn
        {
            get { return isTurn; }
            set { isTurn = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Vector2 Pos
        {
            get { return pos; }
        }

        public Vector2 Dims
        {
            get { return dims; }
        }

        public Sprite2D Sprite
        { 
            get { return sprite; } 
        }

        public Sprite2D Bar
        {
            get { return bar; }
        }

        public Sprite2D BarBKG
        {
            get { return barBKG; }
        }

        public UnitStats UnitStats
        {
            get { return unitStats; }
        }

        public QuantityDisplayBar HpBar
        { 
            get { return hpBar; } 
        }

        public List<Skill> Skills
        {
            get { return skills; }
        }

        public Skill ActiveSkill
        {
            get { return activeSkill; }
            set { activeSkill = value; }
        }

        #endregion

        public Unit(Sprite2D Sprite, string Name ,Vector2 Pos, Vector2 Dims) 
        {
            unitLoader = RPGgame.Instance.loadManager;

            isTurn = false;
            turnDone = false;
            isDead = false;
            isHero = false;

            sprite = Sprite;
            name = Name;
            pos = Pos;
            dims = Dims;

            unitStats = new UnitStats();

            SetStats(); //sets unit stats, overriden in children classes
            SetQuantityDisplayBar();
            maxHealth = unitStats.GetValueFromName("Health");
        }

        public virtual void Update(LevelDataPacket LevelDataPacket) 
        {
            hpBar.Update(unitStats.GetValueFromName("Health"), maxHealth);
            
            if (actionTaken)
            {
                turnDone = true;
            }
        }    

        public virtual void SetStats()
        {
            unitStats.Clear(); //clears previous stats

            unitStats.SetStatValue("Health", 1);
            unitStats.SetStatValue("Damage", 1);
            unitStats.SetStatValue("Speed", 1);
        }

        public virtual void SetQuantityDisplayBar()
        {
            bar = unitLoader.LoadSprite2D("2d/Misc/solid");
            barBKG = unitLoader.LoadSprite2D("2d/Misc/shade");
            hpBar = new QuantityDisplayBar(bar, barBKG, new Vector2(52, 10), 2, Color.Red);
        }

        public virtual void TakeDamage(Unit Attacker, int Damage) //take damage function and checks if the unit dies 
        {
            unitStats.AddStatValue("Health", -Damage);

            if (unitStats.GetValueFromName("Health") <= 0)
            {
                isDead = true;
            }
        }

        public virtual void TurnComplete() //ends the unit turn
        {
            turnDone = true;
            isTurn = false;
        }

        public virtual void TurnStart() //starts the unit turn
        {
            isTurn = true;
            turnDone = false;
            actionTaken = false;
        }
    }
}
