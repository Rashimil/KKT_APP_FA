using KKT_APP_FA.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    // Конвертация из Unicode в 866, с отбрасыванием несовместимых символов (НЕ НУЖЕН !!!)
    public class CP866Helper 
    {
        private Dictionary<string, byte> CP866List; // Unicode:866

        //=======================================================================================================================================
        public CP866Helper()
        {
            CP866List = new Dictionary<string, byte>();
            CP866List.Add("410", 0x80);
            CP866List.Add("411", 0x81);
            CP866List.Add("412", 0x82);
            CP866List.Add("413", 0x83);
            CP866List.Add("414", 0x84);
            CP866List.Add("415", 0x85);
            CP866List.Add("416", 0x86);
            CP866List.Add("417", 0x87);
            CP866List.Add("418", 0x88);
            CP866List.Add("419", 0x89);
            CP866List.Add("41A", 0x8A);
            CP866List.Add("41B", 0x8B);
            CP866List.Add("41C", 0x8C);
            CP866List.Add("41D", 0x8D);
            CP866List.Add("41E", 0x8E);
            CP866List.Add("41F", 0x8F);
            CP866List.Add("420", 0x90);
            CP866List.Add("421", 0x91);
            CP866List.Add("422", 0x92);
            CP866List.Add("423", 0x93);
            CP866List.Add("424", 0x94);
            CP866List.Add("425", 0x95);
            CP866List.Add("426", 0x96);
            CP866List.Add("427", 0x97);
            CP866List.Add("428", 0x98);
            CP866List.Add("429", 0x99);
            CP866List.Add("42A", 0x9A);
            CP866List.Add("42B", 0x9B);
            CP866List.Add("42C", 0x9C);
            CP866List.Add("42D", 0x9D);
            CP866List.Add("42E", 0x9E);
            CP866List.Add("42F", 0x9F);
            CP866List.Add("430", 0xA0);
            CP866List.Add("431", 0xA1);
            CP866List.Add("432", 0xA2);
            CP866List.Add("433", 0xA3);
            CP866List.Add("434", 0xA4);
            CP866List.Add("435", 0xA5);
            CP866List.Add("436", 0xA6);
            CP866List.Add("437", 0xA7);
            CP866List.Add("438", 0xA8);
            CP866List.Add("439", 0xA9);
            CP866List.Add("43A", 0xAA);
            CP866List.Add("43B", 0xAB);
            CP866List.Add("43C", 0xAC);
            CP866List.Add("43D", 0xAD);
            CP866List.Add("43E", 0xAE);
            CP866List.Add("43F", 0xAF);
            CP866List.Add("2591", 0xB0);
            CP866List.Add("2592", 0xB1);
            CP866List.Add("2593", 0xB2);
            CP866List.Add("2502", 0xB3);
            CP866List.Add("2524", 0xB4);
            CP866List.Add("2561", 0xB5);
            CP866List.Add("2562", 0xB6);
            CP866List.Add("2556", 0xB7);
            CP866List.Add("2555", 0xB8);
            CP866List.Add("2563", 0xB9);
            CP866List.Add("2551", 0xBA);
            CP866List.Add("2557", 0xBB);
            CP866List.Add("255D", 0xBC);
            CP866List.Add("255C", 0xBD);
            CP866List.Add("255B", 0xBE);
            CP866List.Add("2510", 0xBF);
            CP866List.Add("2514", 0xC0);
            CP866List.Add("2534", 0xC1);
            CP866List.Add("252C", 0xC2);
            CP866List.Add("251C", 0xC3);
            CP866List.Add("2500", 0xC4);
            CP866List.Add("253C", 0xC5);
            CP866List.Add("255E", 0xC6);
            CP866List.Add("255F", 0xC7);
            CP866List.Add("255A", 0xC8);
            CP866List.Add("2554", 0xC9);
            CP866List.Add("2569", 0xCA);
            CP866List.Add("2566", 0xCB);
            CP866List.Add("2560", 0xCC);
            CP866List.Add("2550", 0xCD);
            CP866List.Add("256C", 0xCE);
            CP866List.Add("2567", 0xCF);
            CP866List.Add("2568", 0xD0);
            CP866List.Add("2564", 0xD1);
            CP866List.Add("2565", 0xD2);
            CP866List.Add("2559", 0xD3);
            CP866List.Add("2558", 0xD4);
            CP866List.Add("2552", 0xD5);
            CP866List.Add("2553", 0xD6);
            CP866List.Add("256B", 0xD7);
            CP866List.Add("256A", 0xD8);
            CP866List.Add("2518", 0xD9);
            CP866List.Add("250C", 0xDA);
            CP866List.Add("2588", 0xDB);
            CP866List.Add("2584", 0xDC);
            CP866List.Add("258C", 0xDD);
            CP866List.Add("2590", 0xDE);
            CP866List.Add("2580", 0xDF);
            CP866List.Add("440", 0xE0);
            CP866List.Add("441", 0xE1);
            CP866List.Add("442", 0xE2);
            CP866List.Add("443", 0xE3);
            CP866List.Add("444", 0xE4);
            CP866List.Add("445", 0xE5);
            CP866List.Add("446", 0xE6);
            CP866List.Add("447", 0xE7);
            CP866List.Add("448", 0xE8);
            CP866List.Add("449", 0xE9);
            CP866List.Add("44A", 0xEA);
            CP866List.Add("44B", 0xEB);
            CP866List.Add("44C", 0xEC);
            CP866List.Add("44D", 0xED);
            CP866List.Add("44E", 0xEE);
            CP866List.Add("44F", 0xEF);
            CP866List.Add("401", 0xF0);
            CP866List.Add("451", 0xF1);
            CP866List.Add("404", 0xF2);
            CP866List.Add("454", 0xF3);
            CP866List.Add("407", 0xF4);
            CP866List.Add("457", 0xF5);
            CP866List.Add("40E", 0xF6);
            CP866List.Add("45E", 0xF7);
            CP866List.Add("B0", 0xF8);
            CP866List.Add("2219", 0xF9);
            CP866List.Add("B7", 0xFA);
            CP866List.Add("221A", 0xFB);
            CP866List.Add("2116", 0xFC);
            CP866List.Add("A4", 0xFD);
            CP866List.Add("25A0", 0xFE);
            CP866List.Add("A0", 0xFF);
        }

        //=======================================================================================================================================

        // Конвертирует UNICODE строку (1 символ = 2 байта) в массив кодировки CP866 (1 символ = 1 байт)
        public byte[] StringToByteArray(string unucode_string)
        {

            byte[] tmp_arr = Encoding.Unicode.GetBytes(unucode_string);
            List<byte> result = new List<byte>();

            // item[нечетное] - сами значения
            // item[четное] - признак того что не влезет в 1 байт
            // если item[четное] =0 то оставляем пердыдущий нечетный байт как есть, т. к. для них Unicode = 866

            // если item[четное] !=0 то работаем с предыдущим нечетным байтом так:
            // конвертим в hex, далее если он меньше

            for (int i = 0; i < tmp_arr.Length; i += 2)
            {
                //int int_code = (tmp_arr[i + 1] * 100) + tmp_arr[i];
                string tmp_str;
                if (tmp_arr[i + 1] == 0)
                    tmp_str = "";
                else
                    tmp_str = tmp_arr[i + 1].ToString("X");
                string hex = tmp_str + tmp_arr[i].ToString("X"); // 43D 438 441 434 430 2E 63 6F 6D 70 61 6E 79 2E 72 75
                if (tmp_str != "")
                {
                    // тут генерим byte для соответствующего hex
                    try
                    {
                        result.Add(CP866List[hex]);
                    }
                    catch (Exception)
                    {
                        result.Add(0xFF); // 0xFF - пробел
                    }
                }
                else
                {
                    result.Add(tmp_arr[i]);
                }
            }
            return result.ToArray();
        }

        //=======================================================================================================================================

        // Возвращает UNICODE строку (1 символ = 2 байта) из массива кодировки 866 (1 символ = 1 байт)
        public string ByteArrayToString(byte[] arr_866)
        
        {
            string result = "";
            foreach (var item in arr_866)
            {
                short unicode_code = Convert.ToInt16(CP866List.Where(c => c.Value == item).FirstOrDefault().Key, 16);
                // он не всегда находит, т. к. CP866List содержит только русскую часть таблицы.
                // тогда просто транслировать байт => unicode если вернуло 0
                if (unicode_code == 0) // ничего не нашлось
                {
                    //result += Encoding.Unicode.GetString((new byte[] { item, 0x00 }).XReverse().ToArray());
                    result += (char)item;
                }
                else
                {
                    byte[] arr = BitConverter.GetBytes(unicode_code);
                    result += Encoding.Unicode.GetString((arr).XReverse().ToArray());
                }
                try
                {
                
                    //string sym = Char.ConvertFromUtf32(unicode_code);
                    //result += 
                }
                catch (Exception)
                {
                    result += " ";
                }
            }
            return result;
        }

        //=======================================================================================================================================

        public void GetFromCP866List()
        {

        }

        //=======================================================================================================================================
    }
}
