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

    internal enum EnumFieldColumnState
    {
        FREE,
        OCCUPIED,
        DONE
    }

    internal class NPCJobFarm : NPCJobArea
    {
        public int DayToCheck;

        public Item seed;
        public Block cropBlock;
        public EnumFarmState State;

        public int columnLength;
        private Dictionary<int, EnumFieldColumnState> columns;

        public NPCJobFarm(Cuboidi area, string crop, ICoreServerAPI api, NPCJobStockpile stockpile) : base(area, api)
        {
            this.stockpile = stockpile;

            this.seed = api.World.GetItem(new AssetLocation("game", $"seeds-{crop}"));

            int finalStage = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-1")).CropProps.GrowthStages;
            this.cropBlock = api.World.GetBlock(new AssetLocation("game", $"crop-{crop}-{finalStage}"));

            State = EnumFarmState.TILLING;

            this.columnLength = area.SizeZ;
            this.columns = CreateFieldColumns();
        }

        public override void TryGetDuty(EntityDominionsNPC npc, ref IDuty duty)
        {
            TryUpdateFarmState();

            int nextCol = GetNextFieldColumnStart();

            if (nextCol == -1) return;

            BlockPos startPos = area.Start.AsBlockPos.AddCopy(nextCol, 0, 0);
            BlockPos endPos = area.Start.AsBlockPos.AddCopy(nextCol, 0, area.SizeZ);

            switch (State)
            {
                case EnumFarmState.TILLING:
                    if (npc.HasItemEquipped(EnumTool.Hoe))
                    {
                        columns[nextCol] = EnumFieldColumnState.OCCUPIED;
                        duty = new ErrandTillSoil(api, startPos, endPos, OnFinished(nextCol));
                    }
                    else
                    {
                        if (npc.RightHandItemSlot.Empty)
                        {
                            duty = new ErrandSearchForTool(EnumTool.Hoe, stockpile);
                        }
                        else
                        {
                            duty = new ErrandDepositItems(stockpile);
                        }
                    }
                    break;

                case EnumFarmState.SOWING:
                    if (npc.HasItemEquipped(seed, area.SizeZ + 1))
                    {
                        columns[nextCol] = EnumFieldColumnState.OCCUPIED;
                        duty = new ErrandSowCrop(api, startPos, endPos, seed.LastCodePart(), OnFinished(nextCol));
                    }
                    else
                    {
                        if (npc.RightHandItemSlot.Empty)
                        {
                            duty = new ErrandSearchForItem(seed, area.SizeZ + 1, stockpile);
                        }
                        else
                        {
                            duty = new ErrandDepositItems(stockpile);
                        }
                    }
                    break;

                case EnumFarmState.GROWING:
                    // I know, I know...
                    if (DayToCheck > api.World.Calendar.DayOfYear) return;

                    if (!IsReadyForHarvest())
                    {
                        DayToCheck = api.World.Calendar.DayOfYear + 1;
                        return;
                    }

                    goto case EnumFarmState.HARVESTING;

                case EnumFarmState.HARVESTING:
                    if (npc.RightHandItemSlot.Empty)
                    {
                        duty = new ErrandHarvestCrop(api, startPos, endPos, cropBlock, OnFinished(nextCol));
                    }
                    else
                    {
                        duty = new ErrandDepositItems(stockpile);
                    }
                    break;
            }
        }

        public int GetNextFieldColumnStart()
        {
            foreach (int key in columns.Keys.ToList())
            {
                if (columns[key] == EnumFieldColumnState.FREE)
                {
                    return key;
                }
            }

            return -1;
        }

        public void MarkFieldColumnDone(int col)
        {
            columns[col] = EnumFieldColumnState.DONE;
        }

        private Dictionary<int, EnumFieldColumnState> CreateFieldColumns()
        {
            Dictionary<int, EnumFieldColumnState> columns = new Dictionary<int, EnumFieldColumnState>();

            for (int x = 0; x <= area.SizeX; x++)
            {
                columns.Add(x, EnumFieldColumnState.FREE);
            }

            return columns;
        }

        private void TryUpdateFarmState()
        {
            if (State == EnumFarmState.GROWING || columns.All(col => col.Value == EnumFieldColumnState.DONE))
            {
                State = NextFarmState();
            }
        }

        private EnumFarmState NextFarmState()
        {
            foreach (int key in columns.Keys.ToList())
            {
                columns[key] = EnumFieldColumnState.FREE;
            }

            switch (State)
            {
                case EnumFarmState.TILLING:
                    return EnumFarmState.SOWING;

                case EnumFarmState.SOWING:
                    DayToCheck = api.World.Calendar.DayOfYear + 1;
                    return EnumFarmState.GROWING;

                case EnumFarmState.GROWING:
                    return EnumFarmState.HARVESTING;

                default:
                    return EnumFarmState.SOWING;
            }
        }

        private bool IsReadyForHarvest()
        {
            bool isReady = true;

            api.World.BlockAccessor.SearchBlocks(area.Start.AsBlockPos, area.End.AsBlockPos,
               (Block block, BlockPos pos) =>
               {
                   if (api.World.BlockAccessor.GetBlock(pos) is BlockCrop crop)
                   {
                       if (crop == cropBlock) return false;

                       return !(isReady = false);
                   }

                   return false;
               });

            return isReady;
        }

        private Vintagestory.API.Common.Action<bool> OnFinished(int col) =>
            (bool cancelled) =>
            {
                columns[col] = cancelled ? EnumFieldColumnState.FREE : EnumFieldColumnState.DONE;
            };
    }
}