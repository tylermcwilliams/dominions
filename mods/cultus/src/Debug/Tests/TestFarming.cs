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
    internal class TestFarming
    {
        public static string SpawnFarmTest(IServerPlayer player)
        {
            Entity playerEntity = player.Entity;

            ICoreServerAPI api = (ICoreServerAPI)playerEntity.World.Api;

            BEAreaMarker marker = DebugManagerMarker.CreateJobMarker(player.CurrentBlockSelection.Position.UpCopy(), api, EnumMarkerJob.FARMING);

            EntityDominionsNPC npc = DebugManagerNPC.SpawnNPC(player, api);

            npc.Job = marker.job;

            //npc.GetBehavior<EntityBehaviorNPCWork>().AddWork(marker.job);

            return "Created farm";
        }

        public static string SpawnFarmWithStockpileTest(IServerPlayer player, BlockPos centerPos)
        {
            Entity playerEntity = player.Entity;

            ICoreServerAPI api = (ICoreServerAPI)playerEntity.World.Api;

            // STOCK PILE FIRST
            BlockPos stockpileCenter = centerPos.NorthCopy(11);

            TestStockpile.SpawnStockpileWithHeldItem(player, stockpileCenter);

            NPCJobStockpile stockpile = (NPCJobStockpile)((BEAreaMarker)api.World.BlockAccessor.GetBlockEntity(stockpileCenter)).job;
            // ----

            BEAreaMarker marker = DebugManagerMarker.CreateJobMarker(centerPos, api, EnumMarkerJob.FARMING, stockpile);

            ((NPCJobArea)marker.job).stockpile = stockpile;

            EntityDominionsNPC npc = DebugManagerNPC.SpawnNPC(player, api);

            npc.Job = marker.job;

            //npc.GetBehavior<EntityBehaviorNPCWork>().AddWork(marker.job);

            return "Created farm";
        }
    }
}