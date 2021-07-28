namespace cultus.src.NPC.Utils.LocalTalk.TalkModePresets
{
    public class TalkModeShout : TalkModeNormal
    {
        private const float SHOUTING_DISTANCE_MODIFIER = 2;
        public new double AudiableDistance => base.AudiableDistance * SHOUTING_DISTANCE_MODIFIER;
        public override string ApplyTalkModeEffectToMessage(string message)
        {
            return message.ToUpper();
        }

    }
}
