using System.ComponentModel;

namespace KKT_APP_FA.Enums
{
    // Типы налогообложения
    public enum TaxTypeEnum : byte
    {
        [Description("ОСН")]
        Common = 1,

        [Description("УСН доход")]
        Simplified = 2,

        [Description("УСН доход-расход")]
        Simplified2 = 4,

        [Description("ЕНВД")]
        ENVD = 8,

        [Description("ЕСН")]
        ESN = 16,

        [Description("ПСН")]
        Patent = 32,

        Unknown = 128
    }
}
