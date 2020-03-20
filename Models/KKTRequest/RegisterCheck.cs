using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;

namespace KKT_APP_FA.Models.KKTRequest
{
    // обьект для команды Сформировать чек (0x24)
    public class RegisterCheck
    {
        public RegisterCheck(OperationEnum operation, decimal TotalSum, string AddInfoForPrint = "")
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(AddInfoForPrint))
                AddInfoForPrint = "";

            // Конвертация с удобоваримого формата:
            if (AddInfoForPrint.Length > 511)
                AddInfoForPrint = AddInfoForPrint.Substring(0, 511); // обрезка 
            uint sum = (uint)Math.Truncate(TotalSum * 100); // в копейках
            byte[] sum4 = BitConverter.GetBytes(sum);
            var sum5 = new List<byte>();
            sum5.AddRange(sum4);
            sum5.Add(0x00);

            // Заполнение:
            this.AddInfoForPrint = AddInfoForPrint;
            this.Operation = (byte)operation;
            this.TotalSum = sum5.ToArray();
        }

        public byte Operation { get; set; } //Признак расчета
        public byte[] TotalSum { get; set; } // Итог чека в копейках, упакованный в 5 байт, формат LE
        public string AddInfoForPrint { get; set; } // Дополнительная информация для печати на чеке. MAX длина строки 512 символов
    }
}
