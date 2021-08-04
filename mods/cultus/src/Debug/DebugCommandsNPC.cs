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
        private ICoreServerAPI sapi;
        private Dictionary<string, List<EntityDominionsNPC>> playerSelectedNpcsPairs;

        public override void StartServerSide(ICoreServerAPI api)
        {
            sapi = api;
            playerSelectedNpcsPairs = new Dictionary<string, List<EntityDominionsNPC>>();

            api.RegisterCommand("dnpc", "", "[new|select|clear|job|despawn|list|inv]", CommandHandler);
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
                    {
                        int selectedNpcCount = playerSelectedNpcsPairs.ContainsKey(player.PlayerUID)
                            ? playerSelectedNpcsPairs[player.PlayerUID].Count
                            : 0;
                        playerSelectedNpcsPairs.Remove(player.PlayerUID);
                        msg = $"Cleared {selectedNpcCount} NPCs from selection";
                        break;
                    }
                case "job":
                    AssignJobToSelectedNPC(player);
                    msg = "Assigned job";
                    break;

                case "despawn":
                    msg = ClearAllNPCs();
                    break;

                case "list": //List selected NPCs
                    {
                        if (!playerSelectedNpcsPairs.ContainsKey(player.PlayerUID))
                        {
                            msg = $"No NPCs Selected.";
                            break;
                        }

                        var selectedNpcs = playerSelectedNpcsPairs[player.PlayerUID];
                        if (selectedNpcs.Count == 0)
                        {
                            msg = $"No NPCs Selected.";
                            break;
                        }

                        var selectedNpcNames = from npc in selectedNpcs select npc.GetName();
                        var npcsNamesJoined = string.Join(", ", selectedNpcNames);
                        msg = $"Selected NPCs: [{npcsNamesJoined}]";
                    }
                    break;
                case "inv":
                    msg = ViewInventory(player);
                    break;

                default:
                    msg = "Error, Invalid args";
                    break;
            }

            //_sapi.BroadcastMessageToAllGroups("", EnumChatType.Notification);

            player.SendMessage(groupId, msg, EnumChatType.CommandSuccess);
        }

        private string CreateNPC(IServerPlayer player)
        {
            EntityDominionsNPC npc = DebugManagerNPC.SpawnNPC(player, sapi);

            if (npc == null)
            {
                return "Failed to create NPC";
            }

            if (!playerSelectedNpcsPairs.ContainsKey(@player.PlayerUID)) playerSelectedNpcsPairs.Add(@player.PlayerUID, new List<EntityDominionsNPC>());
            playerSelectedNpcsPairs[@player.PlayerUID].Add(npc);

            var npcName = npc.GetName();
            return $"Created npc named '{npcName}'";
        }

        private string SelectNPC(IServerPlayer player)
        {
            EntitySelection curSel = player.CurrentEntitySelection;

            if (curSel != null && curSel.Entity is EntityDominionsNPC npc)
            {
                if (!playerSelectedNpcsPairs.ContainsKey(player.PlayerUID))
                {
                    CreateNpcSelectionList(player);
                }

                if (playerSelectedNpcsPairs[player.PlayerUID].Contains(npc))
                {
                    playerSelectedNpcsPairs[player.PlayerUID].Remove(npc);
                    return "Unselected " + npc.GetName();
                }
                else
                {
                    playerSelectedNpcsPairs[player.PlayerUID].Add(npc);
                    return "Selected " + npc.GetName();
                }
            }
            else
            {
                return "Not looking at an NPC";
            }
        }
        private void CreateNpcSelectionList(IServerPlayer player)
        {
            playerSelectedNpcsPairs.Add(player.PlayerUID, new List<EntityDominionsNPC>());
        }

        private string AssignJobToSelectedNPC(IServerPlayer player)
        {
            if (!playerSelectedNpcsPairs.ContainsKey(player.PlayerUID)) return "No NPCs are selected";

            BlockSelection curSel = player.CurrentBlockSelection;

            if (curSel != null
                && sapi.World.BlockAccessor.GetBlockEntity(curSel.Position) is BEAreaMarker marker)
            {
                int count = 0;

                foreach (EntityDominionsNPC npc in playerSelectedNpcsPairs[player.PlayerUID])
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
            int clearedNpcCount = 0;
            foreach (Entity ent in sapi.World.LoadedEntities.Values)
            {
                if (ent is EntityDominionsNPC npc)
                {
                    npc.Die();
                    clearedNpcCount++;
                }
            }

            return $"Despawned {clearedNpcCount} NPCs";
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