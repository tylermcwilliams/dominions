namespace cultus
{
    internal interface INPCJob
    {
        void TryGetDuty(EntityDominionsNPC npc, ref IDuty TryGetDuty);
    }
}