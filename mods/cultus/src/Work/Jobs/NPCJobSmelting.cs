using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

namespace cultus
{
    using static EnumFoundryState;

    internal enum EnumFoundryState
    {
        PREPMOLDS,
        FUEL,
        METAL,
        LIGHT,
        WAIT,
        POUR,
        COLLECT
    }

    internal class NPCJobSmelting : NPCJobArea
    {
        public EnumFoundryState State;

        // Molds
        private List<BlockPos> moldPos;

        private List<IDuty> dutyQueue;

        public NPCJobSmelting(Cuboidi area, ICoreServerAPI api) : base(area, api)
        {
            moldPos = new List<BlockPos>();
        }

        public override void TryGetDuty(EntityDominionsNPC npc, ref IDuty activeDuty)
        {
            if (dutyQueue.Count > 0)
            {
                activeDuty = dutyQueue.PopOne();
            }
            else
            {
                TryUpdateState();
            }
        }

        private void TryUpdateState()
        {
            switch (State)
            {
                case PREPMOLDS:
                    // get req molds
                    // create mold-place task
                    break;

                case FUEL:
                    // get req fuel from batch
                    // fuel deposit
                    //dutyQueue.Add(new ErrandPutItem(charcoalStack, ));
                    break;

                case METAL:
                    // get req molds
                    // create mold-place task
                    //dutyQueue.Add(new ErrandPutItem(metalStack, ));
                    break;

                case LIGHT:
                    // get req molds
                    // create mold-place task
                    //dutyQueue.Add(new ErrandLight());
                    break;
            }

            dutyQueue = CreateDutyQueue();
        }

        private List<IDuty> CreateDutyQueue()
        {
            List<IDuty> dutyQueue = new List<IDuty>();

            return dutyQueue;
        }
    }
}