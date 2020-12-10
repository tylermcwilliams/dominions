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
    internal class AiNPCGoTo : AiTaskWorkBase
    {
        public AiNPCGoTo(EntityAgent entity) : base(entity)
        {
        }

        public override int Slot => base.Slot;

        public override float Priority => 1.5f;

        public override float PriorityForCancel => base.PriorityForCancel;

        public override bool ContinueExecute(float dt)
        {
            if (ShouldExecute())
            {
                return base.ContinueExecute(dt);
            }
            else
            {
                return false;
            }
        }

        public override void FinishExecute(bool cancelled)
        {
            base.FinishExecute(cancelled);
        }

        public override bool ShouldExecute()
        {
            if (base.ShouldExecute() && !workBehavior.CanIdle() && entity.ServerPos.DistanceTo(workBehavior.NextTarget().ToVec3d()) > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void StartExecute()
        {
            entity.StartAnimation("walk");
            pathTraverser.NavigateTo(workBehavior.NextTarget().ToVec3d(), 0.05f, 1, OnGoalReached, OnStuck);

            base.StartExecute();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void OnGoalReached()
        {
            entity.StopAnimation("walk");
            pathTraverser.Stop();
        }

        private void OnStuck()
        {
        }
    }
}