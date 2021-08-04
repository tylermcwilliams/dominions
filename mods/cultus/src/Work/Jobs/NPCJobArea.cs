using System.Collections.Generic;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace cultus
{
    internal abstract class NPCJobArea : INPCJob
    {
        protected ICoreServerAPI api;
        protected Cuboidi area;

        protected List<EntityDominionsNPC> workers;

        public NPCJobStockpile stockpile;

        public NPCJobArea(Cuboidi area, ICoreServerAPI api)
        {
            this.api = api;
            this.area = area;

            this.workers = new List<EntityDominionsNPC>();
        }

        public abstract bool TryGetErrand(EntityDominionsNPC npc, ref IErrand errand);
    }
}