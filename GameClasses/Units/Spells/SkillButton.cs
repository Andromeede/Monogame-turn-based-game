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
using System.Linq;
using FontStashSharp;

namespace RPGWithManagers
{
    public class SkillButton : Button2D
    {
        private Skill buttonSkill;

        #region Acessors

        public Skill ButtonSkill
        {
            get { return buttonSkill; }
        }

        #endregion

        public SkillButton(Texture2D Texture, Vector2 Pos, Vector2 Dims, SpriteFontBase Font, SendObject ButtonFunction, object Content) 
            : base(Texture,Pos,Dims,Font,"", ButtonFunction, Content)
        {
            buttonSkill = (Skill)Content;          
        }

        public override void Update()
        {
            if (buttonSkill != null) //if there is a skill in the skillbutton sets it to active if the cooldown is 0
            {
                //Active = true;
                base.Update(); //calls the base function which has the RunBtnClick (RunBtnClick from Button2D overriden by the RunBtnClick by SkillButton)
            }
        }
    }
}
