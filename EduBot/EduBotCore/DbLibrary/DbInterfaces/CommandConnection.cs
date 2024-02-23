using MySqlConnector;
using EduBotCore.Properties;

namespace DbLibrary.DbInterfaces
{
    internal partial class Command : IDisposable
    {
        private class Connection : IDisposable
        {
            private readonly static string _connectionString;

            static Connection()
            {
                string Server = DbConfigProperties.Server;
                string DatabaseName = DbConfigProperties.DatabaseName;
				string UserName = DbConfigProperties.UserName;
				string Password = DbConfigProperties.Password;

                if (DbConfigProperties.NeedPort == "true")
                {
                    int Port = int.Parse(DbConfigProperties.Port);
                    _connectionString =
                    $"Server={Server}; database={DatabaseName}; UID={UserName}; password={Password}; port={Port}";
                }
                else
                {
					_connectionString =
					$"Server={Server}; database={DatabaseName}; UID={UserName}; password={Password}";
				}
                
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
