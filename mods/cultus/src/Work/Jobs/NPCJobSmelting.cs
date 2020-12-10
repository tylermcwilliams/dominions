using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

namespace cultus
{
    internal class NPCJobSmelting : NPCJobArea
    {
        private List<BatchSmelting> batches;

        public NPCJobSmelting(Cuboidi area, ICoreServerAPI api) : base(area, api)
        {
            batches = new List<BatchSmelting>();
        }

        public void FindSmeltingPoints()
        {
            string msg = "No firepit found";
            int counter = 0;

            api.World.BlockAccessor.SearchBlocks(area.Start.ToBlockPos(), area.End.ToBlockPos(),
                (Block block, BlockPos blockPos) =>
                {
                    counter++;

                    if (api.World.BlockAccessor.GetBlockEntity(blockPos) is BlockEntityFirepit firepit)
                    {
                        if (!firepit.inputSlot.Empty)
                        {
                            return true;
                        }

                        msg = "Found firepit!";

                        return false;
                    }

                    return true;
                });

            api.BroadcastMessageToAllGroups($"Searched {counter} blocks\n" + msg, EnumChatType.Notification);
        }
    }
}