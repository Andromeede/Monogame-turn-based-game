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
    public class World
    {
        private int turn, currentHero, currentMob;
        
        private Load2DManager worldLoader;

        private Sprite2D worldSprite;

        private LevelDataPacket levelDataPacket;

        private List<Hero> heroes = new List<Hero>();
        private List<Mob> mobs = new List<Mob>();

        private Sprite2D heroSprite;
        private Sprite2D heroSprite2;
        private Sprite2D mobSprite;

        #region Accessors

        public LevelDataPacket LevelDataPacket
        {
            get { return levelDataPacket; }
        }

        #endregion

        public World() 
        {     
            turn = 0;
            
            worldLoader = RPGgame.Instance.loadManager;
        }

        public virtual void Load()
        {
            heroSprite = worldLoader.LoadSprite2D("2d/Units/Heroes/tri");
            heroSprite2 = worldLoader.LoadSprite2D("2d/Units/Heroes/squa");
            mobSprite = worldLoader.LoadSprite2D("2d/Units/Mobs/Imp");

            heroes.Add(new Mage(heroSprite, "Mage", new Vector2(400, 400), new Vector2(50, 50)));
            heroes.Add(new Warrior(heroSprite2, "Warrior", new Vector2(700, 600), new Vector2(50, 50)));

            mobs.Add(new Mob(mobSprite, "Mob 1", new Vector2(1200, 450), new Vector2(50, 50)));
            mobs.Add(new Mob(mobSprite, "Mob 2", new Vector2(1350, 650), new Vector2(50, 50)));

            TurnStartHeroes();
        }

        public virtual void Update(GameTime gameTime)
        {
            levelDataPacket = new LevelDataPacket(turn, heroes, mobs, currentHero, currentMob);            

            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].Update(levelDataPacket);

                for (int j = 0; j < heroes[i].SkillButtons.Count; j++)
                {
                    heroes[i].SkillButtons[j].Update();
                }               
            }

            for (int i = 0; i < mobs.Count; i++)
            {
                mobs[i].Update(levelDataPacket);

                if (mobs[i].IsDead)
                {
                    mobs.RemoveAt(i);
                    i--;
                }
            }

            if (!AllHeroesDone())
            {
                if (heroes[currentHero].TurnDone)
                {
                    heroes[currentHero].SetUnAvailableSkills();
                    currentHero++;

                    if (currentHero <= heroes.Count - 1)
                    {
                        heroes[currentHero].SetAvailableSkills();
                    }                   
                }
            }

            if (!AllMobsDone())
            {
                if (mobs[currentMob].TurnDone)
                {
                    currentMob++;
                    mobs[currentMob].TurnStart();
                }
            }

            if (turn == 0 && AllHeroesDone()) //calls TurnStartMobs() when it's hero's turn and hero turn is done and there is no more projectiles 
            {
                TurnStartMobs();
            }

            if (turn == 1 && AllMobsDone()) //calls TurnStartHeroes() when it's hero's turn and hero turn is done and there is no more projectiles
            {
                TurnStartHeroes();
            }

        }

        public virtual void Draw(GameDrawer worldDrawer) 
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                worldDrawer.Draw(heroes[i].Sprite, heroes[i].Pos, heroes[i].Dims);
                worldDrawer.DrawQuantityDisplayBar(heroes[i]);
            }

            for (int i = 0; i < mobs.Count; i++)
            {
                worldDrawer.Draw(mobs[i].Sprite, mobs[i].Pos, mobs[i].Dims);
                worldDrawer.DrawQuantityDisplayBar(mobs[i]);
            }
        }

        public virtual bool AllHeroesDone() //checks if all heroes have finished their turn
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                if (!heroes[i].TurnDone)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool AllMobsDone() //checks if all mobs have finished their turn
        {
            for (int i = 0; i < mobs.Count; i++)
            {
                if (!mobs[i].TurnDone)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void TurnStartHeroes() //sets variables to make sure that it is the heroes turn
        {
            turn = 0;
            currentHero = 0;

            if (heroes != null)
            {
                for (int i = 0; i < heroes.Count; i++) //starts all heroes turn
                {
                    heroes[i].TurnStart();
                }
            }

            heroes[0].SetAvailableSkills();

            for (int i = 1 ;i < heroes.Count; i++)
            {
                heroes[i].SetUnAvailableSkills();
            }
        }

        public virtual void TurnStartMobs() //sets variables to make sure that it is the mobs turn
        {
            turn = 1;
            currentMob = 0;
            mobs[currentMob].TurnStart(); //starts mob 1 turn            
        }
    }
}
