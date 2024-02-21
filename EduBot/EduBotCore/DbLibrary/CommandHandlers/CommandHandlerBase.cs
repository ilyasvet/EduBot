using DbLibrary.DbInterfaces;
using DbLibrary.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbLibrary.CommandHandlers
{
    public abstract class CommandHandlerBase<T> where T : new()
    {
        protected readonly string? _tableName;
        protected string? _primaryKeyName;
        protected Type? _primaryKeyType;

        protected CommandHandlerBase()
        {
            var attributes = typeof(T).GetCustomAttributes(false);
            _tableName = GetTableAttributeName(attributes);
            GetPrimaryKeyName(attributes);
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
        private void GetPrimaryKeyName(object[] attributes)
        {
            foreach (var attribute in attributes)
            {
                if (attribute is PrimaryKeyAttribute primaryKeyAttribute)
                {
                    _primaryKeyName = primaryKeyAttribute.Name;
                    _primaryKeyType = primaryKeyAttribute.Type;
                }
            }
        }
        protected bool GetPrimaryKeyValue(object value, out string result)
        {
            result = string.Empty;
            if(_primaryKeyType != null)
            {
                if(_primaryKeyType == typeof(long))
                {
                    result = ((long)value).ToString();
                }
                else if(_primaryKeyType == typeof(string))
                {
                    result = $"\'{(string)value}'";
                }
                return true;
            }
            return false;
        }
    }
}
