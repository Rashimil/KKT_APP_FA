using Dapper;
using KKT_APP_FA.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.DB
{
    // Модель таблицы Registrations
    [Table("Registrations")]
    public class RegistrationsContext
    {
        [Key]
        [Required]       
        public string id { get; set; }

        public string group_code { get; set; }

        public string operation { get; set; }

        [Index]
        public string uuid { get; set; }

        public string timestamp { get; set; }

        public string request_body { get; set; }

        public string crc { get; set; }

        public string response_body { get; set; }

        public int request_date_time { get; set; }

        public int response_date_time { get; set; }

        public bool error { get; set; }

        public string status { get; set; }

        public string kkt_id { get; set; } // НЕ УДАЛЯТЬ
    }


    
}
