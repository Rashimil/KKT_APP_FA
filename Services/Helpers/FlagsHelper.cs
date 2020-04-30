using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    // Работа с флагами (например флаги стостояния ФН)
    public class FlagsHelper
    {
        // Декодировка байта в строку бит вида 10001100
        public string DecodeFlags(byte Flags)
        {
            bool[] bits = new bool[8];
            Dictionary<int, string> result = new Dictionary<int, string>();
            string tmp = "";
            BitArray bitArr = new BitArray(BitConverter.GetBytes(Flags));
            for (int i = 0; i <= 7; i++)
            {
                bits[i] = ((Flags >> i) & 1) != 0;
            }
            List<string> stringlist = new List<string>();
            foreach (var bit in bits)
            {
                if (bit)
                {
                    //tmp += "1";
                    stringlist.Add("1");
                }
                else
                {
                    //tmp += "0";
                    stringlist.Add("0");
                }
            }
            stringlist.Reverse(); // ВРЕМЕННЫЙ РЕВЕРС
            foreach (var item in stringlist)
            {
                if (item == "1")
                {
                    tmp += "1";
                }
                else
                {
                    tmp += "0";
                }
            }
            return tmp;
        }
    }
}
