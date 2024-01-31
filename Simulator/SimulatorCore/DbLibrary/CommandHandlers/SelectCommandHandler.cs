using DbLibrary.DbInterfaces;
using DbLibrary.Reflection;
using SimulatorCore.Properties;
using System.Text;

namespace DbLibrary.CommandHandlers
{
    public class SelectCommandHandler<T> : CommandHandlerBase<T> where T : class, new()
    {
        public SelectCommandHandler() : base() {}

        public async Task<T> GetEntity(object primaryKey)
        {
            if (GetPrimaryKeyValue(primaryKey, out string value))
            {
                var commandText = GetCommandTextBase();
                commandText.Append($" WHERE {_primaryKeyName} = {value}");

                var entities = await ExecuteReader(commandText.ToString());
                if (entities.Count == 0)
                {
                    return null;
                }

                return entities[0];
            }
            throw new NotSupportedException("There is not primary key in table");
        }

        public async Task<List<T>> GetAllEntitiesFromTable()
        {
            var commandText = GetCommandTextBase();
            return await ExecuteReader(commandText.ToString());
        }

        private async Task<List<T>> ExecuteReader(string commandText)
        {
            List<T> entities = new List<T>();
            using (var command = new Command())
            {
                using (var reader = await command.ExecuteReader(commandText))
                {
                    while (reader.Read())
                    {
                        T entity = DbEntityFactory.CreateEntity<T>(reader);
                        entities.Add(entity);
                    }
                    return entities;
                }
            }
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"SELECT * FROM {DbConfigProperties.DatabaseName}.{_tableName}");
            return result;
        }
    }
}
