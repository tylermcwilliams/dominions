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
            entName.SetName(new NpcNameGenerator().GetRandomName());

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

            entity.PositionBeforeFalling.Set(entity.ServerPos.X, entity.ServerPos.Y, entity.ServerPos.Z);

            entity.Attributes.SetString("origin", "playerplaced");

            api.World.SpawnEntity(entity);
        }

    }
}