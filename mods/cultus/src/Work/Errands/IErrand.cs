using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace cultus
{
    public interface IErrand
    {
        void Init(EntityDominionsNPC npc);

        bool ShouldRun();

        BlockPos NextBlock();

        void Run(float dt);
    }
}