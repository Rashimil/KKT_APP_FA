using System;

namespace KKT_APP_FA.Services.Helpers
{
    public interface IDateTimeHelper
    {
        long ConvertToUnixTimeMilliseconds(DateTime date_time);
        int ConvertToUnixTimeSeconds(DateTime date_time);
        int ConvertToUnixTimeSeconds(string date_time);
        string DateToFolderName(DateTime dateTime);
        long GetCurrentDayToUnixTimeSeconds();
        int GetTimeZoneShift();
    }
}