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

    internal class ErrandSowCrop : Errand
    {
        private ICoreServerAPI api;
        private BlockPos curPos;
        private BlockPos end;

        private string crop;

        public ErrandSowCrop(ICoreServerAPI api, BlockPos start, BlockPos end, string crop, Vintagestory.API.Common.Action<bool> OnFinished = null) : base(OnFinished)
        {
            this.api = api;
            this.curPos = start;
            this.end = end;

            this.crop = crop;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);

            this.elapsed = cd;

            this.npc = npc;
        }

        public override BlockPos NextBlock()
        {
            return curPos;
        }

        public override void Run(float dt)
        {
            BlockPos groundPos = curPos.DownCopy(1);
            BlockEntity be = api.World.BlockAccessor.GetBlockEntity(groundPos);

            if (be is BlockEntityFarmland farmland && farmland.CanPlant())
            {
                if (ShouldWait(dt)) return;

                farmland.TryPlant(api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-1")));
                npc.ConsumeItems(npc.RightHandItemSlot);
                api.World.BlockAccessor.MarkBlockDirty(groundPos);

                if (be.Block.Sounds != null) api.World.PlaySoundAt(be.Block.Sounds.Place, groundPos.X, groundPos.Y, groundPos.Z, null);
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

            if (!npc.HasItemEquipped(api.World.GetItem(new AssetLocation("seeds-" + crop)), end.Z - curPos.Z))
            {
                OnFinished?.Invoke(true);
                return false;
            }

            return true;
        }
    }
}