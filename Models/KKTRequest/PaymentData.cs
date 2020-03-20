using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;

namespace KKT_APP_FA.Models.KKTRequest
{
    // Данные оплаты (для команды 0x2D)

    /*
     Режим налогообложения (TaxType) может включать только одно из значений режимов, указанных при регистрации
    (перерегистрации) ККТ (См. команду 0x0A - Запрос текущих параметров регистрации ККТ)
    Тэг 1203 (ИНН Кассира) указывается при наличии ИНН у кассира
    Тэги 1021, 1203 в автоматическом режиме могут не указываться
     */
    public class PaymentData
    {
        public PaymentData(
            TaxTypeEnum TaxType,
            decimal CASH,
            decimal ELECTRONICALLY,
            decimal PREPAID,
            decimal CREDIT,
            decimal OTHER,
            CashierData cashierData,
            ClientData clientData,
            string CustomReq = "",
            string CustomUserReqName = "",
            string CustomUserReqValue = ""
            )
        {
            // Корректировка null - значений:
            if (string.IsNullOrEmpty(CustomReq))
                CustomReq = "";
            if (string.IsNullOrEmpty(CustomUserReqName))
                CustomUserReqName = "";
            if (string.IsNullOrEmpty(CustomUserReqValue))
                CustomUserReqValue = "";

            // Конвертация с удобоваримого формата:
            byte taxType = (byte)TaxType;
            uint cash = (uint)Math.Truncate(CASH * 100); // в копейках
            uint electronnically = (uint)Math.Truncate(ELECTRONICALLY * 100); // в копейках
            uint prepaid = (uint)Math.Truncate(PREPAID * 100); // в копейках
            uint credit = (uint)Math.Truncate(CREDIT * 100); // в копейках
            uint other = (uint)Math.Truncate(OTHER * 100); // в копейках

            string clientEmail = clientData.ClientEmail;
            if (clientEmail.Length > 64)
                clientEmail = clientEmail.Substring(0, 64); // обрезка 
            string clientINN = clientData.ClientINN;
            if (clientINN.Length > 12)
                clientINN = clientINN.Substring(0, 12); // обрезка 
            string cashierFIO = cashierData.CashierFIO;
            if (cashierFIO.Length > 64)
                cashierFIO = cashierFIO.Substring(0, 64); // обрезка 
            string cashierINN = cashierData.CashierINN;
            if (cashierINN.Length > 12)
                cashierINN = cashierINN.Substring(0, 12); // обрезка 
            if (cashierINN.Length < 12)
                cashierINN = cashierINN.PadLeft(12, ' '); // добивка пробелами до 12 символов 
            string сlientName = clientData.ClientName;
            if (сlientName.Length > 255)
                сlientName = сlientName.Substring(0, 255); // обрезка 
            string сustomReq = CustomReq;
            if (сustomReq.Length > 16)
                сustomReq = сustomReq.Substring(0, 16); // обрезка 
            string сustomUserReqName = CustomUserReqName;
            if (сustomUserReqName.Length > 64)
                сustomUserReqName = сustomUserReqName.Substring(0, 64); // обрезка 
            string сustomUserReqValue = CustomUserReqValue;
            if (сustomUserReqValue.Length > 255)
                сustomUserReqValue = сustomUserReqValue.Substring(0, 255); // обрезка 

            // Заполнение:
            this.TaxType = new KKTRequestProperty<byte>() { TAG = 1055, USER_VALUE = taxType };
            this.CASH = new KKTRequestProperty<uint>() { TAG = 1031, USER_VALUE = cash };
            this.ELECTRONICALLY = new KKTRequestProperty<uint>() { TAG = 1081, USER_VALUE = electronnically };
            this.PREPAID = new KKTRequestProperty<uint>() { TAG = 1215, USER_VALUE = prepaid };
            this.CREDIT = new KKTRequestProperty<uint>() { TAG = 1216, USER_VALUE = credit };
            this.OTHER = new KKTRequestProperty<uint>() { TAG = 1217, USER_VALUE = other };
            this.ClientEmail = new KKTRequestProperty<string>() { TAG = 1008, USER_VALUE = clientEmail };
            this.ClientINN = new KKTRequestProperty<string>() { TAG = 1228, USER_VALUE = clientINN };
            this.ClientName = new KKTRequestProperty<string>() { TAG = 1227, USER_VALUE = сlientName };
            this.CashierFIO = new KKTRequestProperty<string>() { TAG = 1021, USER_VALUE = cashierFIO };
            this.CashierINN = new KKTRequestProperty<string>() { TAG = 1203, USER_VALUE = cashierINN };
            this.CustomReq = new KKTRequestProperty<string>() { TAG = 1192, USER_VALUE = сustomReq };
            this.CustomUserReqName = new KKTRequestProperty<string>() { TAG = 1085, USER_VALUE = сustomUserReqName };
            this.CustomUserReqValue = new KKTRequestProperty<string>() { TAG = 1086, USER_VALUE = сustomUserReqValue };
        }

        public KKTRequestProperty<byte> TaxType { get; set; } // Режим налогообложения (тэг 1055) (ТОЛЬКО ОДНО из значений режимов, указанных при регистрации ККТ)
        public KKTRequestProperty<uint> CASH { get; set; } // Наличными (в копейках)
        public KKTRequestProperty<uint> ELECTRONICALLY { get; set; } // Безналичными (в копейках)
        public KKTRequestProperty<uint> PREPAID { get; set; } // Предоплатой (зачетом аванса) (в копейках)
        public KKTRequestProperty<uint> CREDIT { get; set; } // Постоплатой (в кредит) (в копейках)
        public KKTRequestProperty<uint> OTHER { get; set; } // Встречным предоставлением (в копейках)
        public KKTRequestProperty<string> ClientEmail { get; set; } // Адрес электронной почты клиента (тэг 1008) (максимум 64 символа)
        public KKTRequestProperty<string> ClientINN { get; set; } // ИНН покупателя (тэг 1228) Не более 12 символов (необязательный)
        public KKTRequestProperty<string> ClientName { get; set; } // Имя покупателя (тэг 1227) Не более 255 символов (необязательный)
        public KKTRequestProperty<string> CashierFIO { get; set; } // ФИО кассира (тэг 1021) (в автоматическом режиме может не указываться) (Не более 64 символов)
        public KKTRequestProperty<string> CashierINN { get; set; } // ИНН кассира (тэг 1203) (указывается при наличии ИНН у кассира) (в автоматическом режиме может не указываться) (Строго 12 символов)
        public KKTRequestProperty<string> CustomReq { get; set; } // Доп реквизит (тэг 1192) Не более 16 символов (необязательный)
        public KKTRequestProperty<string> CustomUserReqName { get; set; } // Наименование доп реквизита пользователя (тэг 1085) Не более 64 символов (необязательный)
        public KKTRequestProperty<string> CustomUserReqValue { get; set; } // Значение доп реквизита пользователя (тэг 1086) Не более 256 символов (необязательный) 
    }
}