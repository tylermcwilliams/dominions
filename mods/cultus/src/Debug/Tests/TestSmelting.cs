using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Server;

namespace cultus
{
    internal static class TestSmelting
    {
        public static void SpawnSmeltingTest(IServerPlayer player)
        {
            ICoreServerAPI api = (ICoreServerAPI)player.Entity.World.Api;

            BEAreaMarker marker = DebugManagerMarker.CreateJobMarker(player.CurrentBlockSelection.Position.UpCopy(), api, EnumMarkerJob.SMELTING);

            NPCJobSmelting job = (NPCJobSmelting)marker.job;
            job.FindSmeltingPoints();
        }
    }
}