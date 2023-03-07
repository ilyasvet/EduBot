using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TelegramBotLibrary
{
    public static class LocalSqlDbConnection
    {
        public static SqlConnection Connection { get; private set; }
        /// <summary>
        /// Создаёт экземпляр подключения к базе данных. Сразу открывает соединение.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dataBaseName"></param>
        static LocalSqlDbConnection()
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = ConfigurationManager.AppSettings["ServerName"],
                InitialCatalog = ConfigurationManager.AppSettings["DataBaseName"],
                IntegratedSecurity = true,
                ConnectTimeout = 15,
            };
            Connection = new SqlConnection(stringBuilder.ConnectionString);
            Connection.Open();
        }

        public static void Dispose()
        {
            Connection.Dispose();
        }
    }
}
