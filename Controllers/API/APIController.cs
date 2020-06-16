using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKT_APP_FA.Models.API;
using KKT_APP_FA.Services.DB;
using KKT_APP_FA.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using KKT_APP_FA.Extensions;

namespace KKT_APP_FA.Controllers
{
    // Контроллер, на входе ловит запросы от MainApp. 

    [Produces("application/json")]
    public class APIController : Controller// : ControllerBase
    {
        SQLiteService SQLiteService;
        IConfiguration Configuration;
        ILoggerHelper logger;
        APIHelper aPIHelper;
        ICRC32 CRC32;
        //string SQLiteConnectionString;
        string MainAppConnectionString;

        //=======================================================================================================================================
        public APIController(SQLiteService sQLiteService, IConfiguration configuration, ILoggerHelper loggerHelper, APIHelper aPIHelper, ICRC32 CRC32)
        {
            this.SQLiteService = sQLiteService;
            this.Configuration = configuration;
            this.logger = loggerHelper;
            this.aPIHelper = aPIHelper;
            this.CRC32 = CRC32;
            MainAppConnectionString = this.Configuration.GetSection("ConnectionStrings")["MainAppConnectionString"];
        }

        //=======================================================================================================================================

        [Route("api/v4/")]
        [HttpPost]
        public object Register([FromBody]Request request) // Обработка реквеста
        {
            string original_transaction_string = "";

            try
            {
                string request_string = JsonConvert.SerializeObject(request);

                // Пишем лог входящего запроса:
                logger.Write(request_string, "request_log");

                // Проверка CRC:
                string crc = CRC32.Crc(request.group_code, request.operation, request.uuid);

                // Если CRC НЕ совпал:
                if (crc != request.crc)
                {
                    // Пишем в БД реквест:
                    SQLiteService.WriteRequest(request.group_code, request.operation, request.uuid, request.timestamp, request.body, request.crc, request.kkt_id);

                    // Пишем лог неверного CRC:
                    logger.Write(request_string + (char)13 + (char)10 + "request CRC: " + request.crc + (char)13 + (char)10 + "calculated CRC: " + crc, "incorrect_crc_log");

                    // Создаем "успокоительный" ответ-заглушку для Main-APP.APIController (он ждет ответа)
                    Response res = new Response()
                    {
                        group_code = request.group_code,
                        operation = request.operation,
                        uuid = request.uuid,
                        timestamp = DateTime.Now.ToString(),
                        error = false, // Тут всегда false, т. к. это успокоительный ответ
                        body = "Incorrect CRC" + (char)13 + (char)10 + "request CRC: " + request.crc + (char)13 + (char)10 + "calculated CRC: " + crc,
                        crc = request.crc,
                        kkt_id = request.kkt_id
                    };

                    // Пишем в БД результат, чтобы цикл его не нашел:
                    SQLiteService.WriteResult(request.group_code, request.operation, request.uuid, res.timestamp, res.body, res);

                    return res;
                }

                // Если CRC СОВПАЛ:
                else
                {
                    // Проверка на наличие такой транзакции в БД (защита от повторных фискализаций):
                    original_transaction_string = SQLiteService.GetTransaction(request.group_code, request.operation, request.uuid);

                    // Если оригинальная транзакция НЕ нашлась - пишем в БД для цикла:
                    if (original_transaction_string == null || original_transaction_string == "")
                    {
                        // Пишем в БД реквест, цикл сам разберется с ним:
                        SQLiteService.WriteRequest(request.group_code, request.operation, request.uuid, request.timestamp, request.body, request.crc, request.kkt_id);

                        // Создаем "успокоительный" ответ-заглушку для Main-APP.APIController (он ждет ответа) 
                        Response res = new Response()
                        {
                            group_code = request.group_code,
                            operation = request.operation,
                            uuid = request.uuid,
                            timestamp = DateTime.Now.XToString(),
                            error = false, // Тут всегда false, т. к. это успокоительный ответ
                            body = null,
                            crc = request.crc,
                            kkt_id = request.kkt_id
                        };

                        return res;
                    }

                    // Если оригинальная тразакция НАШЛАСЬ - возвращаем результат транзакции из БД:
                    else
                    {
                        // Пишем лог POST запроса на MainApp:
                        logger.Write(original_transaction_string + " ОТВЕТ С АПИ КОНТРОЛЛЕРА - оригинальная транзакция нашлась " + DateTime.Now.ToString(), "post_to_main_app_log");

                        // POST запросом её отправляем на MainAppConnection:
                        string request_to_main_app_status = aPIHelper.POST_to_main_app(original_transaction_string);

                        // Пишем лог статуса POST запроса на MainApp:
                        logger.Write(request_to_main_app_status, "post_to_main_app_status_log");

                        // Создаем "успокоительный" ответ-заглушку для Main-APP.APIController (он ждет ответа)
                        Response res = new Response()
                        {
                            group_code = request.group_code,
                            operation = request.operation,
                            uuid = request.uuid,
                            timestamp = DateTime.Now.ToString(),
                            error = false, // Тут всегда false, т. к. это успокоительный ответ
                            body = "original_transaction_string is found",
                            crc = request.crc,
                            kkt_id = request.kkt_id
                        };

                        return res;
                    }
                }
            }
            catch (Exception ex) // Если исключение в контроллере
            {
                // Пишем лог исключения в контроллере:
                logger.Write(ex.ToString(), "ApiController_Register_exception_log");

                // Создаем "успокоительный" ответ для Main-APP.APIController (он ждет ответа)
                Response res = new Response()
                {
                    group_code = request.group_code,
                    operation = request.operation,
                    uuid = request.uuid,
                    timestamp = DateTime.Now.ToString(),
                    error = true,
                    body = null,
                    crc = request.crc,
                    kkt_id = request.kkt_id
                };

                return res;
            }
            
        }

        //=======================================================================================================================================

        //[Route("api/v4/")]
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View("Index");
        //}
        //=======================================================================================================================================
    }
}