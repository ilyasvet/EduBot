using System;
using System.Data.SqlClient;

namespace GoodsDbLibrary.DbInterfaces
{
    internal partial class Command : IDisposable
    {
        private class Connection : IDisposable
        {
            private readonly static string _connectionString;

            static Connection()
            {
                SqlConnectionStringBuilder builder = new()
                {
                    IntegratedSecurity = true,
                    DataSource = DbConfig.serverName,
                    InitialCatalog = DbConfig.dataBaseName,
                    ConnectTimeout = 10,
                };

                _connectionString = builder.ToString();
            }

            private readonly SqlConnection _connection;

            public Connection()
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            public void ConnectWithCommand(SqlCommand sqlCommand)
            {
                sqlCommand.Connection = _connection;
            }

            public void Dispose()
            {
                _connection?.Dispose();
            }
        }
    }
}
