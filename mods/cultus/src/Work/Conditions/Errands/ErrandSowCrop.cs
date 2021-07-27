using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private IErrand SubErrand;

        private ICoreServerAPI api;
        private BlockPos curPos;
        private BlockPos end;

        private string crop;

        public ErrandSowCrop(ICoreServerAPI api, BlockPos start, BlockPos end, string crop, NPCJobStockpile stockpile)
        {
            SubErrand = new ErrandSearchForItem(IsPlantableSeed(crop), stockpile, end.Z - start.Z + 1);

            this.api = api;
            this.curPos = start;
            this.end = end;

            this.crop = crop;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);
            SubErrand.Init(npc);

            this.timeElapsed = cooldown;

            this.npc = npc;
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
            BlockEntity be = api.World.BlockAccessor.GetBlockEntity(groundPos);

            if (be is BlockEntityFarmland farmland && farmland.CanPlant())
            {
                if (ShouldWait(dt)) return;

                farmland.TryPlant(api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-1")));
                ((ErrandSearchForItem)SubErrand).amount--;
                npc.ConsumeItems(npc.RightHandItemSlot);
                api.World.BlockAccessor.MarkBlockDirty(groundPos);

                if (be.Block.Sounds != null) api.World.PlaySoundAt(be.Block.Sounds.Place, groundPos.X, groundPos.Y, groundPos.Z, null);
            }

            curPos.Add(0, 0, 1);
        }

        public override bool ShouldRun()
        {
            return curPos.Z <= end.Z;
        }
    }
}