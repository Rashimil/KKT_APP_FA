using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Extensions;

namespace KKT_APP_FA.Models.KKTRequest
{
    public class CheckItem
    {
        public CheckItem(
            string Name,
            decimal Price,
            double Count,
            VatEnum Vat,
            PaymentMethodEnum PaymentMethod,
            PaymentObjectEnum PaymentObject,
            string NomenclatureCode = "",
            string MeasurementUnit = "",
            decimal Excise = 0,
            string CustomsDeclarationNumber = "",
            string CountryCode = "",
            string CustomReq = "",
            AgentEnum agentType = AgentEnum.None,
            API.Agent_info agent_info = null // Атрибуты агента. Обрабатывать только если agentType != None
            )
        {
            // Тут надо заполнить Агентские параметры на основании agentType, и вроде всё...

            // Корректировка null - значений:
            if (string.IsNullOrEmpty(Name))
                Name = "Позиция чека";
            if (string.IsNullOrEmpty(NomenclatureCode))
                NomenclatureCode = "";
            if (string.IsNullOrEmpty(MeasurementUnit))
                MeasurementUnit = "";
            if (string.IsNullOrEmpty(CustomsDeclarationNumber))
                CustomsDeclarationNumber = "";
            if (string.IsNullOrEmpty(CountryCode))
                CountryCode = "";
            if (string.IsNullOrEmpty(CustomReq))
                CustomReq = "";

            // Конвертация с удобоваримого формата:
            string name = Name;
            if (name.Length > 128)
                name = name.Substring(0, 128); // обрезка до 128 символов
            uint price = (uint)Math.Truncate(Price * 100); // в копейках

            var correct_count = (uint)Math.Truncate(Count * 1000); // округлили до 3-х знаков после запятой + взяли в мин. единицах
            var correct_count_arr = BitConverter.GetBytes(correct_count).XReverse();
            List<byte> count_list = new List<byte>();
            count_list.Add(0x03);
            count_list.AddRange(correct_count_arr);
            byte[] count = count_list.ToArray(); // структура FVLN, нулевой байт - полжение десятичной точки СПРАВА (в hex само собой).  Все остальные байты число в hex в LE

            byte vat = (byte)Vat;
            byte paymentMethod = (byte)PaymentMethod;
            byte paymentObject = (byte)PaymentObject;

            string nomenclatureCode = NomenclatureCode; // "05AB1208";
            string measurementUnit = MeasurementUnit; // "";
            uint excise = (uint)Math.Truncate(Excise * 100); // в копейках
            string customsDeclarationNumber = CustomsDeclarationNumber; // "";
            string countryCode = CountryCode; // "";
            string customReq = CustomReq; //  "";

            // Заполнение:
            this.Name = new KKTRequestProperty<string>() { TAG = 1030, USER_VALUE = name };
            this.Price = new KKTRequestProperty<uint>() { TAG = 1079, USER_VALUE = price };
            this.Quantity = new KKTRequestProperty<byte[]>() { TAG = 1023, USER_VALUE = count };
            this.Vat = new KKTRequestProperty<byte>() { TAG = 1199, USER_VALUE = vat };
            this.PaymentMethod = new KKTRequestProperty<byte>() { TAG = 1214, USER_VALUE = paymentMethod };
            this.PaymentObject = new KKTRequestProperty<byte>() { TAG = 1212, USER_VALUE = paymentObject };

            this.NomenclatureCode = new KKTRequestProperty<string>() { TAG = 1162, USER_VALUE = nomenclatureCode };
            this.MeasurementUnit = new KKTRequestProperty<string>() { TAG = 1197, USER_VALUE = measurementUnit };
            this.Excise = new KKTRequestProperty<uint>() { TAG = 1229, USER_VALUE = excise };
            this.CustomsDeclarationNumber = new KKTRequestProperty<string>() { TAG = 1231, USER_VALUE = customsDeclarationNumber };
            this.CountryCode = new KKTRequestProperty<string>() { TAG = 1230, USER_VALUE = countryCode };
            this.CustomReq = new KKTRequestProperty<string>() { TAG = 1191, USER_VALUE = customReq };
        }

        public KKTRequestProperty<string> Name { get; set; } // Наименование предмета расчета ()
        public KKTRequestProperty<uint> Price { get; set; } // Цена за ед. предмета расчета (с учетом скидок и наценок) (тег 1079)
        public KKTRequestProperty<byte[]> Quantity { get; set; } // Количество предмета расчета (временно ushort)
        public KKTRequestProperty<byte> Vat { get; set; } // Ставка НДС
        public KKTRequestProperty<byte> PaymentMethod { get; set; } // Признак СПОСОБА расчета
        public KKTRequestProperty<byte> PaymentObject { get; set; } /* Признак ПРЕДМЕТА расчета (необязательный параметр) 
        если 15 или 16, то значение тэга 1030 (НАИМЕНОВАНИЕ предмета расчета) должно принимать одно из значений, приведенных в приложении 7 */

        // Далее не совпадает с порядком в доке, но совпадает с успешным примером:
        public KKTRequestProperty<string> NomenclatureCode { get; set; } /* Код товарной номенклатуры (тег 1162) 
        Массив байт в виде строкового представления, например {0x05, 0xAB, 0x12, 0x08} надо передавать как строку "05AB1208"
        За формирование КТН полностью отвечает ПО верхнего уровня. ККТ в данном случае ретранслирует значения, переданные через команду в тэг 1162 фискального документа */
        public KKTRequestProperty<string> MeasurementUnit { get; set; } // Единица измерения предмета расчета (тег 1197)
        public KKTRequestProperty<uint> Excise { get; set; } // Акциз (тег 1229)
        public KKTRequestProperty<string> CustomsDeclarationNumber { get; set; } // Номер таможенной декларации (тег 1231) 
        public KKTRequestProperty<string> CountryCode { get; set; } // Код страны (тег 1230)
        public KKTRequestProperty<string> CustomReq { get; set; } // Доп. реквизит предмета расчета (тег 1191)

        // Агентские параметры:

        public KKTRequestProperty<byte> AgentInfo { get; set; } // Признак агента по предмету расчета (Тэг 1222)
        public KKTRequestProperty<string> AgentOperation { get; set; }  // Операция платежного агента (Тэг 1044)
        public KKTRequestProperty<string> AgentPhone { get; set; } // Телефон платежного агента (Тэг 1073)
        public KKTRequestProperty<string> PaymentOperatorPhone { get; set; } // Телефон оператора по приему платежей (Тэг 1074)
        public KKTRequestProperty<string> PaymentOperatorName { get; set; }  // Наименование оператора перевода (Тэг 1026)
        public KKTRequestProperty<string> PaymentOperatorAdress { get; set; } // Адрес оператора перевода (Тэг 1005)
        public KKTRequestProperty<string> PaymentOperatorINN { get; set; }  // ИНН оператора перевода (Тэг 1016)
        public KKTRequestProperty<string> SupplierPhone { get; set; } // Телефон поставщика (Тэг 1171)
        public KKTRequestProperty<string> SupplierName { get; set; }  // Наименование поставщика (Тэг 1225)
        public KKTRequestProperty<string> SupplierINN { get; set; } // ИНН поставщика (Тэг 1226)
    }

}
