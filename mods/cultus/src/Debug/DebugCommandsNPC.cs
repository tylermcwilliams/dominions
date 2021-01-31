using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace cultus
{
#if DEBUG

    internal class DebugCommandsNPC : ModSystem
    {
        private ICoreServerAPI api;
        private Dictionary<string, List<EntityDominionsNPC>> selections;

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.api = api;
            this.selections = new Dictionary<string, List<EntityDominionsNPC>>();

            api.RegisterCommand("dnpc", "", "[new|select|clear|job]", CommandHandler);
        }

        private void CommandHandler(IServerPlayer player, int groupId, CmdArgs args)
        {
            string msg;
            switch (args.PopWord())
            {
                case "new":
                    msg = CreateNPC(player);
                    break;

                case "select":
                    msg = SelectNPC(player);
                    break;

                case "clear":
                    int count = selections.ContainsKey(player.PlayerUID) ? selections[player.PlayerUID].Count : 0;
                    selections.Remove(player.PlayerUID);
                    msg = $"Cleared {count} NPCs from selection";
                    break;

                case "job":
                    AssignJobToNPC(player);
                    msg = "Assigned job";
                    break;

                case "despawn":
                    msg = ClearAllNPCs();
                    break;

                case "inv":
                    msg = ViewInventory(player);
                    break;

                case "quick":
                    msg = "Assets \n";
                    msg += $"{api.WorldManager.GetBlockId(new AssetLocation("game", "blocktypes/liquid/glacierice"))} \n";
                    msg += $"{api.WorldManager.GetBlockId(new AssetLocation("game", "blocktypes/liquid/glacierice.json"))} \n";
                    break;

                default:
                    msg = "Error, Invalid args";
                    break;
            }

            api.BroadcastMessageToAllGroups("", EnumChatType.Notification);

            player.SendMessage(groupId, msg, EnumChatType.CommandSuccess);
        }

        private string CreateNPC(IServerPlayer player)
        {
            EntityDominionsNPC npc = DebugManagerNPC.SpawnNPC(player, api);

            if (npc == null)
            {
                return "Failed to create NPC";
            }

            if (!selections.ContainsKey(@player.PlayerUID)) selections.Add(@player.PlayerUID, new List<EntityDominionsNPC>());
            selections[@player.PlayerUID].Add(npc);

            return "Created npc";
        }

        private string SelectNPC(IServerPlayer player)
        {
            EntitySelection curSel = player.CurrentEntitySelection;

            if (curSel != null && curSel.Entity is EntityDominionsNPC npc)
            {
                if (!selections.ContainsKey(@player.PlayerUID))
                {
                    selections.Add(@player.PlayerUID, new List<EntityDominionsNPC>());
                }

                if (selections[@player.PlayerUID].Contains(npc))
                {
                    selections[@player.PlayerUID].Remove(npc);
                    return "Unselected " + npc.GetName();
                }
                else
                {
                    selections[@player.PlayerUID].Add(npc);
                    return "Selected " + npc.GetName();
                }
            }
            else
            {
                return "Not looking at an NPC";
            }
        }

        private string AssignJobToNPC(IServerPlayer player)
        {
            if (!selections.ContainsKey(player.PlayerUID)) return "No NPCs are selected";

            BlockSelection curSel = player.CurrentBlockSelection;

            if (curSel != null && api.World.BlockAccessor.GetBlockEntity(curSel.Position) is BEAreaMarker marker)
            {
                int count = 0;

                foreach (EntityDominionsNPC npc in selections[player.PlayerUID])
                {
                    npc.Job = marker.job;
                    count++;
                }

                return $"Added {count} NPCs to marker";
            }
            else
            {
                return "Not looking at a marker";
            }
        }

        private string ClearAllNPCs()
        {
            int counter = 0;
            foreach (Entity ent in api.World.LoadedEntities.Values)
            {
                if (ent is EntityDominionsNPC npc)
                {
                    npc.Die();
                    counter++;
                }
            }

            return $"Despawned {counter} NPCs";
        }

        private string ViewInventory(IServerPlayer player)
        {
            string msg = "Contents:\n";
            EntitySelection curSel = player.CurrentEntitySelection;

            if (curSel != null && curSel.Entity is EntityDominionsNPC npc)
            {
                foreach (ItemSlot slot in npc.GearInventory)
                {
                    if (slot.Empty) continue;
                    msg += $"{slot.Itemstack.Item.Code} = {slot.Itemstack.StackSize}\n";
                }
            }

            return msg;
        }

#endif
    }
}