using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace cultus
{
    static partial class Conditions
    {
        public static bool HasItem(Item item, IInventory inv, int amount = 1)
        {
            bool hasItem = false;

            foreach (ItemSlot slot in inv)
            {
                if (slot.Empty) continue;

                if (slot.Itemstack.Item != null && slot.Itemstack.Item == item && slot.Itemstack.StackSize >= amount)
                {
                    hasItem = true;
                    break;
                }
            }

            return hasItem;
        }
    }
}