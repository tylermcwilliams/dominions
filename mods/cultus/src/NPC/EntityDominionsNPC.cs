using cultus.src.NPC.Utils.LocalTalk.TalkModePresets;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace cultus
{
    public class EntityDominionsNPC : EntityHumanoid
    {


        private ILocalTalk localTalk;

        private InventoryBase inventory;
        public INPCJob Job { get; set; }

        public override bool StoreWithChunk
        {
            get => true;
        }

        public override IInventory GearInventory
        {
            get => inventory;
        }

        public override ItemSlot RightHandItemSlot
        {
            get => inventory[15];
        }

        private EntityBehaviorNameTag behaviorNameTag;

        public override string GetName()
        {
            return behaviorNameTag?.DisplayName ?? base.GetName();
        }

        public EntityDominionsNPC() : base()
        {
            inventory = new InventoryNPC(null, null);
        }

        public override void Initialize(EntityProperties properties, ICoreAPI api, long chunkindex3d)
        {
            base.Initialize(properties, api, chunkindex3d);
            behaviorNameTag = GetBehavior<EntityBehaviorNameTag>();

            inventory.LateInitialize("gearinv-" + EntityId, api);

            if (api.Side.IsServer())
            {
                localTalk = new LocalTalk(api as ICoreServerAPI, this);
            }
        }

        public override void OnEntitySpawn()
        {
            base.OnEntitySpawn();

            if (World.Side == EnumAppSide.Client)
            {
                (Properties.Client.Renderer as EntityShapeRenderer).DoRenderHeldItem = true;
            }
        }

        public override void ToBytes(BinaryWriter writer, bool forClient)
        {
            TreeAttribute tree;
            WatchedAttributes["gearInv"] = tree = new TreeAttribute();
            inventory.ToTreeAttributes(tree);

            base.ToBytes(writer, forClient);
        }

        public override void FromBytes(BinaryReader reader, bool forClient)
        {
            base.FromBytes(reader, forClient);

            TreeAttribute tree = WatchedAttributes["gearInv"] as TreeAttribute;
            if (tree != null) inventory.FromTreeAttributes(tree);
        }

        public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
        {
            base.OnInteract(byEntity, slot, hitPosition, mode);

            if (byEntity.Controls.Sneak
                && mode == EnumInteractMode.Interact
                && byEntity.World.Side == EnumAppSide.Server)
            {
                inventory.DropAll(Pos.AsBlockPos.ToVec3d());
                WatchedAttributes.MarkAllDirty();
            }
        }

        public void ConsumeItems(ItemSlot slot, int amount = 1)
        {
            slot.TakeOut(amount);
            WatchedAttributes.MarkAllDirty();
        }

        public void Say(string message, ILocalTalkMode talkMode)
        {
            localTalk.Say(talkMode.ApplyTalkModeEffectToMessage(message), talkMode.AudiableDistance);
        }
    }
}