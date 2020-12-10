using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace cultus
{
    static partial class Conditions
    {
        public static bool HasDuty(IDuty duty, Type T)
        {
            return (duty != null && duty.GetType() == T);
        }
    }
}