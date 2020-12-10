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
    using static Conditions;

    internal class ErrandSearchForTool : Errand
    {
        private EnumTool tool;

        private NPCJobStockpile stockpile;

        private List<BlockEntityContainer> containers;

        private bool foundItem;

        public ErrandSearchForTool(EnumTool tool, NPCJobStockpile stockpile)
        {
            this.tool = tool;

            this.stockpile = stockpile;

            this.containers = stockpile.FindContainers();

            this.foundItem = false;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            this.cd = 2;

            base.Init(npc);
        }

        public override bool ShouldRun()
        {
            return !npc.HasItemEquipped(tool);
        }

        public override BlockPos NextBlock()
        {
            return containers.Count() == 0 ? npc.ServerPos.AsBlockPos : containers.Last().Pos;
        }

        public override void Run(float dt)
        {
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
                if (slot.Empty || slot.Itemstack.Item == null || !slot.Itemstack.Item.Tool.HasValue) continue;

                if (slot.Itemstack.Item.Tool == tool)
                {
                    ItemStack neededItem = slot.TakeOut(1);
                    container.MarkDirty();
                    npc.TryGiveItemStack(neededItem);

                    foundItem = true;

                    break;
                }
            }
        }
    }
}