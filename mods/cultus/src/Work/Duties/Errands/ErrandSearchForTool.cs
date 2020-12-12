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
        private IDuty SubErrand;

        private EnumTool tool;

        private NPCJobStockpile stockpile;

        private List<BlockEntityContainer> containers;

        public ErrandSearchForTool(EnumTool tool, NPCJobStockpile stockpile)
        {
            SubErrand = new ErrandDepositItems(stockpile);

            this.tool = tool;

            this.stockpile = stockpile;

            this.containers = stockpile.FindContainers();
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
            SubErrand.Init(npc);

            this.cd = 2;
        }

        public override bool ShouldRun()
        {
            return !npc.HasItemEquipped(tool);
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

            BlockEntityContainer container = containers.PopOne();

            foreach (ItemSlot slot in container.Inventory)
            {
                if (slot.Empty || slot.Itemstack.Item == null || !slot.Itemstack.Item.Tool.HasValue) continue;

                if (slot.Itemstack.Item.Tool == tool)
                {
                    ItemStack neededItem = slot.TakeOut(1);
                    container.MarkDirty();
                    npc.TryGiveItemStack(neededItem);

                    break;
                }
            }
        }
    }
}