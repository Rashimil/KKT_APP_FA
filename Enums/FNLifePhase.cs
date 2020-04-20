using System.ComponentModel;

namespace KKT_APP_FA.Enums
{
    // Фазы жизни ФН
    public enum FNLifePhaseEnum : byte
    {
        [Description("Готов к фискализации")]
        ReadyToFiscalisation = 0x01,

        [Description("Открыт фискальный режим")]
        FiscalMode = 0x03,

        [Description("Постфискальный режим, идет передача ФД в ОФД")]
        PostFiscalMode = 0x07,

        [Description("Чтение данных из архива/Закончена передача ФД в ОФД")]
        ReadingArchiveData = 0x0F
    }
}
