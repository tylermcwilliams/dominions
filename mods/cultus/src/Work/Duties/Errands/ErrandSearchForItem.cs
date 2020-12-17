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

        private ActionBoolReturn<ItemStack> itemTest;

        private List<BlockEntityContainer> containers;

        private NPCJobStockpile stockpile;

        public ErrandSearchForItem(ActionBoolReturn<ItemStack> itemTest, NPCJobStockpile stockpile, int amount = 1)
        {
            // We clear the inventory
            SubErrand = new ErrandDepositAllItems(stockpile);

            this.itemTest = itemTest;
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
            return npc.RightHandItemSlot.StackSize < amount || !itemTest(npc.RightHandItemSlot.Itemstack);
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
                if (!slot.Empty && itemTest(slot.Itemstack))
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