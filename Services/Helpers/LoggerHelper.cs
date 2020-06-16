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
                                //string directory_date = dateTimeHelper.DateToFolderName(log_item.date_time);
                                var date = dateTimeHelper.DateToFolders(log_item.date_time);
                                string separator = "";
                                if (log_item.use_separator)
                                {
                                    separator = " " + (char)13 + (char)10 + "[=========================================================================================================]";
                                }

                                bool exists = Directory.Exists(@"logs");
                                if (!exists) Directory.CreateDirectory(@"logs");

                                //exists = Directory.Exists(@"logs/" + directory_date);
                                //if (!exists) Directory.CreateDirectory(@"logs/" + directory_date);

                                exists = Directory.Exists(@"logs/" + date.Year);
                                if (!exists) Directory.CreateDirectory(@"logs/" + date.Year);

                                exists = Directory.Exists(@"logs/" + date.Year + "/" + date.Month);
                                if (!exists) Directory.CreateDirectory(@"logs/" + date.Year + "/" + date.Month);

                                exists = Directory.Exists(@"logs/" + date.Year + "/" + date.Month + "/" + date.Day);
                                if (!exists) Directory.CreateDirectory(@"logs/" + date.Year + "/" + date.Month + "/" + date.Day);

                                //using (StreamWriter file = new StreamWriter(@"logs/" + directory_date + "/" + file_name + ".txt", true))
                                using (StreamWriter file = new StreamWriter(@"logs/" + date.Year + "/" + date.Month + "/" + date.Day + "/" + file_name + ".txt", true))
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
        public void Write(object msg, string file_name, bool use_separator = true)
        {
            if (use_logger)
            {
                string _msg;
                if (msg.GetType() == typeof(string))
                {
                    _msg = msg.ToString();
                }
                else
                {
                    _msg = BuildResponseString(msg); // парсим поля обьекта
                }
                var log_item = new QueeItem()
                {
                    date_time = DateTime.UtcNow.AddHours(dateTimeHelper.GetTimeZoneShift()),
                    msg = _msg,
                    file_name = file_name,
                    use_separator = use_separator
                };
                logQueue.Enqueue(log_item);
            }
        }

        //=======================================================================================================================================

        // Построение блока строк для лога по модели:
        public string BuildResponseString(object obj)
        {
            string separator = "\t---" + Environment.NewLine;
            string result = "";
            string result_footer = "";
            var t = obj.GetType();
            result += t.Name.Replace("Response", "") + Environment.NewLine + separator;

            foreach (var p in obj.GetType().GetProperties())
            {
                var attributes = p.CustomAttributes.ToList();

                var TAGAttr = attributes.Where(c => c.AttributeType.Name.ToLower() == "tagattribute").FirstOrDefault(); // атрибут Tag
                var descriptionAttr = attributes.Where(c => c.AttributeType.Name.ToLower() == "descriptionattribute").FirstOrDefault(); // атрибут Description

                string descr = (descriptionAttr != null) ? descriptionAttr.ConstructorArguments[0].Value.ToString() : "";  // значение атрибута Description                    
                int tag = (TAGAttr != null) ? (int)TAGAttr.ConstructorArguments[0].Value : 0; // значение атрибута Tag

                string n = string.IsNullOrEmpty(descr) ? "[" + p.Name + "] " : "[" + descr + "] "; // Наименование свойства

                if (n == "Result" || n == "ErrorCode" || n == "Description") // случай BaseResponse
                {
                    string val;
                    try
                    {
                        val = p.GetValue(obj).ToString();
                    }
                    catch (Exception)
                    {
                        val = "";
                    }
                    result += ("\t" + n + ": " + val + Environment.NewLine);
                }
                else
                {
                    string val;
                    try
                    {
                        val = p.GetValue(obj).ToString();
                    }
                    catch (Exception)
                    {
                        val = "";
                    }
                    result_footer += ("\t" + n + ": " + val + Environment.NewLine);
                }
            }
            result += separator;
            result += result_footer;
            result += separator;
            return result;
        }


        //=======================================================================================================================================     
    }
}
