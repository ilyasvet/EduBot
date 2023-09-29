using GoodsDbLibrary.DbInterfaces;
using System.Text;
using System.Threading.Tasks;

namespace GoodsDbLibrary.CommandHandlers
{
    public class DeleteCommandHandler<T> : CommandHandlerBase<T> where T : DbModel, new()
    {
        public DeleteCommandHandler() : base() { }

        public async Task<int> DeleteEntity(int id)
        {
            var commandText = GetCommandTextBase();
            commandText.Append($"WHERE ID = {id}");

            return await ExecuteNonQuery(commandText.ToString());
        }

        protected override StringBuilder GetCommandTextBase()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"DELETE FROM {_tableName} ");
            return result;
        }
    }
}
