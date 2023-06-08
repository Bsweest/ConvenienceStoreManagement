using ConvenienceStoreManagement.Components.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace ConvenienceStoreManagement.Tools
{
    public static class Utils
    {
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyymmdd");
        }

        public static int[] GetArrayIdFromListCart(ObservableCollection<CartItemViewModel> listCart)
        {
            int[] returnArray = Array.Empty<int>();
            foreach (var cart in listCart)
            {
                returnArray = returnArray.Concat(cart.GetAllGoodID()).ToArray();
            }

            return returnArray;
        }

        public static DateOnly GetDateOnlyFromDb(object d)
        {
            return DateOnly.FromDateTime(DateTime.Parse(d.ToString()));
        }

        public static DateTimeOffset DateOnlyToDTOS(DateOnly d)
        {
            var datetime = d.ToDateTime(new TimeOnly(0));
            return new DateTimeOffset(datetime);
        }

        public static DataTable ToDataTable(List<Dictionary<string, object>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0) return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, object> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
