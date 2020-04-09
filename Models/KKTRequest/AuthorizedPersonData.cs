using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTRequest
{
    // Данные уполномоченного лица (для коррекций)
    public class AuthorizedPersonData
    {
        public AuthorizedPersonData(string AuthorizedPersonFIO, string AuthorizedPersonINN)
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(AuthorizedPersonFIO))
                AuthorizedPersonFIO = "datacenter"; 
            if (string.IsNullOrEmpty(AuthorizedPersonINN) || AuthorizedPersonINN.Length < 12)
                AuthorizedPersonINN = "            "; // 12 пробелов

            // Конвертация с удобоваримого формата:
            if (AuthorizedPersonFIO.Length > 63)
                AuthorizedPersonFIO = AuthorizedPersonFIO.Substring(0, 63); // обрезка до 63 символов
            if (AuthorizedPersonINN.Length > 12)
                AuthorizedPersonINN = AuthorizedPersonINN.Substring(0, 12); // обрезка до 12 символов

            // Заполнение:
            this.AuthorizedPersonFIO = AuthorizedPersonFIO;
            this.AuthorizedPersonINN = AuthorizedPersonINN;
        }
        public string AuthorizedPersonFIO { get; set; } // ФИО Кассира (1021, Не более 64 символов)
        public string AuthorizedPersonINN { get; set; } // ИНН Кассира (1203, Параметр необязательный, если передается, то строго 12 символов)
    }
}
