using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace cultus
{
    internal class NPCJobStockpile : NPCJobArea
    {
        private List<BlockEntityContainer> containers;

        public NPCJobStockpile(Cuboidi area, ICoreServerAPI api) : base(area, api)
        {
        }

        public List<BlockEntityContainer> FindContainers()
        {
            if (containers != null && containers.Count() > 0)
            {
                return new List<BlockEntityContainer>(containers);
            }

            containers = new List<BlockEntityContainer>();

            int counter = 0;

            api.World.BlockAccessor.SearchBlocks(area.Start.ToBlockPos(), area.End.ToBlockPos(),
                (Block block, BlockPos blockPos) =>
                {
                    BlockEntity be = api.World.BlockAccessor.GetBlockEntity(blockPos);
                    if (api.World.BlockAccessor.GetBlockEntity(blockPos) is BlockEntityContainer container)
                    {
                        counter++;
                        containers.Add(container);
                    }
                    return true;
                });

            api.BroadcastMessageToAllGroups($"Found {counter} containers\n", EnumChatType.Notification);

            return containers;
        }

        public override void TryGetErrand(EntityDominionsNPC npc, ref IErrand errand)
        {
            throw new NotImplementedException();
        }
    }
}