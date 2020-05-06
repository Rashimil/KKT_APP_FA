using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    public enum KKTOperatingModeEnum : byte
    {
        [Description("Шифрование")]
        EncryptionMode = 0x01,

        [Description("Автономный режим")]
        OfflineMode = 0x02,

        [Description("Автоматический режим")]
        AuthomaticMode = 0x04,

        [Description("Применение в сфере услуг")]
        ServicesSectorMode = 0x08,

        [Description("Режим БСО")]
        BSOMode = 0x10, // Если бит установлен, Режим БСО, иначе Режим кассовых чеков

        [Description("ККТ для интернет")]
        InternetMode = 0x20,
    }
}
