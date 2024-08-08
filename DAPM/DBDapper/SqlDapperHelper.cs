using System.Data;
using Dapper;
using Npgsql;

namespace DBDapper
{
    /// <summary>
    /// Database operation class
    /// </summary>
    public class SqlDapperHelper : CommonDb
    {
        static int? commandTimeout = 30;
        public static IDbConnection GetConnection(string ConnType)
        {
            return new NpgsqlConnection(CreateConnectionString(ConnType));
        }

        /// <summary>
        /// Add.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Entity type instance.</param>
        /// <returns>int.</returns>
        public static int Add<T>(string ConnType, string sql, T t)
            where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Batch add.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Generic list instance.</param>
        /// <returns>int.</returns>
        public static int Add<T>(string ConnType, string sql, List<T> t)
            where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Delete.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Entity type instance.</param>
        /// <returns>int.</returns>
        public static int Delete<T>(string ConnType, string sql, T t)
              where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Batch delete.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Generic list instance.</param>
        /// <returns>int.</returns>
        public static int Delete<T>(string ConnType, string sql, List<T> t)
              where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Entity type instance.</param>
        /// <returns>int.</returns>
        public static int Update<T>(string ConnType, string sql, T t)
              where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Entity type instance.</param>
        /// <returns>int.</returns>
        public static int Update(string ConnType, string sql)
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// Batch update.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Generic list instance.</param>
        /// <returns>int.</returns>
        public static int Update<T>(string ConnType, string sql, List<T> t)
              where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Execute(sql, t);
            }
        }

        /// <summary>
        /// Query.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <returns>Generic list.</returns>
        public static List<T> QueryList<T>(string ConnType, string sql, T? t)
             where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Query<T>(sql, t).ToList();
            }
        }

        /// <summary>
        /// Query specific data.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <returns>Class instance.</returns>
        public static T Query<T>(string ConnType, string sql) where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Query<T>(sql).SingleOrDefault();
            }
        }
        /// <summary>
        /// Query specific data.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <param name="t">Generic list instance.</param>
        /// <returns>Class instance.</returns>
        public static T Query<T>(string ConnType, string sql, T t) where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Query<T>(sql, t).FirstOrDefault();
            }
        }

        /// <summary>
        /// Query with IN operation.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="sql">SQL statement to execute.</param>
        /// <returns>Generic list.</returns>
        public static List<T> Query<T>(string ConnType, string sql, int[] ids)
            where T : class
        {
            using (IDbConnection connection = GetConnection(ConnType))
            {
                return connection.Query<T>(sql, new { ids }).ToList();
            }
        }
    }
}
