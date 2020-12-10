using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.MathTools;
using Action = Vintagestory.API.Common.Action;

namespace cultus
{
    internal abstract class Errand : IDuty
    {
        protected EntityDominionsNPC npc;

        protected float cd = 3;
        protected float elapsed;

        protected Errand(Vintagestory.API.Common.Action<bool> OnFinished = null)
        {
            this.OnFinished = OnFinished;
        }

        public virtual void Init(EntityDominionsNPC npc)
        {
            this.npc = npc;
            this.elapsed = 0;
        }

        public abstract bool ShouldRun();

        public abstract BlockPos NextBlock();

        public abstract void Run(float dt);

        protected bool ShouldWait(float dt)
        {
            if ((elapsed += dt) >= cd)
            {
                elapsed = 0;
                return false;
            }

            return true;
        }

        protected Vintagestory.API.Common.Action<bool> OnFinished;
    }
}