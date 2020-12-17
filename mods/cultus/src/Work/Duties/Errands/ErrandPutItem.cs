using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace cultus
{
    using static Conditions;

    internal class ErrandPutItem : Errand
    {
        private IDuty SubErrand;
        private ItemStack stack;
        private BlockEntityContainer container;

        public ErrandPutItem(ItemStack stack, BlockEntityContainer container, NPCJobStockpile stockpile)
        {
            SubErrand = new ErrandSearchForItem(IsItemstack(stack), stockpile);

            this.stack = stack;

            this.container = container;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
        }

        public override bool ShouldRun()
        {
            return !npc.RightHandItemSlot.Empty;
        }

        public override BlockPos NextBlock()
        {
            return container.Pos;
        }

        public override void Run(float dt)
        {
            if (SubErrand.ShouldRun())
            {
                SubErrand.Run(dt);
                return;
            }

            if (ShouldWait(dt)) return;

            WeightedSlot bestSlot = container.Inventory.GetBestSuitedSlot(npc.RightHandItemSlot);

            if (bestSlot != null)
            {
                int qty = npc.RightHandItemSlot.StackSize;
                npc.RightHandItemSlot.TryPutInto(npc.Api.World, bestSlot.slot, qty);
            }
        }
    }
}