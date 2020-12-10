using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace cultus
{
    internal class ErrandSearchForItem : Errand
    {
        private Item item;
        private int amount;

        private List<BlockEntityContainer> containers;

        public ErrandSearchForItem(Item item, int amount, NPCJobStockpile stockpile)
        {
            this.item = item;
            this.amount = amount;

            this.containers = stockpile.FindContainers();
        }

        public override void Init(EntityDominionsNPC npc)
        {
            this.cd = 2;

            base.Init(npc);
        }

        public override bool ShouldRun()
        {
            return containers.Count > 0;
        }

        public override BlockPos NextBlock()
        {
            return containers.Count() == 0 ? npc.ServerPos.AsBlockPos : containers.Last().Pos;
        }

        public override void Run(float dt)
        {
            if (ShouldWait(dt)) return;

            BlockEntityContainer container = containers.PopOne();

            foreach (ItemSlot slot in container.Inventory)
            {
                if (slot.Empty) continue;

                if (slot.Itemstack.Item.Equals(item))
                {
                    if (slot.Itemstack.StackSize >= amount)
                    {
                        ItemStack neededItem = slot.TakeOut(amount);
                        npc.TryGiveItemStack(neededItem);

                        container.MarkDirty();

                        break;
                    }
                    else
                    {
                        ItemStack neededItem = slot.TakeOutWhole();
                        npc.TryGiveItemStack(neededItem);

                        container.MarkDirty();
                    }
                }
            }
        }
    }
}