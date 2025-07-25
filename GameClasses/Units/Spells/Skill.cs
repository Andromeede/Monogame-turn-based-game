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
    public class Skill
    {
        private bool active;
        private bool done;
        private bool hasHit;
        private int cooldown, cooldownRemaining;

        private string description;

        private Unit owner;
        private Unit target;

        private Sprite2D icon;

        #region Properties

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public bool Done
        {
            get { return done; }
            set { done = value; }
        }

        public bool HasHit
        {
            get { return hasHit; }
            set { hasHit = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Unit Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public Unit Target
        {
            get { return target; }
            set { target = value; }
        }

        public Sprite2D Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        #endregion

        public Skill(Unit Owner)
        {
            active = false;          
            done = false;
            hasHit = false;

            cooldown = 0;
            cooldownRemaining = 0;

            owner = Owner;
            target = null;        
        }

        public virtual void Update(LevelDataPacket LevelDataPacket)
        {
            if (active && !done) //if the skill if active and not done calls targeting function 
            {
                SkillTarget(LevelDataPacket);
            }
        }

        public virtual void FinishSkill() //ends the skill 
        {            
            done = true;
            hasHit= false;
            target = null;
        }

        public virtual void SkillTarget(LevelDataPacket LevelDataPacket)
        {
            if (!done)
            {
                if (GlobalUtil.mouse.RightClickRelease())
                {
                    ResetSkill();
                }
            }
        }

        public virtual void ResetSkill()
        {
            active = false;           
            done = false;
            target = null;
        }

    }
}
