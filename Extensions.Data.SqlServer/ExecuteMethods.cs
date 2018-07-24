using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Extensions.Data.SqlServer
{
    /// <summary>
    /// Class ExecuteMethods.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ExecuteMethods
    {

        /// <summary>
        /// Executes query against an Sql Server database and returns a dataset.
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">Any parameters required by the query</param>
        /// <returns>DataSet.</returns>
        public static DataSet ExecuteDataSet(this SqlConnection connection, string query, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter(query, connection))
            {
                da.SelectCommand.Parameters.AddRange(parameters);
                da.SelectCommand.CommandType = IsStoredProcedure(query);
                da.Fill(ds);
            }

            return ds;
        }


        /// <summary>
        /// Executes query against an Sql Server database and returns a data table.
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">Any parameters required by the query</param>
        /// <returns>DataTable.</returns>
        public static DataTable ExecuteDataTable(this SqlConnection connection, string query, params SqlParameter[] parameters)
        {
            DataSet ds = ExecuteDataSet(connection, query, parameters);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }


        /// <summary>
        /// Determines whether [is stored procedure] [the specified query].
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>CommandType.</returns>
        private static CommandType IsStoredProcedure(string query)
        {
            return query.Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
        }

    }
}