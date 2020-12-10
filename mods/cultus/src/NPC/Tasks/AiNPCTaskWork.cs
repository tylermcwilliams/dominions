using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Essentials;

namespace cultus
{
    internal class AiNPCTaskWork : AiTaskWorkBase
    {
        public AiNPCTaskWork(EntityAgent entity) : base(entity)
        {
        }

        public override int Slot => base.Slot;

        public override float Priority => 1.7f;

        public override float PriorityForCancel => 2;

        public override bool ContinueExecute(float dt)
        {
            workBehavior.PerformWork(dt);
            return false;
        }

        public override void FinishExecute(bool cancelled)
        {
            base.FinishExecute(cancelled);
        }

        public override bool ShouldExecute()
        {
            return base.ShouldExecute() && entity.ServerPos.DistanceTo(workBehavior.NextTarget().ToVec3d()) < 3;
        }

        public override void StartExecute()
        {
            base.StartExecute();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}