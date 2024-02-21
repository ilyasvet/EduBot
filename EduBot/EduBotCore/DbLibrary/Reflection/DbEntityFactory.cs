using MySqlConnector;
using System.Reflection;

namespace DbLibrary.Reflection
{
    internal static class DbEntityFactory
    {
        public static T CreateEntity<T>(MySqlDataReader reader) where T : new()
        {
            Type entityType = typeof(T);
            var properties = entityType.GetProperties();

            if(EqualTypes(reader, properties))
            {
                T entity = new();
                foreach(var property in properties)
                {
                    object propertyData = reader[property.Name];
                    if (propertyData is DBNull)
                    {
                        property.SetValue(entity, null);
                    }
                    else
                    {
                        property.SetValue(entity, propertyData);
                    }
                }
                return entity;
            }

            throw new NotSupportedException("Types are not equal");
        }

        private static bool EqualTypes(MySqlDataReader reader, PropertyInfo[] properties)
        {
            var fields = GetFieldsNames(reader);
            var propertyNames = Properties.GetPropertiesNames(properties);

            bool equals = CompareProperties(propertyNames, fields);
            return equals;
        }


        private static string[] GetFieldsNames(MySqlDataReader reader)
        {
            int fieldCount = reader.FieldCount;
            var fields = new string[fieldCount];

            for (int i = 0; i < fieldCount; i++)
            {
                fields[i] = reader.GetName(i);
            }

            return fields;
        }

        private static bool CompareProperties(string[] properties, string[] fields)
        {
            if(properties.Length != fields.Length)
            {
                return false;
            }
            for (int i = 0; i < properties.Length; i++)
            {
                if (!properties[i].ToLower().Equals(fields[i].ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
