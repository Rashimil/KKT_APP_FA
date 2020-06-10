using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
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
		public KktRegistrationReport(byte[] registrationReportTLVs, LogicLevel logicLevel)
        {
			if (registrationReportTLVs.Length > 12) // минимально 
			{
				TLVs = registrationReportTLVs.Skip(5).Take(registrationReportTLVs.Length - 7).ToArray();
				// или TLVs = registrationReportTLVs; ведь не факт что придет полный TLV а не только VALUE
			
				List<LogicLevel.TLV> GetTLV(byte[] data, List<LogicLevel.TLV> TLVs, int pos = 0) // рекурсивная функция заполнения листа из TLV
				{
					if (pos <= (data.Length - 1 - 5))
					{
						byte[] TAG = data.Skip(pos).Take(2).ToArray();
						int len = logicLevel.ConvertFromByteArray.ToInt(data.Skip(pos + 2).Take(2).ToArray());
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

			}
		}

		//public void 
    }

	// тестовые заметки...

	public static class KktRegistrationReportHelper
    {	
		private static Dictionary<int, Type> TagTypesList; // Словарь сопоставления тэгов и типов
		static KktRegistrationReportHelper()
		{
			TagTypesList = new Dictionary<int, Type>();
			TagTypesList.Add(1041, typeof(string)); // носер ФН
			TagTypesList.Add(1037, typeof(string)); // Рег. номер ККТ
			TagTypesList.Add(1018, typeof(string)); // ИНН пользователя
			TagTypesList.Add(1040, typeof(int)); // номер ФД
			TagTypesList.Add(1012, typeof(int)); // Дата и время (unixtime UTC)
			TagTypesList.Add(1077, typeof(string)); // ФПД
			TagTypesList.Add(1056, typeof(bool)); // признак шифрования
			TagTypesList.Add(1002, typeof(bool)); // признак автономного режима
			TagTypesList.Add(1001, typeof(bool)); // признак автоматического режима
			TagTypesList.Add(1109, typeof(bool)); // признак расчетов за услуги
			TagTypesList.Add(1110, typeof(bool)); // признак АС БСО
			TagTypesList.Add(1108, typeof(bool)); // признак ККТ для расчетов только в Интернет
			TagTypesList.Add(1062, typeof(byte)); // системы налогообложения (битовая маска)
			TagTypesList.Add(1048, typeof(string)); // наименование пользователя
			TagTypesList.Add(1009, typeof(string)); // адрес расчетов
			TagTypesList.Add(1187, typeof(string)); // Место расчетов
			TagTypesList.Add(1021, typeof(string)); // кассир
			TagTypesList.Add(1221, typeof(bool)); // признак установки принтера в автомате
			TagTypesList.Add(1017, typeof(string)); // ИНН ФОД
			TagTypesList.Add(1046, typeof(string)); // наименование ОФД
			TagTypesList.Add(1117, typeof(string)); // Адрес электронной почты отправителя чека
			TagTypesList.Add(1060, typeof(string)); // Адрес сайта ФНС
			TagTypesList.Add(1209, typeof(byte)); // версия ФФД (2 = 1.05, 3 = 1.1)
			TagTypesList.Add(1189, typeof(byte)); // версия ФФД ККТ (2 = 1.05, 3 = 1.1)
			TagTypesList.Add(1188, typeof(string)); // версия ККТ
			TagTypesList.Add(1013, typeof(string)); // заводской номер ККТ
		}	
	
	public static Dictionary<int, Type> GetTagTypes()
		{
			return TagTypesList;
		}
	}

    public class RegistrationReportItem // не нужен
	{
        public byte[] TAG { get; set; }
        public byte[] LEN { get; set; }
        public byte[] VALUE { get; set; }
        public string VALUE_TYPE { get; set; }
    }
}
