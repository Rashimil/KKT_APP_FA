using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    public enum FNFlagsEnum : byte 
    {
        [Description("Предупреждения отстутствуют")]
        None = 0x00,

        [Description("Требуется срочная замена ФН (до окончания срока действия 3 дня)")]
        FNNeedUrgentReplace = 0x01,

        [Description("Приближается окончание срока действия ФН (до окончания срока действия 30 дней)")]
        FNNeedReplace = 0x02,

        [Description("Переполнение памяти ФН (Архив ФН заполнен на 90% или более)")]
        FNMEmoryFull = 0x04,

        [Description("Превышено время ожидания ответа ОФД")]
        OFDTimeOut = 0x08,

        [Description("Критическая ошибка ФН")]
        FNCriticalError = 0x80,
    }
}
