using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
//using DapperExtensions;
using System.Data;
using System.Data.SqlClient;
//using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using KKT_APP_FA.Models.DB;
using Microsoft.Data.Sqlite;

namespace KKT_APP_FA.Services.DB
{
    public class BaseService : IBaseService
    {
        //=======================================================================================================================================
        public List<T> SendSQLQueryToList<T>(string sql, string connectionString) // Возвращает тип (напр. для select)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.Query<T>(sql).ToList();
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public T SendSQLQuery<T>(string sql, string connectionString) // Возвращает тип (напр. для select)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.Query<T>(sql).FirstOrDefault();
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public int SendSQLQuery(string sql, string connectionString) // Возвращает результат (напр. для update, insert)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.Execute(sql);
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public async Task<List<T>> SendSQLQueryToListAsync<T>(string sql, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = await db.QueryAsync<T>(sql) as List<T>;
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public async Task<T> SendSQLQueryAsync<T>(string sql, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var result = await db.QueryAsync<T>(sql) as List<T>;
                var res = result.FirstOrDefault();
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public async Task<int> SendSQLQueryAsync(string sql, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = await db.ExecuteAsync(sql);
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        //=======================================================================================================================================
        //=======================================================================================================================================
        public string Insert<T>(T obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.Insert<string, T>(obj);
                db.Close();
                db.Dispose();
                return res;//.ToString();
            }
        }

        //=======================================================================================================================================
        public async Task<string> InsertAsync<T>(T obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = await db.InsertAsync<Guid, T>(obj);
                db.Close();
                db.Dispose();
                return res.ToString();
            }
        }

        //=======================================================================================================================================
        public int Update<T>(T obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.Update<T>(obj);
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public async Task<int> UpdateAsync<T>(T obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = await db.UpdateAsync<T>(obj);
                db.Close();
                db.Dispose();
                return res;
            }
        }

        //=======================================================================================================================================
        public List<T> SelectList<T>(T obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.GetList<T>(obj);
                db.Close();
                db.Dispose();
                return res.ToList();
            }
        }

        //=======================================================================================================================================
        public List<T> Select<T>(object obj, string connectionString)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Open();
                var res = db.GetList<T>(obj);
                db.Close();
                db.Dispose();
                return res.ToList();
            }
        }

        //=======================================================================================================================================

        // Вывод строк для создания БД SQLite
        protected List<string> GetCreateSQLiteDBString(object obj)
        {
            List<string> result = new List<string>();
            List<string> indexes = new List<string>();

            string create_string = "CREATE TABLE ";
            var tablename = ("[" + obj.GetType().Name + "] ").Replace("Context", "");
            create_string += (tablename + "(");
            foreach (var p in obj.GetType().GetProperties())
            {
                create_string += ("" + (char)13 + (char)10 + "[" + p.Name + "] ");
                if (p.PropertyType == typeof(bool))
                    create_string += "BOOLEAN DEFAULT 'true' ";
                if (p.PropertyType == typeof(int))
                    create_string += "INTEGER DEFAULT '0' ";
                if (p.PropertyType == typeof(string))
                    create_string += "TEXT ";

                var atrs = p.CustomAttributes;
                if (atrs.Count() == 0)
                {
                    create_string += ("NULL ");
                }
                foreach (var a in atrs)
                {
                    if (a.AttributeType.Name == "KeyAttribute")
                    {
                        create_string += ("UNIQUE PRIMARY KEY ");
                    }
                    else if (a.AttributeType.Name == "RequiredAttribute")
                    {
                        create_string += ("NOT NULL ");
                    }
                    else if (a.AttributeType.Name == "IndexAttribute")
                    {
                        indexes.Add("CREATE INDEX [" + p.Name + "_idx] ON " + tablename + " ( [" + p.Name + "] DESC );");
                    }
                }

                //if (p.Name.ToLower() == "id")  result += ("UNIQUE NOT NULL PRIMARY KEY");
                //else result += ("NULL");

                create_string += ", ";
            }
            create_string = create_string.Remove(create_string.Length - 2);
            create_string += ("" + (char)13 + (char)10 + "); " + (char)13 + (char)10);

            result.Add(create_string);
            result.AddRange(indexes);

            return result;
        }

        //=======================================================================================================================================
        //=======================================================================================================================================
        //=======================================================================================================================================

        // Вывод строки с именами полей класса для SQL запроса в Dapper (НЕ НУЖНО)
        protected string GetFieldsToSQLString(object obj)
        {
            string result = "";
            var t = obj.GetType();
            foreach (var p in obj.GetType().GetProperties())
            {
                result += ("\"" + p.Name + "\", ");
            }
            result = result.Remove(result.Length - 2);
            return result;
        }

        //=======================================================================================================================================

        // Вывод строки со значениями полей класса для SQL запроса в Dapper (НЕ НУЖНО)
        protected string GetFieldsValuesToSQLString(object obj)
        {
            string result = "";
            var t = obj.GetType();
            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string)) // строковый тип
                {
                    try
                    {
                        result += "'" + (p.GetValue(obj).ToString() + "', ");
                    }
                    catch (Exception)
                    {
                        result += "'', "; // 
                    }

                }
                else
                {
                    try
                    {
                        var val = p.GetValue(obj).ToString();
                        if (val.ToLower() == "true") val = "1";
                        else val = "2";
                        result += (val + ", ");
                    }
                    catch (Exception)
                    {
                        result += "0, "; // 
                    }
                }
            }
            result = result.Remove(result.Length - 2);
            return result;
        }

        //=======================================================================================================================================

        // Вывод строК!!! со значениями полей класса для SQL запроса в Dapper (НЕ НУЖНО)
        protected string GetFieldsListValuesToSQLString<T>(List<T> objs)
        {
            string result = "";
            foreach (var obj in objs)
            {
                var res = "(" + GetFieldsValuesToSQLString(obj) + "), " + (char)13 + (char)10;
                result += res;
            }
            result = result.Remove(result.Length - 4);
            return result;
        }

        //=======================================================================================================================================

        // Автогенерация SQL строки для INSERT (одна запись) через Reflection (НЕ НУЖНО)
        protected string GenerateSingleInsertSQL(object obj)
        {
            string name = obj.GetType().Name.Replace("Context", "");
            string sql = "INSERT INTO " + name + " (" + GetFieldsToSQLString(obj) + ") VALUES (" + GetFieldsValuesToSQLString(obj) + ");";
            return sql;
        }
    }
}
