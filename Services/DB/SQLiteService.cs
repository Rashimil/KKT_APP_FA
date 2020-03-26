//using DapperExtensions;
using KKT_APP_FA.Models.API;
using KKT_APP_FA.Models.DB;
using KKT_APP_FA.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.DB
{
    // Сервис для работы с SQLITE
    public class SQLiteService : BaseService
    {
        IDateTimeHelper dateTimeHelper;
        IConfiguration Configuration;
        string SQLiteConnectionString;
        //=======================================================================================================================================
        public SQLiteService(IDateTimeHelper dateTimeHelper, IConfiguration Configuration)
        {
            this.dateTimeHelper = dateTimeHelper;
            this.Configuration = Configuration;
            this.SQLiteConnectionString = this.Configuration.GetSection("ConnectionStrings")["LocalDBConnectionString"];
            CreateSQLiteDatabaseIfNeed(); // автосоздание SQLite файла 
        }
        //=======================================================================================================================================

        // Запись входящего реквеста (асинхронно):
        public async Task WriteRequestAsync(string group_code, string operation, string uuid, string timestamp, object request_body, string crc, string kkt_id)
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

            await InsertAsync(context, SQLiteConnectionString);
        }

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

            Insert<RegistrationsContext>(context, SQLiteConnectionString);
        }

        //=======================================================================================================================================

        // Запись результатов фискализации (асинхронно):
        public async Task WriteResultAsync(string group_code, string operation, string uuid, string timestamp, object response_body, Response response)
        {
            RegistrationsContext context = Select<RegistrationsContext>(new { uuid = uuid }, SQLiteConnectionString).FirstOrDefault();  //db.Get<RegistrationsContext>(contextuuid);

            try
            {
                context.response_body = JsonConvert.SerializeObject(response_body);
            }
            catch (Exception)
            {
                context.response_body = "";
            }
            context.response_date_time = dateTimeHelper.ConvertToUnixTimeSeconds(DateTime.Now);
            context.error = response.error;
            context.status = "done"; // done - значит обработано

            await UpdateAsync<RegistrationsContext>(context, SQLiteConnectionString);
        }

        //=======================================================================================================================================

        // Запись результатов фискализации:
        public void WriteResult(string group_code, string operation, string uuid, string timestamp, object response_body, Response response)
        {
            RegistrationsContext context = Select<RegistrationsContext>(new { uuid = uuid }, SQLiteConnectionString).FirstOrDefault();  //db.Get<RegistrationsContext>(contextuuid);

            try
            {
                context.response_body = JsonConvert.SerializeObject(response_body);
            }
            catch (Exception)
            {
                context.response_body = "";
            }
            context.response_date_time = dateTimeHelper.ConvertToUnixTimeSeconds(DateTime.Now);
            context.error = response.error;
            context.status = "done"; // done - значит обработано

            Update<RegistrationsContext>(context, SQLiteConnectionString);
        }

        //=======================================================================================================================================

        // Проверка наличия транзакции с таким group_code, operation и uid в БД:
        public bool IsTransactionExist(string group_code, string operation, string uuid)
        {
            try
            {
                RegistrationsContext context = Select<RegistrationsContext>(new { uuid = uuid }, SQLiteConnectionString).FirstOrDefault();
                if (context != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //=======================================================================================================================================

        // Поиск транзакции в БД по UUID: (возвращает готовый response как строку)
        public string GetTransaction(string group_code, string operation, string uuid)
        {
            try
            {
                RegistrationsContext context = Select<RegistrationsContext>(new { uuid = uuid }, SQLiteConnectionString).FirstOrDefault();
                if (context != null)
                {
                    object body;
                    try
                    {
                        body = JsonConvert.DeserializeObject(context.response_body);
                    }
                    catch (Exception)
                    {
                        body = "";
                    }
                    Response response = new Response()
                    {
                        group_code = group_code,
                        operation = operation,
                        uuid = uuid,
                        timestamp = context.timestamp,
                        error = context.error,
                        body = body,
                        crc = context.crc
                    };
                    return JsonConvert.SerializeObject(response, Formatting.None);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //=======================================================================================================================================

        // Поиск транзакции со статусом wait:
        public RegistrationsContext GetWaitTransaction() 
        {
            RegistrationsContext context = Select<RegistrationsContext>(new { status = "wait" }, SQLiteConnectionString).FirstOrDefault(); 
            return context;
        }

        //=======================================================================================================================================

        // Автосоздание БД SQLite, если таковая отсутствует
        private void CreateSQLiteDatabaseIfNeed()
        {
            try
            {
                GetWaitTransaction();
            }
            catch (Exception) // файл БД отсутствует, нужно его создать 
            {
                List<string> sqls = GetCreateSQLiteDBString(new RegistrationsContext());
                foreach (var sql in sqls)
                {
                    SendSQLQuery(sql, SQLiteConnectionString);
                }
            }
        }

        //=======================================================================================================================================

        // Автоочистка файла БД SQLite (удаление старых транзакций + vacuum)
        public void ClearSQLiteDB()
        {
            try
            {
                string response_date_time = (dateTimeHelper.GetCurrentDayToUnixTimeSeconds() - 24 * 60 * 60).ToString(); // Текущий день минус сутки
                List<string> sqls = new List<string>();
                string table_name = new RegistrationsContext().GetType().Name.Replace("Context", "");
                string sql = "DELETE FROM " + (table_name + " WHERE [status] = 'done' AND [request_date_time] < " + response_date_time);
                sqls.Add(sql);
                sql = "VACUUM";
                sqls.Add(sql);
                foreach (var s in sqls)
                {
                    SendSQLQuery(s, SQLiteConnectionString);
                }
            }
            catch (Exception)
            {
            }
        }

        //=======================================================================================================================================
    }

}