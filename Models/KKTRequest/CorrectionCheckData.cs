using KKT_APP_FA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KKT_APP_FA.Helpers.LogicLevel;

namespace KKT_APP_FA.Models.KKTRequest
{
    public class CorrectionCheckData
    {
        public CorrectionCheckData(
            AuthorizedPersonData authorizedPersonData,
            byte CorrectionType, // 0 – самостоятельно, 1 – по предписанию
            TaxTypeEnum TaxType,
            decimal CASH,
            decimal ELECTRONICALLY,
            decimal PREPAID,
            decimal CREDIT,
            decimal OTHER,
            decimal Vat20SUM,
            decimal Vat10SUM,
            decimal Vat0SUM,
            decimal VatNoneSUM,
            decimal Vat20120SUM,
            decimal Vat10110SUM,
            byte[] BaseCorrectionData
            )
        {
            // Корректировка null - значений:
            // пока неясно что тут

            // Конвертация с удобоваримого формата:
            string authorizedPersonFIO = authorizedPersonData.AuthorizedPersonFIO;
            string authorizedPersonINN = authorizedPersonData.AuthorizedPersonINN;
            if (CorrectionType > 1) CorrectionType = 0; // по умолчанию - самостоятельно
            byte taxType = (byte)TaxType;
            uint cash = (uint)Math.Truncate(CASH * 100); // в копейках
            uint electronnically = (uint)Math.Truncate(ELECTRONICALLY * 100); // в копейках
            uint prepaid = (uint)Math.Truncate(PREPAID * 100); // в копейках
            uint credit = (uint)Math.Truncate(CREDIT * 100); // в копейках
            uint other = (uint)Math.Truncate(OTHER * 100); // в копейках
            uint vat20SUM = (uint)Math.Truncate(Vat20SUM * 100); // в копейках
            uint vat10SUM = (uint)Math.Truncate(Vat10SUM * 100); // в копейках
            uint vat0SUM = (uint)Math.Truncate(Vat0SUM * 100); // в копейках
            uint vatNoneSUM = (uint)Math.Truncate(VatNoneSUM * 100); // в копейках
            uint vat20120SUM = (uint)Math.Truncate(Vat20120SUM * 100); // в копейках
            uint vat10110SUM = (uint)Math.Truncate(Vat10110SUM * 100); // в копейках
            byte[] baseCorrectionData = BaseCorrectionData;

            // Заполнение:
            this.AuthorizedPersonFIO = new KKTRequestProperty<string>() { TAG = 1021, USER_VALUE = authorizedPersonFIO };
            this.AuthorizedPersonINN = new KKTRequestProperty<string>() { TAG = 1203, USER_VALUE = authorizedPersonINN };
            this.CorrectionType = new KKTRequestProperty<byte>() { TAG = 1173, USER_VALUE = CorrectionType };
            this.TaxType = new KKTRequestProperty<byte>() { TAG = 1055, USER_VALUE = taxType };
            this.CASH = new KKTRequestProperty<uint>() { TAG = 1031, USER_VALUE = cash };
            this.ELECTRONICALLY = new KKTRequestProperty<uint>() { TAG = 1081, USER_VALUE = electronnically };
            this.PREPAID = new KKTRequestProperty<uint>() { TAG = 1215, USER_VALUE = prepaid };
            this.CREDIT = new KKTRequestProperty<uint>() { TAG = 1216, USER_VALUE = credit };
            this.OTHER = new KKTRequestProperty<uint>() { TAG = 1217, USER_VALUE = other };
            this.Vat20SUM = new KKTRequestProperty<uint>() { TAG = 1102, USER_VALUE = vat20SUM };
            this.Vat10SUM = new KKTRequestProperty<uint>() { TAG = 1103, USER_VALUE = vat10SUM };
            this.Vat0SUM = new KKTRequestProperty<uint>() { TAG = 1104, USER_VALUE = vat0SUM };
            this.VatNoneSUM = new KKTRequestProperty<uint>() { TAG = 1105, USER_VALUE = vatNoneSUM };
            this.Vat20120SUM = new KKTRequestProperty<uint>() { TAG = 1106, USER_VALUE = vat20120SUM };
            this.Vat10110SUM = new KKTRequestProperty<uint>() { TAG = 1107, USER_VALUE = vat10110SUM };
            this.BaseCorrectionData = new KKTRequestProperty<byte[]>() { TAG = 1174, USER_VALUE = baseCorrectionData.Skip(4).ToArray() };
        }
        public KKTRequestProperty<string> AuthorizedPersonFIO { get; set; } // ФИО уполномоченного лица. 1021. Не более 63 символов
        public KKTRequestProperty<string> AuthorizedPersonINN { get; set; } // ИНН уполномоченного лица. 1203. 12 символов
        public KKTRequestProperty<byte> CorrectionType { get; set; } // Тип коррекции. 1173. 0 – самостоятельно, 1 – по предписанию
        public KKTRequestProperty<byte> TaxType { get; set; } //  система налогообложения. 1055. (TaxTypeEnum)
        public KKTRequestProperty<uint> CASH { get; set; } // Наличными (в копейках) 1031
        public KKTRequestProperty<uint> ELECTRONICALLY { get; set; } // Безналичными (в копейках) 1081
        public KKTRequestProperty<uint> PREPAID { get; set; } // Предоплатой (зачетом аванса) (в копейках) 1215
        public KKTRequestProperty<uint> CREDIT { get; set; } // Постоплатой (в кредит) (в копейках) 1216
        public KKTRequestProperty<uint> OTHER { get; set; } // Встречным предоставлением (в копейках) 1217
        public KKTRequestProperty<uint> Vat20SUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке 20% (в копейках) 1102
        public KKTRequestProperty<uint> Vat10SUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке 10% (в копейках) 1103
        public KKTRequestProperty<uint> Vat0SUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке 0% (в копейках) 1104
        public KKTRequestProperty<uint> VatNoneSUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке без НДС (в копейках) 1105
        public KKTRequestProperty<uint> Vat20120SUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке 20/120% (в копейках) 1106
        public KKTRequestProperty<uint> Vat10110SUM { get; set; } // Сумма по чеку, от которой считается НДС по ставке 10/110% (в копейках) 1107
        public KKTRequestProperty<byte[]> BaseCorrectionData { get; set; } // STLV включает 3 TLV Объекта. 1174. Наименование, Дата, Номер документа - основания для коррекции

    }
}
