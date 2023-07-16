using System;
using System.Data.SqlClient;

namespace DbBotLibrary
{
    internal sealed class Connection : IDisposable
    {
        private readonly SqlConnection _connection;

        public Connection(string serverName, string dataBaseName)
        {
            SqlConnectionStringBuilder stringBuilder = new()
            {
                IntegratedSecurity = true,
                DataSource = serverName,
                InitialCatalog = dataBaseName,
                ConnectTimeout = 10,
            };
            _connection = new SqlConnection(stringBuilder.ConnectionString);
            _connection.Open();
        }

        public void AttachToCommand(SqlCommand command)
        {
            command.Connection = _connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
