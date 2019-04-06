using API.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAgents
{
    public class AgentBase : IDisposable
    {
        private SqlConnection sqlConnection;
        private DataContext dataContext;
        private int dbConnTimeout = 60;

        /// <summary>
        /// Initializes a new agent instance
        /// </summary>
        public AgentBase()
        {
            OnInitializeSqlConnection(out sqlConnection);
            OnInitializeDataContext(out dataContext);
        }

        /// <summary>
        /// Initializes a new agent instance with the given SQL connection
        /// </summary>
        /// <param name="connection">SQL connection to use</param>
        public AgentBase(SqlConnection connection)
        {
            sqlConnection = connection;
            OnInitializeDataContext(out dataContext);
        }

        /// <summary>
        /// Initializes a new agent instance with the SQL connection from another agent
        /// </summary>
        /// <param name="agent">Agent where the SQL connection is retrived from</param>
        public AgentBase(AgentBase agent)
        {
            if (agent != null)
            {
                sqlConnection = agent.Connection;
            }
            else
            {
                OnInitializeSqlConnection(out sqlConnection);
            }

            OnInitializeDataContext(out dataContext);

            if (agent != null)
            {
                // keep original connection string
                dataContext.dbConnectionString = agent.dataContext.dbConnectionString;
            }
        }

        ~AgentBase()
        {
            Dispose();
        }

        public int DBConnTimeout
        {
            get { return dbConnTimeout; }
            set { dbConnTimeout = value; }
        }

        protected virtual void OnInitializeSqlConnection(out SqlConnection sqlConnection)
        {
            sqlConnection = GetConnection();
        }

        protected virtual void OnInitializeDataContext(out DataContext dataContext)
        {
            dataContext = new DataContext();
            dataContext.dbConnectionString = sqlConnection.ConnectionString;
        }

        public SqlConnection Connection
        {
            get
            {
                return (sqlConnection);
            }
        }

        public DataContext DataContext
        {
            get
            {
                return (dataContext);
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(GetConfiguration.GetAppSettings().Build().GetSection("ConnectionStrings").Get<string>());
        }
    }
}
