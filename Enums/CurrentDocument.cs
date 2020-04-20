using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Enums
{
    public enum CurrentDocumentEnum : byte
    {
        [Description("Нет открытого документа")]
        None = 0x00,

        [Description("Отчет о регистрации ККТ")]
        KKTRegistrationReport = 0x01,

        [Description("Отчет об открытии смены")]
        OpenShiftReport = 0x02,

        [Description("Кассовый чек")]
        Check = 0x04,

        [Description("Отчет о закрытии смены")]
        CloseShiftReport = 0x08,

        [Description("Отчет о закрытии фискального режима")]
        CloseFiscalModeReport = 0x0A,

        [Description("Отчет об изменении параметров регистрации ККТ в связи с заменой ФН")]
        ChangeKKTParemetersFNReport = 0x0C,

        [Description("Отчет об изменении параметров регистрации ККТ")]
        ChangeKKTParemetersReport = 0x0D,

        [Description("Кассовый чек коррекции")]
        CorrectionCheck = 0x0E,

        [Description("Отчет о текущем состоянии расчетов")]
        CurrentValuationStateReport = 0x11
    }
}
