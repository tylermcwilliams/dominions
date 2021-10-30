using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
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

        private List<IErrand> errandQueue;

        private int plantDate;
        private float growthTime;

        private Item seed;
        private Block ripeCrop;

        public string crop;
        public EnumFarmState State;

        public NPCJobFarm(Cuboidi area, string crop, ICoreServerAPI api, NPCJobStockpile stockpile) : base(area, api)
        {
            this.errandQueue = new List<IErrand>();

            this.stockpile = stockpile;

            this.crop = crop;

            this.seed = api.World.GetItem(new AssetLocation("game", $"seeds-{crop}"));

            int finalStage = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-1")).CropProps.GrowthStages;
            this.ripeCrop = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-{finalStage}"));

            this.growthTime = ripeCrop.CropProps.TotalGrowthDays + 1;

            State = EnumFarmState.TILLING;
            CreateErrandQueue();
        }

        public override bool TryGetErrand(EntityDominionsNPC npc, ref IErrand errand)
        {
            if (errandQueue.Count > 0)
            {
                errand = errandQueue.PopOne();
                return true;
            }
            else
            {
                UpdateJobState();
                return false;
            }
        }

        private void UpdateJobState()
    {
      if (State == EnumFarmState.GROWING && IsHarvestSeason())
      {
        return;
      }

      if (State == EnumFarmState.HARVESTING)
      {
        State = EnumFarmState.SOWING;
      }
      else
      {
        if (State == EnumFarmState.SOWING) plantDate = api.World.Calendar.DayOfYear;
        State++;
      }

      CreateErrandQueue();
    }

    private bool IsHarvestSeason()
    {
      return api.World.Calendar.DayOfYear < (plantDate + growthTime);
    }

    private void CreateErrandQueue()
        {
            BlockPos startPos = new BlockPos(area.MinX, area.MaxY, area.MinZ);
            BlockPos endPos = startPos.AddCopy(0, 0, area.SizeZ);

            for (int i = 0; i <= area.SizeX; i++)
            {
                if (State == EnumFarmState.TILLING)
                    errandQueue.Add(new ErrandTillSoil(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), stockpile));

                if (State == EnumFarmState.SOWING)
                    errandQueue.Add(new ErrandSowCrop(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), crop, stockpile));

                if (State == EnumFarmState.HARVESTING)
                    errandQueue.Add(new ErrandHarvestCrop(api, startPos.AddCopy(i, 0, 0), endPos.AddCopy(i, 0, 0), ripeCrop));
            }
        }
    }
}