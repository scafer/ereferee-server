using Dapper;
using API.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class DataContext
    {
        internal string dbConnectionString;

        public DataContext() { }

        /// <summary>
        /// Executes SQL insert commands directly on the database and returns the identity value
        /// </summary>
        /// <param name="query">The SQL insert command to be executed</param>
        /// <param name="parameters">The array of parameters to be passed to the command</param>
        /// <returns>An Identity object containing the identity value from the executed command</returns>
        public Identity ExecuteInsert(string command, params object[] parameters)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder(command);

            sql.AppendLine().Append("select convert( bigint, @@IDENTITY ) as Id");

            return (ExecuteQueryNoParameters<Identity>(sql.ToString())).FirstOrDefault();
        }

        public Identity ExecuteInsertFromQuery(string queryPath, params object[] parameters)
        {
            var query = GetScriptFromResources(queryPath);

            System.Text.StringBuilder sql = new System.Text.StringBuilder(query);

            sql.AppendLine().Append("select convert( bigint, @@IDENTITY ) as Id");

            if (parameters.Count() != 0)
            {
                DynamicParameters parameterArray = CreateParameterArray(parameters);

                return (ExecuteQueryWithParameters<Identity>(sql.ToString(), parameterArray)).FirstOrDefault();
            }
            else
            {
                return (ExecuteQueryNoParameters<Identity>(sql.ToString())).FirstOrDefault();
            }
        }

        public IEnumerable<TResult> ExecuteQuery<TResult>(string query, params object[] parameters)
        {
            query = GetScriptFromResources(query);

            if (parameters.Count() != 0)
            {
                DynamicParameters parameterArray = CreateParameterArray(parameters);

                return (ExecuteQueryWithParameters<TResult>(query, parameterArray));
            }
            else
            {
                return (ExecuteQueryNoParameters<TResult>(query));
            }
        }

        private IEnumerable<TResult> ExecuteQueryNoParameters<TResult>(string query)
        {
            using (var ctx = new SqlConnection(dbConnectionString))
            {
                return ctx.Query<TResult>(query).ToList();
            }
        }

        private IEnumerable<TResult> ExecuteQueryWithParameters<TResult>(string query, DynamicParameters parameters)
        {
            using (var ctx = new SqlConnection(dbConnectionString))
            {
                return ctx.Query<TResult>(NormalizeQuery(query, parameters), parameters).ToList();
            }
        }

        /// <summary>
        /// Executes SQL commands directly on the database
        /// </summary>
        /// <param name="command">The SQL command to be executed</param>
        /// <param name="parameters">The array of parameters to be passed to the command</param>
        /// <returns>An int representing the number of rows modified by the executed command</returns>
        public int ExecuteCommand(string command, params object[] parameters)
        {
            var query = GetScriptFromResources(command);

            if (!string.IsNullOrEmpty(query))
            {
                command = query;
            }

            if (parameters.Count() != 0)
            {
                DynamicParameters parametersArray = CreateParameterArray(parameters);

                return (ExecuteCommandWithParameters(NormalizeQuery(query, parametersArray), parametersArray));
            }

            return (ExecuteCommand(command));
        }

        private int ExecuteCommandWithParameters(string command, DynamicParameters parameters)
        {
            using (var ctx = new SqlConnection(dbConnectionString))
            {
                return ctx.Execute(NormalizeQuery(command, parameters), parameters);
            }
        }

        private int ExecuteCommand(string command)
        {
            using (var ctx = new SqlConnection(dbConnectionString))
            {
                return ctx.Execute(command);
            }
        }

        private bool AnyStructuredParameter(object[] parameters)
        {
            foreach (var p in parameters)
            {
                if (IsStructuredParameter(p))
                {
                    return (true);
                }
            }

            return (false);
        }

        private bool IsStructuredParameter(object p)
        {
            return ((p != null) && (typeof(IEnumerable<int>).IsAssignableFrom(p.GetType()) || typeof(IEnumerable<long>).IsAssignableFrom(p.GetType())));
        }

        private DynamicParameters CreateParameterArray(object[] parameters)
        {
            DynamicParameters listOfParameters = new DynamicParameters(parameters.Length);

            for (int idx = 0; idx < parameters.Length; idx++)
            {
                listOfParameters.Add(string.Format("@p{0}", idx), parameters[idx]);
            }

            return (listOfParameters);
        }

        protected string GetScriptFromResources(string queryPath)
        {
            string databaseScriptsPath = GetConfiguration.GetAppSettings().Build().GetSection("DatabaseScriptsPath").Get<string>();

            string sql = System.IO.File.ReadAllText(System.IO.Path.Combine(databaseScriptsPath, queryPath + ".sql"), Encoding.UTF8);
            return (sql);
        }

        private string NormalizeQuery(string query, DynamicParameters parameters)
        {
            string sql = query;
            bool containsHiddenCode = query.Contains("--@p");

            int objIdx = -1;
            foreach (var p in parameters.ParameterNames)
            {
                objIdx++;
                var value = parameters.Get<dynamic>("@p" + objIdx);
                sql = sql.Replace(string.Format("{{{0}}}", objIdx), "@p" + objIdx);

                if (containsHiddenCode && IsStructuredParameter(p) && (value.t != null))
                {
                    sql = sql.Replace(string.Format("--@p{0}:", objIdx), "");
                }
            }

            return (sql);
        }
    }
}
