using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory;
using Vintagestory.API.Util;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace cultus
{
    internal class BEAreaMarker : BlockEntity
    {
        public int size = 2;

        public INPCJob job;
        public Cuboidi cube;

        public override void Initialize(ICoreAPI api)
        {
            if (api.Side.IsServer())
            {
                cube = new Cuboidi(
                Pos.AddCopy(-size, 0, -size),
                Pos.AddCopy(size, 0, size)
                ); ;

                //CreateJobs((ICoreServerAPI)api);
            }

            base.Initialize(api);
        }

        public void ShowArea(IPlayer player, ICoreServerAPI api)
        {
            api.World.HighlightBlocks(player, 1, new List<BlockPos>() { cube.Start.AsBlockPos, cube.End.AsBlockPos.AddCopy(1) }, EnumHighlightBlocksMode.Absolute, EnumHighlightShape.Cube);
        }
    }
}