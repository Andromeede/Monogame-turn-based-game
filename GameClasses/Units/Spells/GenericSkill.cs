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


namespace RPGWithManagers
{
    public class GenericSkill : Skill
    {
        //private Sprite2D icon;
        private MaTimer tempTimer;
        private List<Unit> targetableUnits = new List<Unit>();
        public GenericSkill(Sprite2D SkillIcon, Unit Owner) : base(Owner)
        {
            Icon = SkillIcon;
            
            Description = "test spell";

            tempTimer = new MaTimer(1000);
        }

        public override void SkillTarget(LevelDataPacket LevelDataPacket)
        {
            if (Owner.Ishero)
            {
                targetableUnits = LevelDataPacket.Mobs.ToList<Unit>();

                if (GlobalUtil.mouse.LeftClickRelease())
                {
                    for (int i = 0; i < targetableUnits.Count; i++)//loops through all mobs and checks if we have an enemy which is hovered and sets it as target spell
                    {
                        if (GlobalUtil.Hover(targetableUnits[i].Pos, targetableUnits[i].Dims))
                        {
                            Target = targetableUnits[i];
                        }
                    }
                }
            }

            if (Target != null) //if we have a target, launch the spell action
            {
                tempTimer.UpdateTimer();

                if (tempTimer.Timer >= tempTimer.MSec/2 && !HasHit)
                {
                    Target.TakeDamage(Owner,1);
                    HasHit = true;
                }
            }

            if (tempTimer.Test())
            {
                tempTimer.ResetToZero();
                FinishSkill();
            }

            base.SkillTarget(LevelDataPacket);
        }
    }
}
