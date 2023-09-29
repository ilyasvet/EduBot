using GoodsDbLibrary.DbInterfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace GoodsDbLibrary.CommandHandlers
{
    public abstract class CommandHandlerBase<T> where T : DbModel, new()
    {
        protected readonly string? _tableName;

        protected CommandHandlerBase()
        {
            var attributes = typeof(T).GetCustomAttributes(false);
            _tableName = GetTableAttributeName(attributes);
        }

        protected abstract StringBuilder GetCommandTextBase();

        protected async Task<int> ExecuteNonQuery(string commandText)
        {
            using (var command = new Command())
            {
                return await command.ExecuteNonQuery(commandText);
            }
        }

        private string? GetTableAttributeName(object[] attributes)
        {
            foreach (var attribute in attributes)
            {
                if (attribute is TableAttribute tableAttribute)
                {
                    return tableAttribute.Name;
                }
            }
            throw new Exception("Type is not mapped with TableAttribute");
        }
    }
}
