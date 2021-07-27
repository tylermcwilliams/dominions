using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace cultus
{
    public class LocalTalk : ILocalTalk
    {
        private readonly ICoreServerAPI sapi;
        private readonly Entity talkingEntity;

        public LocalTalk(ICoreServerAPI sapi, Entity talkingEntity)
        {
            this.sapi = sapi;
            this.talkingEntity = talkingEntity;
        }

        private void MessagePlayersWithinRange(string message, float range)
        {
            var originOfSound = talkingEntity.Pos.XYZ;
            var playersWithinRange = sapi.World.GetPlayersAround(originOfSound, range, range);

            foreach (var player in playersWithinRange)
            {
                sapi.SendMessage(player, -1, message, EnumChatType.AllGroups);
            }
        }

        public void Say(string message, float range)
        {
            MessagePlayersWithinRange(message, range);
        }

    }
}