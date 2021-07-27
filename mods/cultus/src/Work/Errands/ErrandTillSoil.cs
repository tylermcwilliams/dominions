using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Action = Vintagestory.API.Common.Action;

namespace cultus
{
    using static Conditions;

    internal class ErrandTillSoil : Errand
    {
        private IErrand subErrand;

        private ICoreServerAPI sapi;
        private BlockPos curPos;
        private BlockPos end;
        private EntityDominionsNPC npc;

        public ErrandTillSoil(ICoreServerAPI sapi, BlockPos start, BlockPos end, NPCJobStockpile stockpile)
        {
            subErrand = new ErrandSearchForItem(IsTool(EnumTool.Hoe), stockpile);
            this.sapi = sapi;
            curPos = start;
            this.end = end;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
            this.npc = npc;
            subErrand.Init(npc);
        }

        public override bool ShouldRun()
        {
            return curPos.Z <= end.Z;
        }

        public override BlockPos NextBlock()
        {
            return subErrand.ShouldRun() ? subErrand.NextBlock() : curPos;
        }

        public override void Run(float dt)
        {
            if (subErrand.ShouldRun())
            {
                subErrand.Run(dt);
                return;
            }

            BlockPos groundPos = curPos.DownCopy(1);
            Block block = sapi.World.BlockAccessor.GetBlock(groundPos);
            Block farmland = sapi.World.GetBlock(new AssetLocation("farmland-dry-" + block.LastCodePart(1)));

            if (farmland != null && !block.Equals(farmland))
            {
                if (ShouldWait(dt)) return;

                sapi.World.BlockAccessor.SetBlock(farmland.BlockId, groundPos);
                BlockEntity be = sapi.World.BlockAccessor.GetBlockEntity(groundPos);
                if (be is BlockEntityFarmland beFarmland)
                {
                    beFarmland.CreatedFromSoil(block);
                }
                else
                {
                    return;
                }

                if (block.Sounds != null)
                {
                    sapi.World.PlaySoundAt(block.Sounds.Place, groundPos.X, groundPos.Y, groundPos.Z, null);
                }
                sapi.World.BlockAccessor.MarkBlockDirty(groundPos);
            }

            curPos.Add(0, 0, 1);
        }
    }
}