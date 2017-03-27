using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace RPGManager.Extensions
{
    public static class ObjectExtensions
    {
        public static int? ToNullableInt(this object value)
        {
            if (value != Convert.DBNull)
            {
                return Convert.ToInt32(value);
            }

            return null;
        }

        public static Guid? ToNullableGuid(this object value)
        {
            if (value != Convert.DBNull)
            {
                return (Guid)value;
            }

            return null;
        }

        public static IEnumerable<SqlParameter> ToSqlParameters<T>(this T item, bool addPrefixToIdField = true)
        {
            HashSet<string> convertableTypes = new HashSet<string>
            {
                typeof(bool).Name, typeof(byte).Name, typeof(sbyte).Name, typeof(char).Name,
                typeof(decimal).Name, typeof(double).Name, typeof(float).Name, typeof(int).Name,
                typeof(uint).Name, typeof(long).Name, typeof(ulong).Name, typeof(object).Name,
                typeof(short).Name, typeof(ushort).Name, typeof(string).Name, typeof(Guid).Name,
                typeof(Nullable<>).Name
            };

            List<PropertyInfo> properties = item.GetType()
                .GetProperties()
                .Where(p => convertableTypes.Contains(p.PropertyType.Name))
                .ToList();

            return properties.Select(p => new SqlParameter("@" + (addPrefixToIdField && p.Name.ToLower() == "id" ? typeof(T).Name : String.Empty) + p.Name, p.GetValue(item) ?? DBNull.Value));
        }
    }
}
