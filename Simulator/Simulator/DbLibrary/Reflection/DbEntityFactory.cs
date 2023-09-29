using GoodsDbLibrary.DbInterfaces;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace GoodsDbLibrary.Reflection
{
    internal static class DbEntityFactory
    {
        public static T CreateEntity<T>(SqlDataReader reader) where T : DbModel, new()
        {
            Type entityType = typeof(T);
            var properties = entityType.GetProperties();

            if(EqualTypes(reader, properties))
            {
                T entity = new();
                foreach(var property in properties)
                {
                    object propertyData = reader[property.Name];
                    property.SetValue(entity, propertyData);
                }
                return entity;
            }

            throw new NotSupportedException("Types are not equal");
        }

        private static bool EqualTypes(SqlDataReader reader, PropertyInfo[] properties)
        {
            var fields = GetFieldsNames(reader);
            var propertyNames = Properties.GetPropertiesNames(properties);

            bool equals = CompareProperties(propertyNames, fields);
            return equals;
        }


        private static string[] GetFieldsNames(SqlDataReader reader)
        {
            int fieldCount = reader.FieldCount;
            var fields = new string[fieldCount-1];

            // Скипаем поле ID, которое идёт первым
            for (int i = 1; i < fieldCount; i++)
            {
                // Но массив заполняется с нулевого индекса
                fields[i-1] = reader.GetName(i);
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
