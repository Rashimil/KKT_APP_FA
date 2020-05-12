using KKT_APP_FA.Models;
using KKT_APP_FA.Models.API;
using KKT_APP_FA.Models.DB;
using KKT_APP_FA.Services.DB;
using KKT_APP_FA.Services.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KKT_APP_FA.Units
{
    // Главный цикл обработки
    public class MainCycle
    {
        ICRC32 CRC32;
        SQLiteService sQLiteService;
        APIHelper aPIHelper;
        ILoggerHelper logger;
        static TaskObject taskObject;
        IConfiguration Configuration;

        //=======================================================================================================================================
        public MainCycle(ICRC32 CRC32, SQLiteService sQLiteService, APIHelper aPIHelper, ILoggerHelper logger, IConfiguration configuration)
        {
            this.CRC32 = CRC32;
            this.sQLiteService = sQLiteService;
            this.aPIHelper = aPIHelper;
            this.logger = logger;
            this.Configuration = configuration;
            taskObject = new TaskObject(CRC32, sQLiteService, aPIHelper, logger, configuration);

        }
        //=======================================================================================================================================

        // Объект для передачи в Task
        public class TaskObject
        {
            public ICRC32 CRC32 { get; set; }
            public SQLiteService sQLiteService { get; set; }
            public APIHelper aPIHelper { get; set; }
            public ILoggerHelper logger { get; set; }
            public IConfiguration configuration { get; set; }
            public TaskObject(ICRC32 cRC32, SQLiteService sQLiteService, APIHelper aPIHelper, ILoggerHelper logger, IConfiguration configuration)
            {
                this.CRC32 = cRC32;
                this.sQLiteService = sQLiteService;
                this.aPIHelper = aPIHelper;
                this.logger = logger;
                this.configuration = configuration;
            }

            // Работа с главной очередью запросов, на вход - строка элемент очереди
            public void QueueWork(string request_string)
            {
                Request request = JsonConvert.DeserializeObject<Request>(request_string); // Десериализуем его (первый элемент очереди)

                if (request != null && !string.IsNullOrEmpty(request.group_code) && !string.IsNullOrEmpty(request.operation) && !string.IsNullOrEmpty(request.uuid))
                {

                    KKTHighLevel kKTHighLevel = new KKTHighLevel(); // Создаем новый экземпляр KKTHighLevel

                    // Далее работа с запросом из очереди:
                    KKTHighLevelResponse FiscalResult = null; // прямой ответ из ККТ
                    dynamic body = null; // body РЕКВЕСТА А НЕ РЕСПОНСА!!!!!! 
                    Response response = new Response(); // Сам респонс
                    if (request != null)
                    {
                        response.group_code = request.group_code;
                        response.operation = request.operation;
                        response.uuid = request.uuid;
                        response.timestamp = request.timestamp;
                        response.crc = CRC32.Crc(response.group_code, response.operation, response.uuid);
                    }

                    // Если операция = приход, расход, возврат прихода, возврат расхода:
                    if ((request != null) && (request.operation.ToLower() == "sell" || request.operation.ToLower() == "buy" || request.operation.ToLower() == "sell_refund" || request.operation.ToLower() == "buy_refund"))
                    {
                        body = JsonConvert.DeserializeObject<Registration>(JsonConvert.SerializeObject(request.body));
                        FiscalResult = kKTHighLevel.Register(body, request.operation); // метод регистрации
                    }
                    // Если операция = коррекция
                    else if ((request != null) && (request.operation.ToLower() == "sell_correction" || request.operation.ToLower() == "buy_correction"))
                    {
                        body = JsonConvert.DeserializeObject<Correction>(JsonConvert.SerializeObject(request.body));
                        FiscalResult = kKTHighLevel.Correction(body, request.operation); // метод коррекции
                    }

                    // Если операция = открытие смены (не проходит валидатор в MAIN-APP, доступно только с кабинета):
                    //else if ((request != null) && (request.operation.ToLower() == "openshift")) //
                    //{
                    //    body = null;
                    //    FiscalResult = kKTHighLevel.OpenShift(); // метод открытия смены
                    //}

                    // Если операция = закрытие смены (не проходит валидатор в MAIN-APP, доступно только с кабинета):
                    //else if ((request != null) && (request.operation.ToLower() == "closeshift")) //
                    //{
                    //    body = null;
                    //    FiscalResult = kKTHighLevel.CloseShift(); // метод закрытия смены
                    //}

                    // Если операция = запрос информации о ККТ (не проходит валидатор в MAIN-APP, доступно только с кабинета):
                    else if ((request != null) && (request.operation.ToLower() == "getkktinfo"))
                    {
                        body = null;
                        FiscalResult = null;
                    }

                    else // Если ничего не совпало - вернуть какую ниботь неизвестную ошибку
                    {
                        body = null;
                        FiscalResult = new KKTHighLevelResponse();
                        FiscalResult.error = true;
                    }

                    // ==================================================================================

                    if (FiscalResult is null) // Значит операция нефискальная (например проверка статуса или изменение настроек)
                    {
                        if ((request != null) && (request.operation.ToLower() == "getkktinfo"))
                        {
                            response.body = kKTHighLevel.GetKktInfo();
                            response.error = false;
                            response.kkt_id = request.kkt_id;
                        }
                    }
                    else if (FiscalResult.error == false) // Если нет ошибок фискализации
                    {
                        ResponseOK responseOK = new ResponseOK()
                        {
                            uuid = request.uuid,
                            error = null,
                            status = "done",
                            payload = new Payload()
                            {
                                total = (double)FiscalResult.total,
                                fns_site = FiscalResult.fns_site,
                                fn_number = FiscalResult.fn_number,
                                shift_number = FiscalResult.shift_number,
                                receipt_datetime = FiscalResult.receipt_datetime,
                                fiscal_receipt_number = FiscalResult.fiscal_receipt_number,
                                fiscal_document_number = FiscalResult.fiscal_document_number,
                                ecr_registration_number = FiscalResult.ecr_registration_number,
                                fiscal_document_attribute = FiscalResult.fiscal_document_attribute
                            },
                            timestamp = request.timestamp,
                            group_code = request.group_code,
                            daemon_code = FiscalResult.daemon_code,
                            device_code = FiscalResult.device_code
                        };

                        if (body is null || body.service is null || body.service.callback_url is null)
                        {
                            responseOK.callback_url = "";
                        }
                        else
                        {
                            responseOK.callback_url = body.service.callback_url;
                        }

                        response.body = responseOK;
                        response.error = false;
                        Program.TransacionsInShift = FiscalResult.fiscal_receipt_number; // увеличиваем счетчик транзакций за смену (приравниваем к номеру чека за смену)
                    }
                    else // Если есть ошибки фискализации
                    {
                        ResponseError responseError = new ResponseError()
                        {
                            error = new Error()
                            {
                                error_id = Guid.NewGuid().ToString(),
                                code = FiscalResult.error_code,
                                text = FiscalResult.error_text,
                                type = FiscalResult.error_type,
                            },
                            status = "fail",
                            payload = null,
                            timestamp = request.timestamp,
                        };
                        if (body is null || body.service is null || body.service.callback_url is null)
                        {
                            responseError.callback_url = "";
                        }
                        else
                        {
                            responseError.callback_url = body.service.callback_url;
                        }

                        response.body = responseError;
                        response.error = true;
                    }

                    // Пишем в БД респонс:
                    sQLiteService.WriteResult(response.group_code, response.operation, response.uuid, response.timestamp, response.body, response);

                    // Тут POST запросом всё отправляем на MainAppConnection:
                    var task = Task.Factory.StartNew(() =>  // вложенная задача (новым потоком)
                    {
                        var post_string = JsonConvert.SerializeObject(response);
                        try
                        {
                            // Пишем лог:
                            logger.Write(post_string + " ОТВЕТ С ГЛАВНОГО ЦИКЛА - оригинальная транзакция НЕ нашлась " + DateTime.Now.ToString(), "post_to_main_app_log");

                            string request_to_main_app_status = aPIHelper.POST_to_main_app(post_string);

                            // Пишем лог:
                            logger.Write(request_to_main_app_status, "post_to_main_app_status_log");
                        }
                        catch (Exception ex)
                        {
                            // Пишем лог:
                            logger.Write(ex.ToString(), "post_to_main_app_exception_log");
                        }
                    }, TaskCreationOptions.AttachedToParent);

                    // Если превышено максимальное число транзакций в смену:
                    if (Program.TransacionsInShift >= Program.MaxTransactionsInShift)
                    {
                        kKTHighLevel.CloseShift(); // То вызываем метод закрытия смены
                    }
                    // Проверка кода ошибки на наличие в списках для нестандартной реакции:
                    //if (EnumerationLists.IsFound(responseError.error.code, EnumerationLists.ErrorCodes_for_CloseShift)) // если ErrorCodes_for_CloseShift содержит этот код ошибки
                    //{
                    //    kKT.CloseShift(); // То вызываем метод закрытия смены
                    //}

                    kKTHighLevel = null; // Уничтожаем экземпляр ККТ
                }
                else // Значит был кривой запрос, оказавшийся в БД
                {

                }
                // ==================================================================================
            }
        }

        //=======================================================================================================================================

        // Метод, вызывающийся в цикле внутри Task
        public void CycleMethod(TaskObject TaskObject)
        {
            string SQLiteConnectionString = TaskObject.configuration.GetSection("ConnectionStrings")["LocalDBConnectionString"];
                RegistrationsContext registration_context = TaskObject.sQLiteService.GetWaitTransaction(); // берём первую транзакцию из БД со статусом wait
                try
                {
                    // string request_string = Startup.requestQueue.Dequeue(); // Забираем первый элемент с таблицы, подходящий под условия. К этому моменту контроллер уже должен кинуть входящий запрос в БД
                    // т. е. выбираем только свежие запросы. Несвежие/повторы отсеет AppController                       
                    if (registration_context != null)
                    {
                        Request request = new Request()
                        {
                            group_code = registration_context.group_code,
                            operation = registration_context.operation,
                            uuid = registration_context.uuid,
                            timestamp = registration_context.timestamp,
                            body = JsonConvert.DeserializeObject(registration_context.request_body),
                            crc = registration_context.crc,
                            kkt_id = registration_context.kkt_id
                        };
                        string incoming_request_string = JsonConvert.SerializeObject(request);
                        if (incoming_request_string != null && incoming_request_string != "")
                        {
                            TaskObject.QueueWork(incoming_request_string); // работаем с элементом
                        }
                        registration_context = null; // Task.CompletedTask
                    }
                }
                catch (Exception ex)
                {
                    TaskObject.logger.Write(ex.ToString(), "QueueWork_exception_log");
                    // Тут надо в БД ставить статус done
                    registration_context.status = "done(QueueWork_exception)";
                    TaskObject.sQLiteService.Update<RegistrationsContext>(registration_context, SQLiteConnectionString);
                }
        }

        //=======================================================================================================================================

        // Старт CycleTask
        public void Start()
        {
            //Action qwe = new Action<SQLiteService>( Action);

            //Func<> Cycle = new Func() { };

            //Task t = new Task((sQLiteService) => { }, new CancellationToken());

            // Получение статичных полей ответа от ККТ:
            KKTHighLevel kKTHighLevel = new KKTHighLevel(); // Создаем новый экземпляр KKTHighLevel
            kKTHighLevel.GetStaticResponseFields();
            kKTHighLevel = null;

            // Старт потока главного цикла:
            Program.MainTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    CycleMethod(taskObject);
                    Thread.Sleep(100); // Ждем чтобы не грузить сервер
                }
            });
        }

        //=======================================================================================================================================


    }
}
