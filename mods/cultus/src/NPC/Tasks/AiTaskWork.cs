using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Essentials;

namespace cultus
{
    internal class AiTaskWork : AiTaskBase
    {
        private readonly EntityDominionsNPC npc;

        private IDuty duty;

        public AiTaskWork(EntityAgent entity) : base(entity)
        {
            if (entity is EntityDominionsNPC npc)
            {
                this.npc = npc;
            }
        }

        public override int Slot => base.Slot;

        public override float Priority => base.Priority;

        public override float PriorityForCancel => base.PriorityForCancel;

        public override bool ContinueExecute(float dt)
        {
            if (!ShouldExecute())
            {
                pathTraverser.Stop();
                return false;
            }

            if (pathTraverser.Active) return true;

            BlockPos destination = duty.NextBlock();

            if (entity.ServerPos.DistanceTo(destination.ToVec3d()) < 3)
            {
                duty.Run(dt);
            }
            else
            {
                pathTraverser.NavigateTo(destination.ToVec3d(), 0.05f, 1, OnGoalReached, OnStuck);
            }

            return base.ContinueExecute(dt);
        }

        public override void FinishExecute(bool cancelled)
        {
            base.FinishExecute(cancelled);
        }

        public override bool ShouldExecute()
        {
            if (npc.Job == null)
            {
                duty = null;
                return false;
            }

            if (duty == null)
            {
                npc.Job.TryGetDuty(npc, ref duty);
                if (duty != null)
                {
                    duty.Init(npc);
                }
                return false;
            }
            else
            {
                if (duty.ShouldRun()) return true;

                duty = null;
                return false;
            }
        }

        public override void StartExecute()
        {
            base.StartExecute();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void OnGoalReached()
        {
            pathTraverser.Stop();
        }

        private void OnStuck()
        {
        }
    }
}