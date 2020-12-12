using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

using static cultus.Conditions;

namespace cultus
{
    internal enum EnumFarmState
    {
        TILLING,
        SOWING,
        GROWING,
        HARVESTING
    }

    internal class NPCJobFarm : NPCJobArea
    {
        public string crop;
        public EnumFarmState State;

        private List<IDuty> dutyQueue;

        private int plantDate;
        private float growthTime;

        public Item seed;
        public Block ripeCrop;

        public NPCJobFarm(Cuboidi area, string crop, ICoreServerAPI api, NPCJobStockpile stockpile) : base(area, api)
        {
            this.dutyQueue = new List<IDuty>();

            this.stockpile = stockpile;

            this.crop = crop;

            this.seed = api.World.GetItem(new AssetLocation("game", $"seeds-{crop}"));

            int finalStage = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-1")).CropProps.GrowthStages;
            this.ripeCrop = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-{finalStage}"));

            this.growthTime = ripeCrop.CropProps.TotalGrowthDays + 1;

            State = EnumFarmState.TILLING;
            dutyQueue = CreateDutyQueue();
        }

        public override void TryGetDuty(EntityDominionsNPC npc, ref IDuty duty)
        {
            if (dutyQueue.Count > 0)
            {
                duty = dutyQueue.PopOne();
            }
            else
            {
                TryUpdateFarmState();
            }
        }

        private void TryUpdateFarmState()
        {
            if (State == EnumFarmState.GROWING && api.World.Calendar.DayOfYear < (plantDate + growthTime))
            {
                return;
            }

            switch (State)
            {
                case EnumFarmState.TILLING:
                    State = EnumFarmState.SOWING;
                    break;

                case EnumFarmState.SOWING:
                    plantDate = api.World.Calendar.DayOfYear;
                    State = EnumFarmState.GROWING;
                    return;

                case EnumFarmState.GROWING:
                    State = EnumFarmState.HARVESTING;
                    break;

                case EnumFarmState.HARVESTING:
                    State = EnumFarmState.SOWING;
                    break;
            }

            dutyQueue = CreateDutyQueue();
        }

        private List<IDuty> CreateDutyQueue()
        {
            List<IDuty> dutyQueue = new List<IDuty>();

            BlockPos startPos = new BlockPos(area.MinX, area.MaxY, area.MinZ);
            BlockPos endPos = startPos.AddCopy(0, 0, area.SizeZ);

            switch (State)
            {
                case EnumFarmState.TILLING:
                    for (int i = 0; i < area.SizeX; i++)
                    {
                        dutyQueue.Add(new ErrandTillSoil(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), stockpile));
                    }
                    break;

                case EnumFarmState.SOWING:
                    for (int i = 0; i < area.SizeX; i++)
                    {
                        dutyQueue.Add(new ErrandSowCrop(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), crop, stockpile));
                    }
                    break;

                case EnumFarmState.HARVESTING:
                    for (int i = 0; i < area.SizeX; i++)
                    {
                        dutyQueue.Add(new ErrandHarvestCrop(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), ripeCrop));
                    }
                    break;
            }

            return dutyQueue;
        }
    }
}