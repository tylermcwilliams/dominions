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
        public static bool HasBlock(Block block, IInventory inv)
        {
            bool hasItem = false;

            foreach (ItemSlot slot in inv)
            {
                if (slot.Empty) continue;

                if (slot.Itemstack.Block != null && slot.Itemstack.Block == block)
                {
                    hasItem = true;
                    break;
                }
            }

            return hasItem;
        }
    }
}