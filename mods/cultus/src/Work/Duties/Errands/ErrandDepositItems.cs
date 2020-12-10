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
    internal class ErrandDepositItems : Errand
    {
        private List<BlockEntityContainer> containers;

        public ErrandDepositItems(NPCJobStockpile stockpile)
        {
            this.containers = stockpile.FindContainers();
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
            return containers.Count() == 0 ? npc.ServerPos.AsBlockPos : containers.Last().Pos;
        }

        public override void Run(float dt)
        {
            if (ShouldWait(dt)) return;

            WeightedSlot bestSlot = containers.PopOne().Inventory.GetBestSuitedSlot(npc.RightHandItemSlot);

            if (bestSlot != null)
            {
                int qty = npc.RightHandItemSlot.StackSize;
                npc.RightHandItemSlot.TryPutInto(npc.Api.World, bestSlot.slot, qty);
            }
        }
    }
}