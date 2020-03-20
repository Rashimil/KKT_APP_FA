using KKT_APP_FA.Models.DB;
using KKT_APP_FA.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.DB
{
    public class DBRegistrationsService_NO_USE : DbContext // Сервис для работы с БД (Таблица Registrations) через Entity
    {
        IDateTimeHelper dateTimeHelper;
        IConfiguration Configuration;
        string SQLiteConnectionString;
        //=======================================================================================================================================
        public DBRegistrationsService_NO_USE(IDateTimeHelper dateTimeHelper, IConfiguration Configuration, DbContextOptions<DBRegistrationsService_NO_USE> options) : base(options)
        {
            this.dateTimeHelper = dateTimeHelper;
            this.Configuration = Configuration;
            this.SQLiteConnectionString = this.Configuration.GetSection("ConnectionStrings")["LocalDBConnectionString"];
        }

        //=======================================================================================================================================
        public DbSet<RegistrationsContext> Registrations { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(SQLiteConnectionString);
        //}

        //=======================================================================================================================================

        // Запись входящего реквеста:
        public void WriteRequest(string group_code, string operation, string uuid, string timestamp, object request_body, string crc, string kkt_id)
        {
            string request_body_string;
            try
            {
                request_body_string = JsonConvert.SerializeObject(request_body, Formatting.None);
            }
            catch (Exception)
            {
                request_body_string = "";
            }
            RegistrationsContext context = new RegistrationsContext()
            {
                id = Guid.NewGuid().ToString(),
                group_code = group_code,
                operation = operation,
                uuid = uuid,
                timestamp = timestamp,
                request_body = request_body_string,
                crc = crc,
                response_body = "",
                request_date_time = dateTimeHelper.ConvertToUnixTimeSeconds(DateTime.Now.ToString()),
                response_date_time = 0,
                error = true, // по умолчанию еррор = тру
                status = "wait", // для только что пришедших запросов статус = wait
                kkt_id = kkt_id
            };

            // DBRegistrationsService dBRegistrations = new DBRegistrationsService();
            this.Registrations.Add(context);
            this.SaveChanges();
        }

        //=======================================================================================================================================
        //public void InsertList(List<object> lst, string connectionString)
        //{
        //    using (var db = new SQLiteConnection(connectionString))
        //    {
        //        db.Select
        //        db.Insert(lst);
        //        db.Close();
        //    }
        //}

        //=======================================================================================================================================


    }
}
