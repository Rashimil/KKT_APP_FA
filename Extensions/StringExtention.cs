using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKT_APP_FA.Services.Helpers;

namespace KKT_APP_FA.Extensions
{
    public static class StringExtention // расширения для класса string
    { 
        //==============================================================================================================================================

        public static int TimestampToUnixtime(this string timestamp) // формат dd.MM.yyyy
        {
            DateTime dateTime;
            try
            {
                // случай dd.MM.yyyy
                dateTime = DateTime.ParseExact(timestamp, "dd.MM.yyyy", null);
            }
            catch (Exception)
            {
                // случай d.MM.yyyy
                dateTime = DateTime.ParseExact(timestamp, "d.MM.yyyy", null);
            }
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = dateTime - origin;
            try
            {
                return Convert.ToInt32(Math.Round(Math.Floor(diff.TotalSeconds), 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //==============================================================================================================================================
    }
}
