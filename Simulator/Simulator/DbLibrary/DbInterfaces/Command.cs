using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GoodsDbLibrary.DbInterfaces
{
    internal partial class Command : IDisposable
    {
        private readonly SqlCommand _command;
        private readonly Connection _connection;

        public Command()
        {
            _command = new SqlCommand();
            _connection = new Connection();
            _connection.ConnectWithCommand(_command);
        }

        public async Task<SqlDataReader> ExecuteReader(string commandText)
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
