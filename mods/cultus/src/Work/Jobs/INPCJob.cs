namespace cultus
{
    public interface INPCJob
    {
        bool TryGetErrand(EntityDominionsNPC npc, ref IErrand TryGetErrand);
    }
}