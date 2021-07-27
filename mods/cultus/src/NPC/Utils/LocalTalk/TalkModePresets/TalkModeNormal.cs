using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cultus.src.NPC.Utils.LocalTalk.TalkModePresets
{
    public class TalkModeNormal : ILocalTalkMode
    {
        public float AudiableDistance => 15;
        public virtual string ApplyTalkModeEffectToMessage(string message)
        {
            return message;
        }
    }
}
