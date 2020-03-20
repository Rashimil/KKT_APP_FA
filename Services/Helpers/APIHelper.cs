using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    public class APIHelper
    {
        IConfiguration configuration;
        string connectionstring;

        //=======================================================================================================================================
        public APIHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionstring = this.configuration.GetSection("ConnectionStrings")["MainAppConnectionString"];
        }

        //=======================================================================================================================================
        public string POST_to_main_app(string RequestText) // POST отправка ответа на задание на MAIN-APP
        {
            try
            {
                WebRequest Request = WebRequest.Create(connectionstring);
                Request.Method = "POST";
                string postData = RequestText;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                Request.ContentType = "application/json";
                Request.ContentLength = byteArray.Length;
                Stream dataStream = Request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                WebResponse Response = Request.GetResponse() as HttpWebResponse;
                Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
                dataStream = Response.GetResponseStream();
                StreamReader Reader = new StreamReader(dataStream);
                string responseFromServer = Reader.ReadToEnd();
                Reader.Close();
                dataStream.Close();
                dataStream.Flush();
                Response.Close();
                return responseFromServer; //Возвращаем респонс
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //=======================================================================================================================================
    }
}
