namespace KKT_APP_FA.Services.Helpers
{
    public interface ILoggerHelper
    {
        void Write(object msg, string file_name, bool _use_separator = true);
        string BuildResponseString(object obj);
    }
}