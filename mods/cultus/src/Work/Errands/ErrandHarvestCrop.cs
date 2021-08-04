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
using Action = Vintagestory.API.Common.Action<bool>;

namespace cultus
{
    internal class ErrandHarvestCrop : Errand
    {
        private EntityDominionsNPC npc;

        private ICoreServerAPI api;
        private BlockPos curPos;
        private BlockPos end;

        private Block ripeCrop;

        public ErrandHarvestCrop(ICoreServerAPI api, BlockPos start, BlockPos end, Block ripeCrop, Vintagestory.API.Common.Action<bool> OnFinished = null) : base(OnFinished)
        {
            this.api = api;
            this.curPos = start;
            this.end = end;

            this.ripeCrop = ripeCrop;
        }

        public override void Init(EntityDominionsNPC npc)
        {
            base.Init(npc);

            this.timeElapsed = 0;

            this.npc = npc;
        }

        public override BlockPos NextBlock()
        {
            return curPos;
        }

        public override void Run(float dt)
        {
            Block block = api.World.BlockAccessor.GetBlock(curPos);

            if (block == ripeCrop)
            {
                if (ShouldWait(dt)) return;

                foreach (BlockDropItemStack drop in block.Drops)
                {
                    ItemStack stack = drop.GetNextItemStack();
                    if (!npc.TryGiveItemStack(stack))
                    {
                        api.World.SpawnItemEntity(stack, curPos.ToVec3d());
                    };
                }
                api.World.BlockAccessor.SetBlock(0, curPos);

                if (block.Sounds != null) api.World.PlaySoundAt(block.Sounds.Place, curPos.X, curPos.Y, curPos.Z, null);
            }

            curPos.Add(0, 0, 1);
        }

        public override bool ShouldRun()
        {
            if (curPos.Z <= end.Z)
            {
                return true;
            }

            OnFinished?.Invoke(false);
            return false;
        }
    }
}