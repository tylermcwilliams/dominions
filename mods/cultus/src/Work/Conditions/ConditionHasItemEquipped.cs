using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace cultus
{
    partial class Conditions
    {
        public static bool HasItemEquipped(this EntityDominionsNPC npc, EnumTool tool)
        {
            ItemSlot slot = npc.RightHandItemSlot;

            return !slot.Empty && slot.Itemstack.Item != null && slot.Itemstack.Item.Tool != null && slot.Itemstack.Item.Tool == tool;
        }

        public static bool HasItemEquipped(this EntityDominionsNPC npc, Item item, int qty = 1)
        {
            ItemSlot slot = npc.RightHandItemSlot;

            return !slot.Empty && slot.Itemstack.Item != null && slot.Itemstack.Item == item && slot.StackSize >= qty;
        }
    }
}