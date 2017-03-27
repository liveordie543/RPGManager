using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace RPGManager.Data
{
    public class DataRowHelper
    {
        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo propertyInfo = item.GetType().GetProperty(column.ColumnName);

                if (propertyInfo != null && row[column] != DBNull.Value)
                {
                    try
                    {
                        propertyInfo.SetValue(item, Convert.ChangeType(row[column], propertyInfo.PropertyType), null);
                    }
                    catch (Exception) { } //Ignore any exceptions. We'll handle special conversions on a case by case basis.
                }
            }
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();

            SetItemFromRow(item, row);

            return item;
        }

        public static List<T> CreateListFromTable<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(CreateItemFromRow<T>(row));
            }

            return list;
        }
    }
}
