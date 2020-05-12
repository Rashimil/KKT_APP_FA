using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;

namespace KKT_APP_FA.Helpers
{
    public static class ByteOrderHelper 
    {
        static byte val;
        static ByteOrderHelper()
        {
            val = BitConverter.GetBytes(Convert.ToInt32(1234567890))[0]; // 1234567890 = 0x499602D2
        }
        public static SystemByteOrderEnum GetSystemByteOrder()
        {
            if (val == 0xD2)
            {
                // Прямой (интеловский) порядок байт в системе (LITTLE ENDIAN) (от младшего к старшему), НЕ реверсируем ответы
                return SystemByteOrderEnum.LE;
            }
            else
            {
                // Обратный (сетевой) порядок байт в системе (BIG ENDIAN) (от старшего к младшему), реверсируем ответы
                return SystemByteOrderEnum.BE;
            }
        }
    }
}
