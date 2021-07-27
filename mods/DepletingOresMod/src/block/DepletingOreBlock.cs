using depletingores.src.blockEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace depletingores.src.block
{
    public class DepletingOreBlock : BlockOre
    {
        private DepletingOreEntity GetBlockEntity(IWorldAccessor world, BlockPos pos)
        {
            DepletingOreEntity entity = (DepletingOreEntity)world.BlockAccessor.GetBlockEntity(pos);
            if (entity == null)
            {
                //A DepletingOreBlock should always have an DepletingOreEntity, therefore we create a new one.
                world.BlockAccessor.SpawnBlockEntity(ModContext.ClassNames.DepletingOreEntity, pos);
                entity = (DepletingOreEntity)world.BlockAccessor.GetBlockEntity(pos);
            }
            return entity;
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            //If player is in creative mode.
            if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative))
            {
                //Destroy the block.
                world.BlockAccessor.SetBlock(0, pos);
                return;
            }

            world.PlaySoundAt(Sounds.GetBreakSound(byPlayer), pos.X, pos.Y, pos.Z, byPlayer);

            var entity = GetBlockEntity(world, pos);

            //Generate item drops.
            // TODO: Generate drops based on type and depletion progress, instead of actuel ore drops. 
            ItemStack[] drops = GetDrops(world, pos, byPlayer, dropQuantityMultiplier);

            if (drops != null)
            {
                for (int i = 0; i < drops.Length; i++)
                {
                    world.SpawnItemEntity(drops[i], new Vec3d(pos.X + 0.5, pos.Y + 0.5, pos.Z + 0.5), null);
                }
            }

            entity.CurrentQuantity--;

            //Decide if block should break.
            // HACK: Note that any ore with less than 0 in quantity will have infinity yield.
            if (entity.CurrentQuantity == 0)
            {
                //Destroy the block.
                world.BlockAccessor.SetBlock(0, pos);
                return;
            }
        }
    }
}
