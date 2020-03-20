using KKT_APP_FA.Services.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    public class LoggerHelper : ILoggerHelper
    {
        IDateTimeHelper dateTimeHelper;
        IConfiguration configuration;
        bool use_logger;

        //=======================================================================================================================================
        public LoggerHelper(IDateTimeHelper dateTimeHelper, IConfiguration configuration)
        {
            this.dateTimeHelper = dateTimeHelper;
            if (logQueue == null)
            {
                this.configuration = configuration;
                use_logger = this.configuration.GetSection("Logging")["UseLogger"].ToLower() == "true" ? true : false;
                logQueue = new Queue<QueeItem>(); // Singleton
                this.Init();
            }
        }

        //=======================================================================================================================================
        private class QueeItem // элемент очереди
        {
            public DateTime date_time;
            public string msg;
            public string file_name;
            public bool use_separator;
        }
        private static Queue<QueeItem> logQueue = null; // очередь запросов логирования

        //=======================================================================================================================================

        private void Init()
        {
            if (use_logger)
            {
                Task.Factory.StartNew(() =>
                {
                    //Lazy
                    //Mutex 
                    while (true)
                    {
                        try
                        {
                            if (logQueue.Count() > 0)
                            {
                                var log_item = logQueue.Dequeue();
                                string file_name = log_item.file_name.Replace("/", "");
                                string directory_date = dateTimeHelper.DateToFolderName(log_item.date_time);
                                string separator = "";
                                if (log_item.use_separator)
                                {
                                    separator = " " + (char)13 + (char)10 + "[=========================================================================================================]";
                                }

                                bool exists = Directory.Exists(@"logs");
                                if (!exists) Directory.CreateDirectory(@"logs");

                                exists = Directory.Exists(@"logs/" + directory_date);

                                if (!exists) Directory.CreateDirectory(@"logs/" + directory_date);

                                using (StreamWriter file = new StreamWriter(@"logs/" + directory_date + "/" + file_name + ".txt", true))
                                {
                                    string tmptxt = String.Format("{0:[dd.MM.yy HH:mm:ss]} {1}", log_item.date_time, log_item.msg + separator);
                                    file.WriteLine(tmptxt);
                                    file.Close();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(50);
                    }
                });
            }
        }

        //=======================================================================================================================================
        public void Write(string msg, string file_name, bool use_separator = true)
        {
            if (use_logger)
            {
                var log_item = new QueeItem()
                {
                    date_time = DateTime.UtcNow.AddHours(dateTimeHelper.GetTimeZoneShift()),
                    msg = msg,
                    file_name = file_name,
                    use_separator = use_separator
                };
                logQueue.Enqueue(log_item);
            }
        }

        //=======================================================================================================================================
    }
}
