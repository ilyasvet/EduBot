using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace DbBotLibrary
{
    public abstract class CommandTable
    {
        private static string _serverName;
        private static string _dataBaseName;
        public static void SetServerName(string serverName)
        {
            _serverName = serverName;
        }
        public static void SetDataBaseName(string dataBaseName)
        {
            _dataBaseName = dataBaseName;
        }
        protected async Task ExecuteNonQueryCommand(string commandText)
        {
            using (Connection connection = new(_serverName, _dataBaseName))
            {
                using (SqlCommand command = new(commandText))
                {
                    connection.AttachToCommand(command);
                    await command.ExecuteNonQueryAsync();
                }       
            }
        }
        protected async Task<object> ExecuteReaderCommand(string commandText, Func<SqlDataReader, object> func)
        {
            using (Connection connection = new(_serverName, _dataBaseName))
            {
                using (SqlCommand command = new(commandText))
                {
                    connection.AttachToCommand(command);
                    object result = null;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        result = func(reader);
                    }
                    if (result == null) throw new ArgumentException("not found value in database");
                    return result;
                }
            }
        }
    }
}