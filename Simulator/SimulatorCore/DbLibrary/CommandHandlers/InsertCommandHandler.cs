using DbLibrary.Reflection;
using SimulatorCore.Properties;
using System.Text;

namespace DbLibrary.CommandHandlers
{
    public class InsertCommandHandler<T> : CommandHandlerBase<T> where T : new()
    {
        public InsertCommandHandler() : base() { }

        public async Task<int> AddEntity(T entity)
        {
            var commandText = GetCommandTextBase();
            commandText.Append($"({Properties.GetPropertiesDataString(entity)});");

            return await ExecuteNonQuery(commandText.ToString());
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"INSERT INTO {DbConfigProperties.DatabaseName}.{_tableName} ");

            var properties = Properties.GetPropertiesNames(typeof(T).GetProperties());
            string propertyString = Properties.GetPropertiesString(properties);
            result.Append($"({propertyString}) VALUES ");

            return result;
        }
    }
}
