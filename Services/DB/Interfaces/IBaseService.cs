using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKT_APP_FA.Services.DB
{
    public interface IBaseService
    {
        string Insert<T>(T obj, string connectionString);
        Task<string> InsertAsync<T>(T obj, string connectionString);
        List<T> Select<T>(object obj, string connectionString);
        List<T> SelectList<T>(T obj, string connectionString);
        int SendSQLQuery(string sql, string connectionString);
        T SendSQLQuery<T>(string sql, string connectionString);
        Task<int> SendSQLQueryAsync(string sql, string connectionString);
        Task<T> SendSQLQueryAsync<T>(string sql, string connectionString);
        List<T> SendSQLQueryToList<T>(string sql, string connectionString);
        Task<List<T>> SendSQLQueryToListAsync<T>(string sql, string connectionString);
        int Update<T>(T obj, string connectionString);
        Task<int> UpdateAsync<T>(T obj, string connectionString);
    }
}