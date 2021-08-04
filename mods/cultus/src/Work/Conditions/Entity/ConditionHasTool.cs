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
        public static bool HasTool(EnumTool tool, IInventory inv)
        {
            bool hasHoe = false;

            foreach (ItemSlot slot in inv)
            {
                if (slot.Empty) continue;

                if (slot.Itemstack.Item != null && slot.Itemstack.Item.Tool.HasValue && slot.Itemstack.Item.Tool == tool)
                {
                    hasHoe = true;
                    break;
                }
            }

            return hasHoe;
        }
    }
}