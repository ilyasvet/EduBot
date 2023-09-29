using GoodsDbLibrary.DbInterfaces;
using GoodsDbLibrary.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoodsDbLibrary.CommandHandlers
{
    public class InsertCommandHandler<T> : CommandHandlerBase<T> where T : DbModel, new()
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
            result.Append($"INSERT INTO {_tableName} ");

            var properties = Properties.GetPropertiesNames(typeof(T).GetProperties());
            string propertyString = Properties.GetPropertiesString(properties);
            result.Append($"({propertyString}) VALUES ");

            return result;
        }
    }
}
