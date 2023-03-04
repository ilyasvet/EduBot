using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TelegramBotLibrary
{
    public abstract class AbstractSqlCommand : IDisposable
    {
        protected SqlCommand _command = new SqlCommand();

        public AbstractSqlCommand(LocalSqlDbConnection localConnection)
        {
            _command.Connection = localConnection.Connection;
        }
        protected void ExecuteNonQueryCommand(string commandText)
        {
            _command.CommandText = commandText;
            _command.ExecuteNonQuery();
        }
        public void Dispose()
        {
            _command.Dispose();
        }
    }
}
