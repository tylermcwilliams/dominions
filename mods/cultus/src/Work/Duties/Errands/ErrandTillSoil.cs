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
        private ICoreServerAPI api;
        private BlockPos curPos;
        private BlockPos end;

        public ErrandTillSoil(ICoreServerAPI api, BlockPos start, BlockPos end, Vintagestory.API.Common.Action<bool> OnFinished = null) : base(OnFinished)
        {
            this.api = api;
            this.curPos = start;
            this.end = end;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
        }

        public override BlockPos NextBlock()
        {
            return curPos;
        }

        public override void Run(float dt)
        {
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

        public override bool ShouldRun()
        {
            if (curPos.Z > end.Z)
            {
                OnFinished?.Invoke(false);
                return false;
            }

            if (!npc.HasItemEquipped(EnumTool.Hoe))
            {
                OnFinished?.Invoke(true);
                return false;
            }

            return true;
        }
    }
}