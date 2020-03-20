using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.DB
{
    public interface IBaseService
    {
        int SendSQLQuery(string sql, string connectionString);
        T SendSQLQuery<T>(string sql, string connectionString);
        Task<int> SendSQLQueryAsync(string sql, string connectionString);
        Task<T> SendSQLQueryAsync<T>(string sql, string connectionString);
        List<T> SendSQLQueryToList<T>(string sql, string connectionString);
        Task<List<T>> SendSQLQueryToListAsync<T>(string sql, string connectionString);
    }
}