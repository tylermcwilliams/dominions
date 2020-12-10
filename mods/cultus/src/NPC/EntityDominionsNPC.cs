using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace cultus
{
    public class EntityDominionsNPC : EntityHumanoid
    {
        //public override double EyeHeight => base.Properties.EyeHeight - (controls.Sneak ? 0.1 : 0.0);

        protected InventoryBase inv;

        public override bool StoreWithChunk
        {
            get { return true; }
        }

        public override IInventory GearInventory
        {
            get
            {
                return inv;
            }
        }

        public override ItemSlot RightHandItemSlot
        {
            get
            {
                return inv[15];
            }
        }

        public EntityDominionsNPC() : base()
        {
            inv = new InventoryNPC(null, null);
        }

        public override void Initialize(EntityProperties properties, ICoreAPI api, long chunkindex3d)
        {
            base.Initialize(properties, api, chunkindex3d);

            inv.LateInitialize("gearinv-" + EntityId, api);
        }

        public override void OnEntitySpawn()
        {
            base.OnEntitySpawn();

            if (World.Side == EnumAppSide.Client)
            {
                (Properties.Client.Renderer as EntityShapeRenderer).DoRenderHeldItem = true;
            }
        }

        /*public override void SetName(string playername)
        {
            base.SetName(playername);
            this.Name = playername;
        }*/

        public override void ToBytes(BinaryWriter writer, bool forClient)
        {
            TreeAttribute tree;
            WatchedAttributes["gearInv"] = tree = new TreeAttribute();
            inv.ToTreeAttributes(tree);

            base.ToBytes(writer, forClient);
        }

        public override void FromBytes(BinaryReader reader, bool forClient)
        {
            base.FromBytes(reader, forClient);

            TreeAttribute tree = WatchedAttributes["gearInv"] as TreeAttribute;
            if (tree != null) inv.FromTreeAttributes(tree);
        }

        public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
        {
            base.OnInteract(byEntity, slot, hitPosition, mode);

            if ((byEntity as EntityPlayer)?.Controls.Sneak == true && mode == EnumInteractMode.Interact && byEntity.World.Side == EnumAppSide.Server)
            {
                inv.DiscardAll();
                WatchedAttributes.MarkAllDirty();
            }
        }

        public void ConsumeItems(ItemSlot slot, int amount = 1)
        {
            slot.TakeOut(amount);
            WatchedAttributes.MarkAllDirty();
        }
    }
}