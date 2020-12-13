using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace cultus
{
#if DEBUG

    internal class DebugCommandsTests : ModSystem
    {
        private ICoreServerAPI api;
        private Dictionary<string, List<EntityDominionsNPC>> selections;

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.api = api;
            this.selections = new Dictionary<string, List<EntityDominionsNPC>>();

            api.RegisterCommand("dtest", "", "[farm|smelt]", CommandHandler);
        }

        private void CommandHandler(IServerPlayer player, int groupId, CmdArgs args)
        {
            string msg;
            switch (args.PopWord())
            {
                case "farm":
                    msg = TestFarming.SpawnFarmTest(player);
                    break;

                case "farmstock":
                    msg = TestFarming.SpawnFarmWithStockpileTest(player, player.CurrentBlockSelection.Position.UpCopy());
                    break;

                case "smelt":
                    TestSmelting.SpawnSmeltingTest(player);
                    msg = "Ran smelt test";
                    break;

                case "stockpile":
                    msg = "Not implemented";
                    break;

                    ItemStack item = player.InventoryManager.ActiveHotbarSlot.Itemstack;
                    TestStockpile.ItemWithdrawalTest(player, item);
                    msg = "Ran Stockpile test";
                    break;

                default:
                    msg = "Invalid args";
                    break;
            }

            player.SendMessage(groupId, msg, EnumChatType.CommandSuccess);
        }
    }

#endif
}