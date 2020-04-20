using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    public enum AgentEnum : byte
    {
        [Description("Отсутствует")]
        None = 0x00,

        [Description("Банковский платежный агент")]
        BankPayAgent = 0x01,

        [Description("Банковский платежный субагент")]
        BankPaySubAgent = 0x02,

        [Description("Платежный агент")]
        PayAgent = 0x04,

        [Description("Платежный субагент")]
        PaySubAgent = 0x08,

        [Description("Поверенный")]
        Attorney = 0x10,

        [Description("Комиссионер")]
        ComissionAgent = 0x20,

        [Description("Прочее")]
        Another = 0x40,
    }
}
