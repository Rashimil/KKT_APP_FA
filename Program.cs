using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KKT_APP_FA
{
    public class Program
    {
        public static Task MainTask = null; // по умолчанию null
        public static Task ControlTask = null; // по умолчанию null
        public static Task SQLiteTask = null; // по умолчанию null
        public static int TransacionsInShift = 0; // количество транзакций в смене
        public static int MaxTransactionsInShift; // максимальное число транзакций, разрешенное в смену. При превышении - автозакрытие смены. Заполняется из конфигурации
        public static bool IsAutomaticDevice; // Признак автомата 

        //=======================================================================================================================================
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //=======================================================================================================================================

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseKestrel(options =>
                {
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                    options.Limits.MinRequestBodyDataRate = null;
                    options.Limits.MinResponseDataRate = null;
                    //options.Listen(IPAddress.Any, 50722); // временно
                }
        );

        //=======================================================================================================================================
    }
}
