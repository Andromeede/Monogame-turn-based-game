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

using FontStashSharp;
using System.IO;
using System.Reflection.Metadata;
using System;
using RPGWithManagers.EngineClasses;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace RPGWithManagers
{
    public class WorldUI
    {          
        private Load2DManager loaderUI;

        private Inventory inventory;

        private Sprite2D spriteUI;

        private FontSystem fontSystem;

        public WorldUI()
        {
            loaderUI = RPGgame.Instance.loadManager;
            inventory = new Inventory(new Vector2(1100, 800));
        }

        public virtual void Load()
        {
            spriteUI = loaderUI.LoadSprite2D("2d/Units/Heroes/circ");
            fontSystem = new FontSystem();

            fontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts            
        }

        public virtual void Update(GameTime gameTime)
        {
            inventory.Update();
        }

        public virtual void Draw(UIDrawer UIDrawer, LevelDataPacket LevelDataPacket, float CameraZoom)
        {
            UIDrawer.Draw(spriteUI, new Vector2(300, 200), new Vector2(50, 50), Color.White);
            UIDrawer.Draw(loaderUI.testItem, new Vector2(400, 300), new Vector2(40, 40), Color.White);

            SpriteFontBase font18 = fontSystem.GetFont(18);
            UIDrawer.DrawString(font18, "World UI", new Vector2(20,20), Color.Black);

            UIDrawer.DrawInventory(inventory);

            if (LevelDataPacket != null)
            {
                if (LevelDataPacket.Turn == 0 && !HasHeroesHovered(LevelDataPacket.Heroes)) // Heroes turn draw elements (to be simplified)
                {
                    for (int i = 0; i < LevelDataPacket.Heroes[LevelDataPacket.CurrentHeroTurn].SkillButtons.Count; i++)
                    {
                        UIDrawer.DrawSkillButton(LevelDataPacket.Heroes[LevelDataPacket.CurrentHeroTurn].SkillButtons[i]);
                    }
                }

                if (LevelDataPacket.Turn == 0 && !HasUnitHovered(LevelDataPacket.Heroes, LevelDataPacket.Mobs))
                {                    
                    DrawUnitStats(UIDrawer, font18, LevelDataPacket.Heroes[LevelDataPacket.CurrentHeroTurn]);                                                                     
                }
                else
                {
                    for (int i = 0; i < LevelDataPacket.Heroes.Count; i++)
                    {
                        if (IsHovered(LevelDataPacket.Heroes[i]) && CameraZoom == 1)
                        {
                            DrawUnitStats(UIDrawer, font18, LevelDataPacket.Heroes[i]);

                            for (int j = 0; j < LevelDataPacket.Heroes[i].SkillButtons.Count; j++)
                            {
                                UIDrawer.DrawSkillButton(LevelDataPacket.Heroes[i].SkillButtons[j]);
                            }
                        }
                    }
                }

                if (LevelDataPacket.Turn == 1 && !HasHeroesHovered(LevelDataPacket.Heroes)) // Mobs turn draw elements
                {
                    for (int i = 0; i < LevelDataPacket.Heroes[LevelDataPacket.Heroes.Count - 1].SkillButtons.Count; i++)
                    {
                        UIDrawer.DrawSkillButton(LevelDataPacket.Heroes[LevelDataPacket.Heroes.Count - 1].SkillButtons[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < LevelDataPacket.Heroes.Count; i++)
                    {
                        if (IsHovered(LevelDataPacket.Heroes[i]) && CameraZoom == 1)
                        {
                            for (int j = 0; j < LevelDataPacket.Heroes[i].SkillButtons.Count; j++)
                            {
                                UIDrawer.DrawSkillButton(LevelDataPacket.Heroes[i].SkillButtons[j]);
                            }
                        }
                    }                  
                }

                for (int i = 0; i < LevelDataPacket.Mobs.Count; i++)
                {
                    if (IsHovered(LevelDataPacket.Mobs[i]) && CameraZoom == 1)
                    {
                        DrawUnitStats(UIDrawer, font18, LevelDataPacket.Mobs[i]);
                        break;
                    }
                }

                if (LevelDataPacket.Turn == 0)
                {
                    DrawCurrentTurn(UIDrawer, LevelDataPacket, font18, LevelDataPacket.Heroes[LevelDataPacket.CurrentHeroTurn]);                                                       
                }
                else
                {
                    DrawCurrentTurn(UIDrawer, LevelDataPacket, font18, LevelDataPacket.Mobs[LevelDataPacket.CurrentMobTurn]);                  
                }
            }
        }

        public bool IsHovered(Unit unit)
        {
            return GlobalUtil.Hover(unit.Pos, unit.Dims);
        }

        public bool HasUnitHovered(List<Hero> Heroes, List<Mob> Mobs)
        {
            bool Output = false;
            
            for (int i = 0;i < Heroes.Count; i++)
            {
                if (IsHovered(Heroes[i]))
                {
                    Output = true; 
                    break;
                }
            }

            for (int i = 0; i < Mobs.Count; i++)
            {
                if (IsHovered(Mobs[i]))
                {
                    Output = true;
                    break;
                }
            }

            return Output;
        }

        public bool HasHeroesHovered(List<Hero> Heroes)
        {
            bool Output = false;

            for (int i = 0; i < Heroes.Count; i++)
            {
                if (IsHovered(Heroes[i]))
                {
                    Output = true;
                    break;
                }
            }

            return Output;
        }

        public bool HasMobsHovered(List<Mob> Mobs)
        {
            bool Output = false;

            for (int i = 0; i < Mobs.Count; i++)
            {
                if (IsHovered(Mobs[i]))
                {
                    Output = true;
                    break;
                }
            }

            return Output;
        }

        public virtual void DrawUnitStats(UIDrawer UIDrawer, SpriteFontBase Font, Unit Unit)
        {
            UIDrawer.DrawString(Font, Unit.Name, new Vector2(20, 820), Color.Black);
            UIDrawer.DrawString(Font, "Health points : " + Unit.UnitStats.GetValueFromName("Health"), new Vector2(20, 840), Color.Black);
            UIDrawer.DrawString(Font, "Damage : " + Unit.UnitStats.GetValueFromName("Damage"), new Vector2(20, 860), Color.Black);
            UIDrawer.DrawString(Font, "Speed : " + Unit.UnitStats.GetValueFromName("Speed"), new Vector2(20, 880), Color.Black);
        }

        public virtual void DrawCurrentTurn(UIDrawer UIDrawer, LevelDataPacket LevelDataPacket, SpriteFontBase Font, Unit Unit)
        {
            UIDrawer.DrawString(Font, "Turn : " + LevelDataPacket.Turn, new Vector2(1400, 20), Color.Black);
            UIDrawer.DrawString(Font, "Current unit turn : " + Unit.Name, new Vector2(1400, 40), Color.Black);
        }
    }
}
