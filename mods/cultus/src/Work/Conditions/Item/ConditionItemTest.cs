﻿using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace cultus
{
    partial class Conditions
    {
        public static ActionBoolReturn<ItemStack> IsTool(EnumTool tool)
        {
            return (ItemStack itemstack) =>
            {
                return itemstack.Item != null && itemstack.Item.Tool == tool;
            };
        }

        public static ActionBoolReturn<ItemStack> IsFertilizer(float N, float P, float K)
        {
            return (ItemStack itemstack) =>
            {
                JsonObject obj = itemstack?.Collectible?.Attributes?["fertilizerProps"];
                if (obj == null || !obj.Exists) return false;
                FertilizerProps props = obj.AsObject<FertilizerProps>();
                return props != null && props.N >= N && props.P >= P && props.K >= K;
            };
        }

        public static ActionBoolReturn<ItemStack> IsPlantableSeed(string crop)
        {
            return (ItemStack itemstack) =>
            {
                if (itemstack.Item == null || itemstack.Item.ItemClass.GetType() == typeof(ItemPlantableSeed)) return false;
                return itemstack.Item.LastCodePart() == crop;
            };
        }

        public static ActionBoolReturn<ItemStack> IsItemstack(ItemStack stack)
        {
            return (ItemStack itemstack) =>
            {
                return itemstack == stack;
            };
        }

        public static ActionBoolReturn<ItemStack> IsItemClass(Type itemclass)
        {
            return (ItemStack itemstack) =>
            {
                return itemstack.Item != null && itemstack.Item.Class == itemclass.Name;
            };
        }
    }
}