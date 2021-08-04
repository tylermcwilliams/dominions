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
        public static bool HasErrand(IErrand errand, Type T)
        {
            return (errand != null && errand.GetType() == T);
        }
    }
}