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

        private IErrand errand;

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

            BlockPos destination = errand.NextBlock();

            if (entity.ServerPos.DistanceTo(destination.ToVec3d()) < 3)
            {
                errand.Run(dt);
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
                errand = null;
                return false;
            }

            if (errand == null)
            {
                npc.Job.TryGetErrand(npc, ref errand);
                if (errand != null)
                {
                    errand.Init(npc);
                }
                return false;
            }
            else
            {
                if (errand.ShouldRun()) return true;

                errand = null;
                return false;
            }
        }

        public override void StartExecute()
        {
            base.StartExecute();
        }

        private void OnGoalReached()
        {
            pathTraverser.Stop();
        }

        private void OnStuck()
        {
            if (world.Api is ICoreServerAPI sapi)
            {
                sapi.BroadcastMessageToAllGroups(
                    message: $"{npc.GetName()}: I'm stuck..",
                    chatType: EnumChatType.AllGroups);
            }
        }

        public override void OnEntityDespawn(EntityDespawnReason reason)
        {
            // return errand.

            base.OnEntityDespawn(reason);
        }
    }
}