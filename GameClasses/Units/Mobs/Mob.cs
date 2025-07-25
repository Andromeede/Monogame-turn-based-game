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
    public class Mob : Unit
    {      
        private MaTimer tempTimer;

        public Mob(Sprite2D Sprite, string Name, Vector2 Pos, Vector2 Dims) : base(Sprite, Name, Pos, Dims)
        {
            tempTimer = new MaTimer(1000);
            SetStats();
        }

        public override void Update(LevelDataPacket LevelDataPacket)
        {
            if (LevelDataPacket.Turn == 1 && IsTurn)
            {
                tempTimer.UpdateTimer();
                
                if (tempTimer.Test())
                {
                    ActionTaken = true;
                    tempTimer.ResetToZero();
                }
            }

            base.Update(LevelDataPacket);
        }

        public override void SetStats()
        {
            UnitStats.SetStatValue("Health", 3);
            UnitStats.SetStatValue("Damage", 3);
            UnitStats.SetStatValue("Speed", 3);
        }

        public override void TakeDamage(Unit Attacker, int Damage) //take damage function and checks if the unit dies 
        {
            UnitStats.AddStatValue("Health", -Damage);

            if (UnitStats.GetValueFromName("Health") <= 0)
            {
                IsDead = true;

                /*int num = Globals.random.Next(0, 0 + 1); // creates a randome number bewtween 0 and 2 (2+1 -> 3 values in total)

                if (num == 0)
                {
                    TestItem item = new TestItem();
                    GameGlobals.PassLootItems(item, 5); //calls the AddToInventory function from UI class

                    TestGoldItem gold = new TestGoldItem();
                    GameGlobals.PassLootItems(gold, Globals.random.Next(25, 30 + 1)); //Globals.random.Next(25, 30 + 1)

                }*/
            }
        }
    }
}
