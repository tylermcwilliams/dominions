namespace cultus.src.NPC.Utils.LocalTalk.TalkModePresets
{
    public class TalkModeWhisper : TalkModeNormal
    {
        private const float WHISPER_DISTANCE_MODIFIER = 0.5f;
        public new float AudiableDistance => base.AudiableDistance * WHISPER_DISTANCE_MODIFIER;
        public override string ApplyTalkModeEffectToMessage(string message)
        {
            return message.ToLower();
        }
    }
}
