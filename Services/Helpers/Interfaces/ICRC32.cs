namespace KKT_APP_FA.Services.Helpers
{
    public interface ICRC32
    {
        string Crc(string group_code, string operation, string uuid);
    }
}