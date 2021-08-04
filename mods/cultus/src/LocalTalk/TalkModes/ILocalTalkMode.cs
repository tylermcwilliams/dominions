namespace cultus.src.NPC.Utils.LocalTalk.TalkModePresets
{
    public interface ILocalTalkMode
    {
        float AudiableDistance { get; }
        string ApplyTalkModeEffectToMessage(string message);
    }
}
