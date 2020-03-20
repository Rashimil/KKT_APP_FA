using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKT_APP_FA.Helpers
{
    // Работа с KKTResponse
    public class KKTResponseHelper
    {
        //==============================================================================================================================================

        // Построение строки ответа по модели
        public string BuildResponseString(object obj)
        {
            string separator = "\t---" + Environment.NewLine;
            string result = "";
            string result_footer = "";
            var t = obj.GetType();
            result += t.Name.Replace("Response", "") + Environment.NewLine + separator;

            foreach (var p in obj.GetType().GetProperties())
            {
                var n = p.Name;
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

        //==============================================================================================================================================
    }
}
