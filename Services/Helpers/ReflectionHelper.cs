using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.Helpers
{
    // Работа с классами череез Reflection
    public class ReflectionHelper
    {
        //=======================================================================================================================================

        // Вывод списка с ИМЕНАМИ полей класса 
        public List<string> GetClassFieldsNames(object obj)
        {
            List<string> result = new List<string>();
            var t = obj.GetType();
            foreach (var p in obj.GetType().GetProperties())
            {
                result.Add(p.Name);
            }
            return result;
        } 

        //=======================================================================================================================================

        // Вывод списка со ЗНАЧЕНИЯМИ полей определенного типа T
        public List<T> GetClassFieldsValues<T>(object obj)
        {
            //Enum.GetValues(typeof(Season)
            List<T> result = new List<T>();
            var t = obj.GetType();
            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(T)) 
                {
                    try
                    {
                        result.Add((T)p.GetValue(obj));
                    }
                    catch (Exception)
                    {
                    }

                }
            }
            return result;
        } 

        //=======================================================================================================================================
    }
}
