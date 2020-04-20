using KKT_APP_FA.Enums;
using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class GetKktStatusResponse : BaseResponse
    {
        public GetKktStatusResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 22)
            {
                this.KKTFactoryNumber = logicLevel.ConvertFromByteArray.ToString(DATA.Take(12).XReverse().ToArray());
                this.KKTDateTime = logicLevel.ConvertFromByteArray.ToDateTime((DATA.Skip(12).Take(5).XReverse().ToArray())).ToString("dd.MM.yyyy HH:mm:ss");
                this.HasCriticalError = (DATA.Skip(17).Take(1).XReverse().ToArray()[0] == 1) ? true : false;
                this.PrinterStatus = DATA.Skip(18).Take(1).XReverse().ToArray()[0];
                this.FNConnected = (DATA.Skip(19).Take(1).XReverse().ToArray()[0] == 1) ? true : false;
                this.FNLifePhase = DATA.Skip(20).Take(1).XReverse().ToArray()[0];
                FNLifePhaseEnum fnlp = (FNLifePhaseEnum)this.FNLifePhase;
                this.FNLifePhaseDescription = EnumHelper.GetTypeDescription(fnlp);
                this.PrinterModel = DATA.Skip(21).Take(1).XReverse().ToArray()[0];
            }
        }

        public string KKTFactoryNumber { get; set; } // заводской номер ККТ
        public string KKTDateTime { get; set; } // Дат и время ККТ // TODO ToString(yyyyMMddHHmmss)
        public bool HasCriticalError { get; set; } // Критические ошибки в ККТ. false – ошибок нет, true – присутствуют
        public byte PrinterStatus { get; set; } // 0 – Корректный статус, бумага присутствует, 1 – Устройство не подключено, 2 – Отсутствует бумага, 3 – Замятие бумаги, 5 – Открыта крышка ПУ, 6 – Ошибка отрезчика ПУ, 7 – Аппаратная ошибка ПУ
        public bool FNConnected { get; set; } // Наличие ФН в ККТ
        public byte FNLifePhase { get; set; } // Фаза жизни ФН
        public string FNLifePhaseDescription { get; set; } // Фаза жизни ФН (описание)
        public byte PrinterModel { get; set; } // Модель принтера (временно byte)
    }
}
