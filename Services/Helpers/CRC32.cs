using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    public class CRC32 : ICRC32
    {
        IConfiguration configuration;

        //=======================================================================================================================================
        public CRC32(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //=======================================================================================================================================
        public string Crc(string group_code, string operation, string uuid) // Вычисление CRC для подписи
        {
            using (var sha1 = new SHA1Managed())
            {
                string salt = $"{configuration["KktApiSettings:Salt"]}";
                string stringToHash = group_code + operation + uuid + salt;
                string crc = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
                crc = crc.Replace("-", "");
                return crc.ToLower();
            }
        }

        //=======================================================================================================================================
    }
}
