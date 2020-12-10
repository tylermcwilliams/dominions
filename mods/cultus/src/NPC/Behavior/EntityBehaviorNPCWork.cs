using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace cultus
{
    internal class EntityBehaviorNPCWork : EntityBehavior
    {
        private INPCJob job;

        private List<IDuty> duties;

        public EntityBehaviorNPCWork(Entity entity) : base(entity)
        {
        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            duties = new List<IDuty>();

            EntityBehaviorTaskAI ai = entity.GetBehavior<EntityBehaviorTaskAI>();
            ai.taskManager.AddTask(new AiNPCGoTo((EntityAgent)entity));
            ai.taskManager.AddTask(new AiNPCTaskWork((EntityAgent)entity));
        }

        public void AddWork(INPCJob job)
        {
            this.job = job;
        }

        public void AddDuty(IDuty duty, bool priority = false)
        {
            duty.Init((EntityDominionsNPC)entity);

            if (priority)
            {
                duties.Add(duty);
            }
            else
            {
                duties.Insert(0, duty);
            }
        }

        public BlockPos NextTarget()
        {
            return duties.Last().NextBlock();
        }

        public void PerformWork(float dt)
        {
            duties.Last().Run(dt);

            if (!duties.Last().ShouldRun())
            {
                duties.PopOne();
                return;
            }
        }

        public bool CanIdle()
        {
            if (duties.Count > 0) return false;

            if (duties.Count == 0)
            {
                IDuty duty = null;
                job?.TryGetDuty((EntityDominionsNPC)entity, ref duty);
                if (duty == null) return true;

                AddDuty(duty);
            }
            else if (!duties.Last().ShouldRun())
            {
                duties.PopOne();
                return CanIdle();
            }

            return false;
        }

        public override string PropertyName()
        {
            return "npcwork";
        }
    }
}