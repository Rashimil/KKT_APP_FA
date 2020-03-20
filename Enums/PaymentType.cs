using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    // Типы оплаты (Наличными / Безналичными / Предоплатой (зачетом аванса) / Постоплатой (в кредит) / Встречным предоставлением ) НЕ НУЖЕН
    public enum PaymentTypeEnum : short
    {
        CASH = 1031, // Наличными (тэг 1031)
        ELECTRONICALLY = 1081, // Электронными (Безналичными) (тэг 1081)
        PREPAID = 1215, // Предоплатой (зачетом аванса) (тэг 1215)
        CREDIT = 1216, // Постоплатой (в кредит) (тэг 1216)
        OTHER = 1217// Встречным предоставлением (тэг 1217)
    }
}
