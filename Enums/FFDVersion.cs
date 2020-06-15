using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    public enum FFDVersionEnum : byte
    {
        [Description("1.0")]
        Zero = 0,

        [Description("1.0")]
        One = 1,

        [Description("1.05")]
        Two = 2,

        [Description("1.1")]
        Three = 3,

        [Description("1.1")]
        Four = 4,
    }
}
