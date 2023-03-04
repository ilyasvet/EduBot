using System;
using System.Data.SqlClient;

namespace TelegramBotLibrary
{
    public class LocalSqlDbConnection : IDisposable
    {
        public SqlConnection Connection { get; private set; }
        /// <summary>
        /// Создаёт экземпляр подключения к базе данных. Сразу открывает соединение.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dataBaseName"></param>
        public LocalSqlDbConnection(string serverName, string dataBaseName)
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = dataBaseName,
                IntegratedSecurity = true,
                ConnectTimeout = 15,
            };
            Connection = new SqlConnection(stringBuilder.ConnectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
