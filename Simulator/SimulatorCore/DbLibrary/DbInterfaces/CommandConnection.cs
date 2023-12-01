using MySqlConnector;

namespace DbLibrary.DbInterfaces
{
    internal partial class Command : IDisposable
    {
        private class Connection : IDisposable
        {
            private readonly static string _connectionString;

            static Connection()
            {
                MySqlConnectionStringBuilder builder = new()
                {
                    UserID = DbConfigProperties.UserName,
                    Password = DbConfigProperties.Password,
                    Server = DbConfigProperties.ServerName,
                    Port = uint.Parse(DbConfigProperties.Port),
                    Database = DbConfigProperties.DatabaseName,
                };

                _connectionString = builder.ToString();
            }

            private readonly MySqlConnection _connection;

            public Connection()
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }

            public void ConnectWithCommand(MySqlCommand sqlCommand)
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
