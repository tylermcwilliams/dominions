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
    internal class AiTaskWorkBase : AiTaskBase
    {
        protected readonly EntityBehaviorNPCWork workBehavior;

        public AiTaskWorkBase(EntityAgent entity) : base(entity)
        {
            workBehavior = entity.GetBehavior<EntityBehaviorNPCWork>();
        }

        public override int Slot => base.Slot;

        public override float Priority => base.Priority;

        public override float PriorityForCancel => base.PriorityForCancel;

        public override bool ContinueExecute(float dt)
        {
            return base.ContinueExecute(dt);
        }

        public override void FinishExecute(bool cancelled)
        {
            base.FinishExecute(cancelled);
        }

        public override bool ShouldExecute()
        {
            return !workBehavior.CanIdle();
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