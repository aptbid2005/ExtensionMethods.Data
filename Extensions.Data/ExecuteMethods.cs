using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Extensions.Data
{

    /// <summary>
    /// Class ExecuteMethods.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class ExecuteMethods
    {

        /// <summary>
        /// Encapsulates ExecuteNonQuery code as an extension of the IDBConnection Class
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">Query to execute at the database</param>
        /// <param name="parameters">Any parameters needed for query to execute</param>
        public static int ExecuteNonQuery(this IDbConnection connection, string query, params IDataParameter[] parameters)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = IsStoredProcedure(query);
            foreach (IDataParameter param in parameters)
            {
                command.Parameters.Add(param);
            }

            return command.ExecuteNonQuery();
        }



        /// <summary>
        /// Encapsulates ExecuteScalar code as an extension of the IDBConnection Class
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">Query to execute at the database</param>
        /// <param name="parameters">Any parameters needed for query to execute</param>
        public static object ExecuteScalar(this IDbConnection connection, string query, params IDataParameter[] parameters)
        {
            //If query has a space in it, then it is not a stored procedure, it's a regular query!
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = IsStoredProcedure(query);
                foreach (IDataParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
                return command.ExecuteScalar();
            }
        }


        /// <summary>
        /// Encapsulates ExecuteDataReader code as an extension of the IDBConnection Class
        /// </summary>
        /// <typeparam name="T">Type you would like returned</typeparam>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">Query to execute at the database</param>
        /// <param name="dr">DataReader</param>
        /// <param name="parameters">Any parameters needed for query to execute</param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteDataReader<T>(this IDbConnection connection, string query, Func<IDataReader, T> dr, params IDataParameter[] parameters) where T : class
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = IsStoredProcedure(query);
                foreach (IDataParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
                using (IDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        yield return dr(rdr);
                    }
                    rdr.Close();
                }
            }

        }




        /// <summary>
        /// Encapsulates GetValue method as an extension of DataReader
        /// </summary>
        /// <typeparam name="T">Type you want returned</typeparam>
        /// <param name="rdr">DataReader</param>
        /// <param name="columnName">Column Name</param>
        public static T GetValue<T>(this IDataReader rdr, string columnName)
        {
            object data = rdr.GetValue(rdr.GetOrdinal(columnName));
            if (!string.IsNullOrEmpty(data.ToString()))
                return (T)data;

            return default(T);
        }




        private static CommandType IsStoredProcedure(string query)
        {
            return query.Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
        }



    }
}
