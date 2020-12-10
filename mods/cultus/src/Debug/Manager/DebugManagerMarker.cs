using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace cultus
{
    internal enum EnumMarkerJob
    {
        FARMING,
        SMELTING,
        STOCKPILE
    }

    internal static class DebugManagerMarker
    {
        public static BEAreaMarker CreateJobMarker(BlockPos pos, ICoreServerAPI api, EnumMarkerJob jobType, NPCJobStockpile stockpile = null)
        {
            AssetLocation location = new AssetLocation("game", "areamarker-normal-down");

            int markerId = api.World.BlockAccessor.GetBlock(location).BlockId;

            api.World.BlockAccessor.ExchangeBlock(markerId, pos);
            api.World.BlockAccessor.SpawnBlockEntity("BEAreaMarker", pos);

            BEAreaMarker markerEntity = (BEAreaMarker)api.World.BlockAccessor.GetBlockEntity(pos);

            switch (jobType)
            {
                case EnumMarkerJob.FARMING:
                    if (stockpile == null) break;
                    markerEntity.job = new NPCJobFarm(markerEntity.cube, "rice", api, stockpile);
                    break;

                case EnumMarkerJob.SMELTING:
                    markerEntity.job = new NPCJobSmelting(markerEntity.cube, api);
                    break;

                case EnumMarkerJob.STOCKPILE:
                    markerEntity.job = new NPCJobStockpile(markerEntity.cube, api);
                    break;
            }

            return markerEntity;
        }
    }
}