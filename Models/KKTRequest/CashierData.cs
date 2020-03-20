using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTRequest
{
    // Данные кассира
    public class CashierData
    {
        public CashierData(string CashierFIO, string CashierINN)
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(CashierFIO))
                CashierFIO = "datacenter"; 
            if (string.IsNullOrEmpty(CashierINN) || CashierINN.Length < 12)
                CashierINN = "            "; // 12 пробелов

            // Конвертация с удобоваримого формата:
            if (CashierFIO.Length > 64)
                CashierFIO = CashierFIO.Substring(0, 64); // обрезка до 64 символов
            if (CashierINN.Length > 12)
                CashierINN = CashierINN.Substring(0, 12); // обрезка до 12 символов

            // Заполнение:
            this.CashierFIO = CashierFIO;
            this.CashierINN = CashierINN;
        }
        public string CashierFIO { get; set; } // ФИО Кассира (1021, Не более 64 символов)
        public string CashierINN { get; set; } // ИНН Кассира (1203, Параметр необязательный, если передается, то строго 12 символов)
    }
}
