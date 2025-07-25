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
    public class Hero : Unit
    {
        private LevelDataPacket dataPacket;

        private List<SkillButton> skillButtons = new List<SkillButton>();

        private List<Sprite2D> skillIcons = new List<Sprite2D>();

        #region Accessors
        public List<SkillButton> SkillButtons
        {
            get { return skillButtons; }
        }

        public List<Sprite2D> SkillIcons
        {
            get { return skillIcons; }
        }
        #endregion

        public Hero(Sprite2D Sprite, string Name, Vector2 Pos, Vector2 Dims) : base(Sprite, Name, Pos, Dims)
        {
            Ishero = true;
            dataPacket = null;
            SetStats();
        }

        public override void Update(LevelDataPacket LevelDataPacket)
        {
            dataPacket = LevelDataPacket;
            
            if (LevelDataPacket.Turn == 0)
            {              
                if (GlobalUtil.keyboard.GetSinglePress("O"))
                {
                    ActionTaken = true;
                }

                if (ActiveSkill != null)
                {
                    ActiveSkill.SkillTarget(LevelDataPacket);

                    if (ActiveSkill.Done)
                    {
                        ActiveSkill.ResetSkill();
                        ActiveSkill = null;
                        ActionTaken = true;
                    }
                }
            }
            
           base.Update(LevelDataPacket);
        }

        public virtual void SetActiveSkill(object SkillInfo) //sets a given skill to active
        {
            Skill tempSkill = (Skill)SkillInfo;//sets the info input as a skill

            if (ActiveSkill == null || !ActiveSkill.Equals(tempSkill))//if we have no active skill or the active skill is different from the tempSkill 
            {
                ActiveSkill = tempSkill; //sets the unit active skill to tempSkill
                ActiveSkill.Active = true; //activates the skill
            }
            else //resets the active skill if we have a previous levelUnitPacket
            {
                if (dataPacket != null)
                {
                    ActiveSkill.ResetSkill();
                }

                ActiveSkill = null;
            }
        }

        public virtual void SetAvailableSkills()
        {
            for (int i = 0; i < skillButtons.Count; i++)
            {             
                skillButtons[i].Active = true;
            }
        }

        public virtual void SetUnAvailableSkills()
        {
            for (int i = 0; i < skillButtons.Count; i++)
            {
                skillButtons[i].Active = false;
            }
        }

        public override void SetStats()
        {
            UnitStats.SetStatValue("Health", 1);
            UnitStats.SetStatValue("Damage", 1);
            UnitStats.SetStatValue("Speed", 1);
        }
    }
}
