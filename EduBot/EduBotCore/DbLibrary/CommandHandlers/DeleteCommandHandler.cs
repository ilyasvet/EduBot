using System.Text;

namespace DbLibrary.CommandHandlers
{
    public class DeleteCommandHandler<T> : CommandHandlerBase<T> where T : new()
    {
        public DeleteCommandHandler() : base() { }

        public async Task<int> DeleteEntity(object primaryKey)
        {
            if (GetPrimaryKeyValue(primaryKey, out string value))
            {
                var commandText = GetCommandTextBase();
                commandText.Append($"WHERE {_primaryKeyName} = {value}");

                return await ExecuteNonQuery(commandText.ToString());
            }
            throw new NotSupportedException("There is not primary key in table");
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"DELETE FROM {_tableName} ");
            return result;
        }
    }
}
