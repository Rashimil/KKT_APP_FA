using System.Collections.Generic;
using System.ComponentModel;

namespace KKT_APP_FA.Enums
{
    // Ставки НДС (тэг 1199):
    public enum VatEnum : byte
    {
        [Description("НДС 20%")]
        Vat20 = 1,

        [Description("НДС 10%")]
        Vat10 = 2,

        [Description("НДС 20/120")]
        Vat20120 = 3,

        [Description("НДС 10/110")]
        Vat10110 = 4,

        [Description("НДС 0%")]
        Vat0 = 5,

        [Description("НДС не облагается")]
        None = 6,
    }
}