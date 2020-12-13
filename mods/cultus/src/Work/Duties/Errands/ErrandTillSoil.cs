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
        private IDuty SubErrand;

        private ICoreServerAPI api;
        private BlockPos curPos;
        private BlockPos end;

        public ErrandTillSoil(ICoreServerAPI api, BlockPos start, BlockPos end, NPCJobStockpile stockpile)
        {
            SubErrand = new ErrandSearchForItem(IsTool(EnumTool.Hoe), stockpile);

            this.api = api;
            this.curPos = start;
            this.end = end;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
            SubErrand.Init(npc);
        }

        public override bool ShouldRun()
        {
            return curPos.Z <= end.Z;
        }

        public override BlockPos NextBlock()
        {
            return SubErrand.ShouldRun() ? SubErrand.NextBlock() : curPos;
        }

        public override void Run(float dt)
        {
            if (SubErrand.ShouldRun())
            {
                SubErrand.Run(dt);
                return;
            }

            BlockPos groundPos = curPos.DownCopy(1);
            Block block = api.World.BlockAccessor.GetBlock(groundPos);
            Block farmland = api.World.GetBlock(new AssetLocation("farmland-dry-" + block.LastCodePart(1)));

            if (farmland != null && !block.Equals(farmland))
            {
                if (ShouldWait(dt)) return;

                api.World.BlockAccessor.SetBlock(farmland.BlockId, groundPos);
                BlockEntity be = api.World.BlockAccessor.GetBlockEntity(groundPos);
                if (be is BlockEntityFarmland)
                {
                    ((BlockEntityFarmland)be).CreatedFromSoil(block);
                }
                else
                {
                    return;
                }
                if (block.Sounds != null) api.World.PlaySoundAt(block.Sounds.Place, groundPos.X, groundPos.Y, groundPos.Z, null);
                api.World.BlockAccessor.MarkBlockDirty(groundPos);
            }

            curPos.Add(0, 0, 1);
        }
    }
}