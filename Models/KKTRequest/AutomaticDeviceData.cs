using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTRequest
{
    // данные автоматического устройства расчетов для кассового чека (0x1F)
    public class AutomaticDeviceData
    {
        public AutomaticDeviceData(string Adress, string Place, string AutomatNumber)
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(Adress))
                Adress = " "; // минимум пробел
            if (string.IsNullOrEmpty(Place))
                Place = " "; // минимум пробел
            if (string.IsNullOrEmpty(AutomatNumber))
                AutomatNumber = " "; // минимум пробел

            // Конвертация с удобоваримого формата:
            if (Adress.Length > 128)
                Adress = Adress.Substring(0, 128); // обрезка до 128 символов
            if (Place.Length > 128)
                Place = Place.Substring(0, 128); // обрезка до 128 символов
            if (AutomatNumber.Length > 32)
                AutomatNumber = AutomatNumber.Substring(0, 32); // обрезка до 32 символов

            // Заполнение:
            this.Adress = new KKTRequestProperty<string>() { TAG = 1009, USER_VALUE = Adress };
            this.Place = new KKTRequestProperty<string>() { TAG = 1187, USER_VALUE = Place };
            this.AutomatNumber = new KKTRequestProperty<string>() { TAG = 1036, USER_VALUE = AutomatNumber };
        }
        public KKTRequestProperty<string> Adress { get; set; } // Адрес расчетов (1009)
        public KKTRequestProperty<string> Place { get; set; } // Место расчетов (1187)
        public KKTRequestProperty<string> AutomatNumber { get; set; } // Номер автомата (1036)
    }
}
