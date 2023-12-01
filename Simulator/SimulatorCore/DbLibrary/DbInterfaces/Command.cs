using MySqlConnector;

namespace DbLibrary.DbInterfaces
{
    internal partial class Command : IDisposable
    {
        private readonly MySqlCommand _command;
        private readonly Connection _connection;

        public Command()
        {
            _command = new MySqlCommand();
            _connection = new Connection();
            _connection.ConnectWithCommand(_command);
        }

        public async Task<MySqlDataReader> ExecuteReader(string commandText)
        {
            _command.CommandText = commandText;
            return await _command.ExecuteReaderAsync();
        }

        public async Task<int> ExecuteNonQuery(string commandText)
        {
            _command.CommandText = commandText;
            return await _command.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            _command?.Dispose();
            _connection?.Dispose();
        }
    }
}
