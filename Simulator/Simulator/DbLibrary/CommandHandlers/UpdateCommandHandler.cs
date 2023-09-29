using GoodsDbLibrary.DbInterfaces;
using GoodsDbLibrary.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoodsDbLibrary.CommandHandlers
{
    public class UpdateCommandHandler<T> : CommandHandlerBase<T> where T : DbModel, new()
    {
        public UpdateCommandHandler() : base() { }

        public async Task<int> UpdateEntity(int id, T newEntity)
        {
            var commandText = GetCommandTextBase();
            commandText.Append($"{Properties.GetPropertiesDataString(newEntity)}) ");
            commandText.Append($"WHERE ID = {id}");

            return await ExecuteNonQuery(commandText.ToString());
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"UPDATE {_tableName} SET (");

            var properties = Properties.GetPropertiesNames(typeof(T).GetProperties());
            result.Append($"{Properties.GetPropertiesString(properties)}) = (");
            

            return result;
        }
    }
}
