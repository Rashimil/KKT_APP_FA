using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTRequest
{
    // Данные клиента
    public class ClientData
    {
        public ClientData(string ClientEmail, string ClientName = "", string ClientINN = "")
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(ClientEmail))
                ClientEmail = "portal@brsc.ru"; 
            if (string.IsNullOrEmpty(ClientName))
                ClientName = "";
            if (string.IsNullOrEmpty(ClientINN))
                ClientINN = "";

            // Конвертация с удобоваримого формата:
            if (ClientEmail.Length > 64)
                ClientEmail = ClientEmail.Substring(0, 64); // обрезка до 64 символов
            if (ClientName.Length > 255)
                ClientName = ClientName.Substring(0, 255); // обрезка до 255 символов
            if (ClientINN.Length > 12)
                ClientINN = ClientINN.Substring(0, 12); // обрезка до 12 символов
            if (ClientINN.Length < 12)
                ClientINN = ClientINN.PadLeft(12, ' '); // добивка до 12 символов

            // Заполнение:
            this.ClientEmail = ClientEmail;
            this.ClientName = ClientName;
            this.ClientINN = ClientINN;
        }
        public string ClientEmail { get; set; } // максимальная длина - 64 символа.
        public string ClientName { get; set; } // Имя (наименование) клиента. Не более 256 символов (необязательный)
        public string ClientINN { get; set; } // ИНН клиента (1228, Параметр необязательный, Не более 12 символов)
    }
}
