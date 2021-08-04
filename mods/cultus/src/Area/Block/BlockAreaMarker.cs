using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace cultus
{
    internal class BlockAreaMarker : Block
    {
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BEAreaMarker ledger && world.Side.IsServer())
            {
                ICoreServerAPI sapi = (ICoreServerAPI)world.Api;

                ledger.ShowArea(byPlayer, sapi);
            };

            return true;
        }
    }
}