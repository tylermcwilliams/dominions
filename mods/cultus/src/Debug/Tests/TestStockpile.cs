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
    internal static class TestStockpile
    {
        public static void SpawnStockpileWithHeldItem(IServerPlayer player, BlockPos centerPos)
        {
            ICoreServerAPI api = (ICoreServerAPI)player.Entity.Api;

            BEAreaMarker marker = DebugManagerMarker.CreateJobMarker(centerPos, api, EnumMarkerJob.STOCKPILE);

            AssetLocation chestLoc = new AssetLocation("game", "chest-east");
            Block chest = api.World.GetBlock(chestLoc);

            api.World.BlockAccessor.ExchangeBlock(chest.BlockId, marker.Pos.NorthCopy());
            api.World.BlockAccessor.SpawnBlockEntity("GenericTypedContainer", marker.Pos.NorthCopy());

            api.World.BlockAccessor.MarkBlockDirty(marker.Pos.NorthCopy());
            api.World.BlockAccessor.MarkBlockEntityDirty(marker.Pos.NorthCopy());

            NPCJobStockpile stockpile = (NPCJobStockpile)marker.job;

            List<BlockEntityContainer> containers = stockpile.FindContainers();
            containers[0].Inventory[0].Itemstack = player.InventoryManager.ActiveHotbarSlot.Itemstack;
        }

        public static void ItemWithdrawalTest(IServerPlayer player, ItemStack stack)
        {
            throw new NotImplementedException();

            ICoreServerAPI api = (ICoreServerAPI)player.Entity.Api;

            BlockPos playerPos = player.Entity.ServerPos.AsBlockPos;

            BEAreaMarker marker = DebugManagerMarker.CreateJobMarker(player.CurrentBlockSelection.Position.UpCopy(), api, EnumMarkerJob.STOCKPILE);

            AssetLocation chestLoc = new AssetLocation("game", "chest-east");
            Block chest = api.World.GetBlock(chestLoc);

            api.World.BlockAccessor.ExchangeBlock(chest.BlockId, marker.Pos.NorthCopy());
            api.World.BlockAccessor.SpawnBlockEntity("GenericTypedContainer", marker.Pos.NorthCopy());

            NPCJobStockpile stockpile = (NPCJobStockpile)marker.job;

            EntityDominionsNPC npc = DebugManagerNPC.SpawnNPC(player, api);

            EntityBehaviorNPCWork workB = npc.GetBehavior<EntityBehaviorNPCWork>();

            List<BlockEntityContainer> containers = stockpile.FindContainers();
            containers[0].Inventory[0].Itemstack = stack;

            //DutyWithdrawItem duty = new DutyWithdrawItem(stack, stockpile.FindContainers(), npc);

            //workB.AddDuty(duty, true);
        }
    }
}