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
    internal static class DebugManagerNPC
    {
        public static EntityDominionsNPC SpawnNPC(IServerPlayer player, ICoreServerAPI api)
        {
            BlockPos pos = player.Entity.ServerPos.HorizontalAheadCopy(3).AsBlockPos;

            AssetLocation location = new AssetLocation("game", "dominionsNPC");
            Entity entity = CreateEntityInstance(location, api);

            MoveEntityToBlockPos(entity, pos, api);

            EntityBehaviorNameTag entName = entity.GetBehavior<EntityBehaviorNameTag>();
            entName.SetName(GetRandomName());

            return (EntityDominionsNPC)entity;
        }

        private static Entity CreateEntityInstance(AssetLocation location, ICoreServerAPI api)
        {
            IWorldAccessor world = api.World;

            EntityProperties entityProperties = world.GetEntityType(location);

            Entity entity = world.ClassRegistry.CreateEntity(entityProperties);

            return entity;
        }

        private static void MoveEntityToBlockPos(Entity entity, BlockPos pos, ICoreServerAPI api)
        {
            entity.ServerPos.X = pos.X + 2;
            entity.ServerPos.Y = pos.Y;
            entity.ServerPos.Z = pos.Z + 2;

            // FIND OUT WHY WE DO THIS
            entity.Pos.SetFrom(entity.ServerPos);
            //

            entity.PositionBeforeFalling.Set(entity.ServerPos.X, entity.ServerPos.Y, entity.ServerPos.Z);

            entity.Attributes.SetString("origin", "playerplaced");

            api.World.SpawnEntity(entity);
        }

        private static string GetRandomName()
        {
            string[] randomNameParts = new string[] {
                "Ar",
                "Mok",
                "Ul",
                "Kar",
                "Grok",
                "Mor",
                "Ala",
                "Kon",
                "Uro"
            };

            int RNPlength = randomNameParts.Length;
            int randomNameLength = new Random().Next(1, 4);
            Random randomNamePart = new Random();

            string newName = "";

            for (int x = 0; x <= randomNameLength; x++)
            {
                string nameSec = randomNameParts[randomNamePart.Next(1, RNPlength)];

                if (x > 0 && x < 3)
                {
                    nameSec = nameSec.ToLower();
                }
                if (x == 3)
                {
                    nameSec = @"'" + nameSec;
                }

                newName += nameSec;
            }

            return newName;
        }
    }
}