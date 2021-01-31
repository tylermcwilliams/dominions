using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace cultus
{
    using static Conditions;

    internal class ErrandLightFirepit : Errand
    {
        private BlockEntityFirepit firepit;

        private IDuty SubErrand;

        public ErrandLightFirepit(BlockEntityFirepit firepit, NPCJobStockpile stockpile)
        {
            this.firepit = firepit;

            SubErrand = new ErrandSearchForItem(IsItemClass(typeof(ItemFirestarter)), stockpile);
        }

        public override bool ShouldRun()
        {
            return !firepit.IsBurning || (!firepit.canIgniteFuel || !firepit.canSmeltInput());
        }

        public override BlockPos NextBlock()
        {
            return firepit.Pos;
        }

        public override void Run(float dt)
        {
            if (SubErrand.ShouldRun())
            {
                SubErrand.Run(dt);
                return;
            };

            if (ShouldWait(dt)) return;

            firepit.igniteFuel();
        }
    }
}