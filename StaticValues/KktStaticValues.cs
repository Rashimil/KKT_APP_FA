using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.StaticValues
{
    // Статичные значения ответа от ККТ. Считываются их ККТ один раз при старте приложения
    public static class KktStaticValues
    {
        public static string FN { get; set; } // Номер ФН
        public static string KKTFactoryNumber { get; set; } // Заводской номер ККТ
        public static string KKTRegistrationNumber { get; set; } // РН ККТ. Дополняется пробелами справа до длины 20 символов
        public static string INN { get; set; } // ИНН. Дополняется пробелами справа до длины 12 символов
        public static byte KKTOperatingMode { get; set; } // Режимы работы ККТ. Временно byte. Битовая маска
        public static byte TaxTypes { get; set; } // Режимы налогообложения. Временно byte. Битовая маска
        public static byte AgentType { get; set; } // Признак платежного агента. Временно byte. Битовая маска
        public static string FirmwareVersion { get; set; } // Версия ПО ККТ
    }
}
