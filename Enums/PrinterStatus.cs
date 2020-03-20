using System;
using System.ComponentModel;
namespace KKT_APP_FA.Enums
{
    public enum PrinterStatusEnum
    {
        [Description("Принтер исправен, бумага присутствует")]
        OK = 0,

        [Description("Принтер не подключен")]
        NoDevice = 1,

        [Description("Бумага отсутствует")]
        NoPaper = 2,

        [Description("Замятие бумаги")]
        PaperJammed = 3,

        [Description("Крышка принтера открыта")]
        OpenBox = 5,

        [Description("Ошибка отрезчика принтера")]
        CutterError = 6,

        [Description("Аппаратная ошибка принтера")]
        HardwareError = 7
    }
}
