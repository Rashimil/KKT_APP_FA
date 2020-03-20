using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models
{
    // Настройки аппарата
    public class TerminalFASettings
    {
        public TerminalFASettings(IConfiguration Configuration)
        {
            try {
                this.IP = Configuration.GetSection("TereminalFASettings")["IP"];
                this.PORT = Convert.ToInt32(Configuration.GetSection("TereminalFASettings")["PORT"]);
                this.READ_TIMEOUT = Convert.ToInt32(Configuration.GetSection("TereminalFASettings")["READ_TIMEOUT"]);
                this.BUFFER_SIZE = Convert.ToInt32(Configuration.GetSection("TereminalFASettings")["BUFFER_SIZE"]);
            } 
            catch (Exception) {
                this.IP = "0.0.0.0";
                this.PORT = 5555;
                this.READ_TIMEOUT = 10;
                this.BUFFER_SIZE = 1;
            }
        }
        public string IP { get; set; } // IP аппарата
        public int PORT { get; set; } // Порт аппарата
        public int READ_TIMEOUT { get; set; } // таймаут чтения с порта (потока), сек
        public int BUFFER_SIZE { get; set; } // размер буфера чтения с порта (потока), байт

    }
}
