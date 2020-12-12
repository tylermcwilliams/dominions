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
        private IDuty SubErrand;

        public int amount;

        private Item item;

        private List<BlockEntityContainer> containers;

        private NPCJobStockpile stockpile;

        public ErrandSearchForItem(Item item, int amount, NPCJobStockpile stockpile)
        {
            SubErrand = new ErrandDepositItems(stockpile);

            this.item = item;
            this.amount = amount;

            this.containers = stockpile.FindContainers();

            this.stockpile = stockpile;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
            SubErrand.Init(npc);

            this.cd = 2;
        }

        public override bool ShouldRun()
        {
            return npc.RightHandItemSlot.StackSize < amount || npc.RightHandItemSlot.Itemstack.Item != item;
        }

        public override BlockPos NextBlock()
        {
            if (SubErrand.ShouldRun()) return SubErrand.NextBlock();

            return containers.Count() == 0 ? npc.ServerPos.AsBlockPos : containers.Last().Pos;
        }

        public override void Run(float dt)
        {
            if (SubErrand.ShouldRun())
            {
                SubErrand.Run(dt);
                return;
            }

            if (ShouldWait(dt)) return;

            if (containers.Count() == 0)
            {
                containers = stockpile.FindContainers();
                cd = 5;
                return;
            }
            else
            {
                cd = 2;
            }

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