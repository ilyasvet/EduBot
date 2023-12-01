using System.Reflection;
using System.Text;

namespace DbLibrary.Reflection
{
    internal static class Properties
    {
        public static string GetPropertiesDataString<T>(T entity)
        {
            StringBuilder result = new StringBuilder();

            var propertiesDatabase = typeof(T).GetProperties().ToList();
            propertiesDatabase.RemoveAt(propertiesDatabase.Count-1);

            foreach (PropertyInfo property in propertiesDatabase)
            {
                PropertyToSQLString(property, property.GetValue(entity), result);

                result.Append(",");
            }

            result.Remove(result.Length - 1, 1); //Удалить последнюю запятую

            return result.ToString();
        }

        private static void PropertyToSQLString(PropertyInfo property, object? value, StringBuilder result)
        {
            if (value == null)
            {
                result.Append("null");
            }
            else if (property.PropertyType == typeof(string))
            {
                result.Append($"'{value}'");
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                string dateString = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                result.Append($"'{dateString}'");
            }
            else
            {
                result.Append(value);
            }
        }

        public static string[] GetPropertiesNames(PropertyInfo[] properties)
        {
            string[] propertiesNames = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                propertiesNames[i] = properties[i].Name;
            }
            return propertiesNames;
        }

        public static string GetPropertiesString(string[] properties)
        {
            StringBuilder result = new StringBuilder();
            foreach (string propertyName in properties)
            {
                result.Append(propertyName);
                result.Append(',');
            }
            result.Remove(result.Length - 1, 1); //Удалить последнюю запятую
            return result.ToString();
        }
    }
}
