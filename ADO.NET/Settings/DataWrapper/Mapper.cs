using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    /// Helps in mapping raw SQL Row to CLR Object properties.
    internal class Mapper : IMapper
    {
        /// Reference to InstanceCreator class for creating default / singleton instances.
        private readonly InstanceCreator instanceCreator = InstanceCreator.Default;

        /// Gets the default / singleton reference for current (Mapper) class.
        public static Mapper Default => Singleton<Mapper>.Instance;

        /// Maps raw SQL Row to CLR Object.
        public T MapDataToObject<T>(SqlDataReader reader) where T : class, new()
        {
            var type = typeof(T);

            var properties = FindProperties(type);
            var instance = instanceCreator.CreateInstance(type);

            // parse for each column of current line
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i); // table column name id , name, email ..
                if (properties.ContainsKey(columnName))
                {
                    var property = properties[columnName];
                    var columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i); // column data value
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType; // type of property object "System.String" ...
                    var propertyValue = columnValue == null ? null : Convert.ChangeType(columnValue, propertyType); // convert column value to property value
                    if (propertyType == typeof(string) && propertyValue != null)
                    {
                        propertyValue = propertyValue.ToString().TrimEnd();
                    }

                    property.SetValue(instance, propertyValue); // set property value
                }
            }

            return (T)instance;
        }

        /// Finds the properties for a given type.
        private Dictionary<string, PropertyInfo> FindProperties(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();

            var propertiesInfos = type.GetRuntimeProperties();

            foreach (var property in propertiesInfos)
            {
                var columnName = property.Name;
                result[columnName] = property;
            }

            return result;
        }
    }
}
