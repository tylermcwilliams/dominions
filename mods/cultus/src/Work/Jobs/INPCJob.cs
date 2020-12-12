namespace cultus
{
    public interface INPCJob
    {
        void TryGetDuty(EntityDominionsNPC npc, ref IDuty TryGetDuty);
    }
}