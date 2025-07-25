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
using System.Text.RegularExpressions;
using FontStashSharp;
using System.Collections.Generic;
using NVorbis;

namespace RPGWithManagers
{
    public class Warrior : Hero
    {
        private Load2DManager heroLoader;

        public Warrior(Sprite2D Sprite, string MageName, Vector2 Pos, Vector2 Dims) : base(Sprite, MageName, Pos, Dims)
        {
            Name = MageName;

            heroLoader = RPGgame.Instance.loadManager;
            GenerateSpells();

            SetStats(); //sets mage stats
        }

        public virtual void GenerateSpells()
        {
            SkillIcons.Clear();
            SkillIcons.Add(heroLoader.LoadSprite2D("2d/Skills/Icons/Blink"));
            SkillIcons.Add(heroLoader.LoadSprite2D("2d/Skills/Icons/FireIcon"));
            
            Skills.Clear();
            Skills.Add(new GenericSkill(SkillIcons[0], this));
            Skills.Add(new GenericSkill(SkillIcons[1], this));

            SkillButtons.Clear();

            for (int i = 0; i < Skills.Count; i++)
            {
                SkillButtons.Add(new SkillButton(heroLoader.testTexture, new Vector2(400 + 45 * i, 800), new Vector2(40, 40), GlobalUtil.GetFontArial18(), SetActiveSkill, Skills[i]));
            }
        }

        public override void SetStats()
        {
            UnitStats.SetStatValue("Health", 10);
            UnitStats.SetStatValue("Damage", 3);
            UnitStats.SetStatValue("Speed", 4);
        }
    }
}
