using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Helpers 
{
    public class CRC16CCITT
    {
        private ushort POLY; // полином
        private ushort[] Table { get; set; }
        private ushort InitialValue { get; set; }

        public CRC16CCITT()
        {
            POLY = 0x1021;
            Table = new ushort[256];
            InitialValue = 0xffff;
            ushort temp, a;
            for (int i = 0; i < Table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                    {
                        temp = (ushort)((temp << 1) ^ POLY);
                    }
                    else
                    {
                        temp <<= 1;
                    }
                    a <<= 1;
                }
                Table[i] = temp;
            }
        }

        //==============================================================================================================================================

        // Вычисление чексуммы (байтмассив)
        public byte[] ComputeCheckSumBytes(byte[] bytes)
        {
            ushort crc = ComputeCheckSum(bytes);
            return BitConverter.GetBytes(crc);
        }

        //==============================================================================================================================================

        // Вычисление чексуммы (число)
        private ushort ComputeCheckSum(byte[] bytes)
        {
            ushort crc = InitialValue;
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ Table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

        //==============================================================================================================================================

        public ushort crc16_update(ushort crc16, byte data)
        {
            crc16 ^= (ushort)(data << 8);
            for (byte i = 0; i < 8; i++) crc16 = (ushort)(((crc16 & 0x8000) != 0) ? (crc16 << 1) ^ 0x1021 : crc16 << 1);
            return crc16;
        }

        //==============================================================================================================================================
    }
}
