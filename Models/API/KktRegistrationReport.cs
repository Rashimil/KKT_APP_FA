using KKT_APP_FA.Attributes;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API
{
    // Отчет о регистрации ККТ (по всем тэгам)
    public class KktRegistrationReport 
    {
        #region request example
        /*
         B6 29 start
01 7D длина 
00 результат. далее идет список TLV
	11 04 tag = 1041
	10 00 len = 16
		39 39 39 39 30 37 38 39 30 32 30 30 33 39 38 38 
	0D 04 tag = 1037
	14 00 len = 20
		30 30 30 30 30 30 30 30 30 31 30 36 31 38 37 35 20 20 20 20 
	FA 03 tag = 1018
	0C 00 len = 12
		30 32 37 34 31 32 34 37 35 32 20 20 
	10 04 tag = 1040
	04 00 len = 4
		01 00 00 00 
	F4 03 tag = 1012
	04 00 len = 4
		88 9D 4E 5E 
	35 04 tag = 1077
	06 00 len = 6
		21 04 5B 49 1F 91 
	20 04 tag = 1056
	01 00 len = 1
		00 
	EA 03 tag = 1002
	01 00 len = 1
		00 
	E9 03 tag = 1001
	01 00 len = 1
		01 
	55 04 tag = 1109
	01 00 len = 1
		01 
	56 04 tag = 1110
	01 00 len = 1
		00 
	54 04 tag = 1108
	01 00 len = 1
		00 
	26 04 tag = 1062
	01 00 len = 1
		01 
	18 04 tag = 1048
	25 00 len = 37
		80 8E 20 81 A0 E8 AA A8 E0 E1 AA A8 A9 20 E0 A5 A3 A8 E1 E2 E0 20 E1 AE E6 A8 A0 AB EC AD EB E5 20 AA A0 E0 E2 
	F1 03 tag = 1009
	17 00 tag = 23
		A3 2E 20 93 E4 A0 2C 20 E3 AB 2E 20 8A E0 E3 AF E1 AA AE A9 2C 20 39 
	A3 04 tag = 1187
	16 00 len = 22
		96 A5 AD E2 E0 20 AE A1 E0 A0 A1 AE E2 AA A8 20 A4 A0 AD AD EB E5 
	FD 03 tag = 1021
	16 00 len = 22
		96 A5 AD E2 E0 20 AE A1 E0 A0 A1 AE E2 AA A8 20 A4 A0 AD AD EB E5 
	C5 04 tag = 1221
	01 00 len = 1
		01 
	F9 03 tag = 1017
	0C 00 len = 12
		37 37 30 39 33 36 34 33 34 36 20 20 
	16 04 tag = 1046
	2A 00 len = 42
		80 8E 20 22 9D AD A5 E0 A3 A5 E2 A8 E7 A5 E1 AA A8 A5 20 E1 A8 E1 E2 A5 AC EB 20 A8 20 AA AE AC AC E3 AD A8 AA A0 E6 A8 A8 22 
	5D 04 tag = 1117
	0E 00 len = 14
		70 6F 72 74 61 6C 40 62 72 73 63 2E 72 75 
	24 04 tag = 1060
	0C 00 len = 12
		77 77 77 2E 6E 61 6C 6F 67 2E 72 75 
	B9 04 tag = 1209
	01 00 len = 1
		02 
	A5 04 tag = 1189
	01 00 len = 1
		02 
	A4 04 tag = 1188
	08 00 len = 8
		46 41 20 30 31 2E 30 35 
	F5 03 tag = 1013
	0C 00 len = 12
		35 35 30 31 30 31 30 30 38 39 31 31 
B6 11 
        */
        #endregion
        byte[] TLVs;
        public DateTimeHelper dateTimeHelper = new DateTimeHelper(Startup.ConfigStatic); 
        public KktRegistrationReport(LogicLevel logicLevel)
        {
            if (logicLevel.response.DATA.Length > 12) // минимально 
            {
                TLVs = logicLevel.response.DATA; // ведь не факт что придет полный TLV а не только VALUE

                List<LogicLevel.TLV> GetTLV(byte[] data, List<LogicLevel.TLV> TLVs, int pos = 0) // рекурсивная функция парсинга всех TLV из byte[] DATA
                {
                    if (pos <= (data.Length - 1 - 5))
                    {
                        byte[] TAG = data.Skip(pos).Take(2).ToArray();
                        ushort len = logicLevel.ConvertFromByteArray.ToUShort(data.Skip(pos + 2).Take(2).XReverse().ToArray());
                        byte[] VALUE = data.Skip(pos + 2 + 2).Take(len).ToArray();
                        pos = pos + TAG.Length + 2 + len;
                        LogicLevel.TLV TLV = new LogicLevel.TLV(TAG, VALUE);
                        TLVs.Add(TLV);
                        return GetTLV(data, TLVs, pos);
                    }
                    else
                    {
                        return TLVs;
                    }
                }

                List<LogicLevel.TLV> TLVList = new List<LogicLevel.TLV>();
                TLVList = GetTLV(TLVs, TLVList);

                foreach (var prop in this.GetType().GetProperties())
                {
                    var attributes = prop.CustomAttributes.ToList();

                    var TAGAttr = attributes.Where(c => c.AttributeType.Name.ToLower() == "tagattribute").FirstOrDefault(); // атрибут Tag
                    var descriptionAttr = attributes.Where(c => c.AttributeType.Name.ToLower() == "descriptionattribute").FirstOrDefault(); // атрибут Description

                    string descriptionAttrValue = (descriptionAttr != null) ? descriptionAttr.ConstructorArguments[0].Value.ToString() : "";  // значение атрибута Description                    
                    int TAG = (TAGAttr != null) ? (int)TAGAttr.ConstructorArguments[0].Value : 0; // значение атрибута Tag

                    Type originType = (TAGAttr != null) ? (Type)TAGAttr.ConstructorArguments[1].Value : null; // оригинальный тип из ККТ
                    Type type = prop.PropertyType; // тип для вывода в класс
                    //string p_name = prop.Name; // имя свойства
                    //string Description = (descriptionAttr != null) ? (string)descriptionAttr.ConstructorArguments[0].Value : ""; // описание свойства
                    byte[] value = (TLVList.Where(c => logicLevel.ConvertFromByteArray.ToShort(c.TAG.XReverse().ToArray()) == TAG).FirstOrDefault().VALUE != null) ? TLVList.Where(c => logicLevel.ConvertFromByteArray.ToShort(c.TAG.XReverse().ToArray()) == TAG).FirstOrDefault().VALUE : new byte[1] { 0x00 }; // значение свойства

                    if (originType == null) // оригинальный тип данных в ККТ совпадает с моделью, можно заполнять соответствующее свойство
                    {
                        prop.SetValue(this, logicLevel.ConvertFromByteArray.ToGenericType(value.XReverse().ToArray(), type));
                    }
                    else // оригинальный тип данных в ККТ НЕ совпадает с моделью (например теги 1012 1062 1209 1189), такое надо обрабатывать отдельно для каждого тэга
                    {
                        if (TAG == 1012) // Дата и время документа, originType - unt, время в формате unixtime UTC, надо конвертить в нормальный формат dd.MM.yyyy HH.mm.ss
                        {
                            int unixtime = logicLevel.ConvertFromByteArray.ToInt(value);
                            var val = dateTimeHelper.UnixtimeToDateTime(unixtime).ToString("dd.MM.yyyy HH:mm:ss");
                            prop.SetValue(this, val);
                        }
                        else if (TAG == 1062) // Cистемы налогообложения. Битовая маска. 
                        {
                            var a = value[0];
                            string TaxTypesDescription = "";
                            foreach (var item in Enum.GetValues(typeof(TaxTypeEnum))) // цикл по полям enum
                            {
                                var b = (byte)(TaxTypeEnum)item;
                                int r = a & b; // 
                                if (r != 0 && r == b) // если есть пересечение и результат = item
                                {
                                    TaxTypesDescription += (EnumHelper.GetTypeDescription((TaxTypeEnum)item) + ", ");
                                }
                            }
                            if (!string.IsNullOrEmpty(TaxTypesDescription) && TaxTypesDescription.Length >= 2)
                                TaxTypesDescription = TaxTypesDescription.Remove(TaxTypesDescription.Length - 2);
                            var val = TaxTypesDescription;
                            prop.SetValue(this, val);
                        }
                        else if (TAG == 1209) // версия ФФД (2 = 1.05, 3 = 1.1)
                        {
                            var a = value[0];
                            var val = EnumHelper.GetTypeDescription((FFDVersionEnum)a);
                            prop.SetValue(this, val);
                        }
                        else if (TAG == 1189) // версия ФФД ККТ (2 = 1.05, 3 = 1.1)
                        {
                            var a = value[0];
                            var val = EnumHelper.GetTypeDescription((FFDVersionEnum)a);
                            prop.SetValue(this, val);
                        }
                        else if (TAG == 1077) // ФПД, в массиве из 6 байт на печать выводятся байты 2-5, которые интерпретируются, как UInt32, big endian
                        {
                            var a = value.Skip(2).ToArray();
                            var val = logicLevel.ConvertFromByteArray.ToInt(a.Reverse().ToArray()).ToString();
                            prop.SetValue(this, val);
                        }
                    }
                } // останов
            }
        }

        // Основное тело ответа:

        [Tag(1041)]
        [Description("Номер ФН")]
        public string FnNumber { get; set; }

        [Tag(1037)]
        [Description("Регистрационный номер ККТ")]
        public string KKTRegistrationNumber { get; set; }

        [Tag(1018)]
        [Description("ИНН пользователя")]
        public string UserINN { get; set; }

        [Tag(1040)]
        [Description("Номер фискального документа")]
        public int FD { get; set; }

        [Tag(1012, typeof(int))]
        [Description("Дата и время документа")]
        public string DocumentDateTime { get; set; } // unixtime UTC !!! надо конвертить в нормальный формат dd.MM.yyyy HH.mm.ss

        [Tag(1077, typeof(int))]
        [Description("Фискальный признак документа")]
        public string FPD { get; set; }

        [Tag(1056)]
        [Description("Признак шифрования")]
        public bool UseEncryption { get; set; }

        [Tag(1002)]
        [Description("Признак автономного режима")]
        public bool UseOfflineMode { get; set; }

        [Tag(1001)]
        [Description("Признак автоматического режима")]
        public bool UseAuthomaticMode { get; set; }

        [Tag(1109)]
        [Description("Признак расчетов за услуги")]
        public bool UseService { get; set; }

        [Tag(1110)]
        [Description("Признак АС БСО")]
        public bool UseBSO { get; set; }

        [Tag(1108)]
        [Description("Признак ККТ для расчетов в Интернет")]
        public bool UseInternetMode { get; set; }

        [Tag(1062, typeof(byte))]
        [Description("Cистемы налогообложения")]
        public string SNO { get; set; } // битовая маска !!! надо конвертировать в ИмяDescription

        [Tag(1048)]
        [Description("Наименование пользователя")]
        public string UserName { get; set; }

        [Tag(1009)]
        [Description("Адрес расчетов")]
        public string PaymentAdress { get; set; }

        [Tag(1187)]
        [Description("Место расчетов")]
        public string PaymentAdressPlace { get; set; }

        [Tag(1021)]
        [Description("Кассир")]
        public string CashierName { get; set; }

        [Tag(1221)]
        [Description("Признак установки принтера в автомате")]
        public bool UseAuthomatPrinter { get; set; }

        [Tag(1017)]
        [Description("ИНН ОФД")]
        public string OfdINN { get; set; }

        [Tag(1046)]
        [Description("Наименование ОФД")]
        public string OfdName { get; set; }

        [Tag(1117)]
        [Description("Адрес электронной почты отправителя чека")]
        public string SenderEmail { get; set; }

        [Tag(1060)]
        [Description("Адрес сайта ФНС")]
        public string FnsSite { get; set; }

        [Tag(1209, typeof(byte))]
        [Description("Версия ФФД")]
        public string FfdVersion { get; set; } // версия ФФД (2 = 1.05, 3 = 1.1)

        [Tag(1189, typeof(byte))]
        [Description("Версия ФФД ККТ")]
        public string FfdKKTVersion { get; set; } // версия ФФД ККТ (2 = 1.05, 3 = 1.1)

        [Tag(1188)]
        [Description("Версия ККТ")]
        public string KKTVersion { get; set; }

        [Tag(1013)]
        [Description("Заводской номер ККТ")]
        public string KKTManufactureNumber { get; set; }

        [Tag(1208)]
        [Description("Сайт для получения чека")]
        public string OfdSite { get; set; } // не входит в обычный отчет, получается отдельно, через метод (0x34) GetParameterFromTAG
    }

}
