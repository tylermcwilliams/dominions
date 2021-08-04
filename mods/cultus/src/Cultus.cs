using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace cultus
{
    internal class Cultus : ModSystem
    {
        private ICoreAPI api;

        public override void Start(ICoreAPI api)
        {
            this.api = api;

            RegisterEntities();

            RegisterBlocks();

            RegisterTasks();
        }

        private void RegisterEntities()
        {
            api.RegisterEntity("EntityDominionsNPC", typeof(EntityDominionsNPC));
        }

        private void RegisterBlocks()
        {
            api.RegisterBlockClass("BlockAreaMarker", typeof(BlockAreaMarker));
            api.RegisterBlockEntityClass("BEAreaMarker", typeof(BEAreaMarker));
        }

        private void RegisterTasks()
        {
            AiTaskRegistry.Register("npcwork", typeof(AiTaskWork));
        }
    }
}