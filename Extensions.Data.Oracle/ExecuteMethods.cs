// ***********************************************************************
// Assembly         : Extensions.Data.Oracle
// Author           : Jason Nowicki
// Created          : 06-25-2018
//
// Last Modified By : Jason Nowicki
// Last Modified On : 06-25-2018
// ***********************************************************************
// <copyright file="ExecuteMethods.cs" company="EECPPC KY.gov">
//     Copyright ©  2018
// </copyright>
// ***********************************************************************
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Oracle.DataAccess.Client;

namespace Extensions.Data.Oracle
{
    /// <summary>
    /// Class ExecuteMethods.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ExecuteMethods
    {

        /// <summary>
        /// Executes query against an Oracle database and returns a dataset.
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">Any parameters required by the query</param>
        /// <returns>DataSet.</returns>
        public static DataSet ExecuteDataSet(this OracleConnection connection, string query, params OracleParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (OracleDataAdapter da = new OracleDataAdapter(query, connection))
            {
                da.SelectCommand.Parameters.AddRange(parameters);
                da.SelectCommand.CommandType = IsStoredProcedure(query);
                da.Fill(ds);
            }

            return ds;
        }


        /// <summary>
        /// Executes query against an Oracle database and returns a data table.
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">Any parameters required by the query</param>
        /// <returns>DataTable.</returns>
        public static DataTable ExecuteDataTable(this OracleConnection connection, string query, params OracleParameter[] parameters)
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