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
    public class Inventory
    {
        private Load2DManager inventoryLoader;

        private List<InventorySlot> inventorySlots = new List<InventorySlot>();
        private Sprite2D slotSprite;

        private Vector2 pos; // 1150, 800

        #region Accessors
        public List<InventorySlot> InventorySlots
        {
            get { return inventorySlots; }
        }
        #endregion

        public Inventory(Vector2 Pos) 
        {
            pos = Pos;

            inventoryLoader = RPGgame.Instance.loadManager;

            slotSprite = inventoryLoader.LoadSprite2D("2d/Misc/solid");
            
            for (int i = 0; i < 16; i++)
            {
                inventorySlots.Add(new InventorySlot(slotSprite, new Vector2(pos.X + 54 * (i % 8), pos.Y + 54 * (i / 8)), new Vector2(48,48)));
            }

        }

        public virtual void Update()
        {
            for (int i = 0;i < inventorySlots.Count;i++)
            {
                inventorySlots[i].Update();
            }
        }
    }
}
