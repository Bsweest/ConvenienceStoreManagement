using ConvenienceStoreManagement.Components.ViewModel;
using ExcelDataReader;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;

namespace ConvenienceStoreManagement.Tools
{
    public static class Utils
    {
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyymmdd");
        }

        public static void ReadAndAddDataFile(DataTable result, string filePath, bool isExcel = true)
        {
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using IExcelDataReader reader = isExcel
                ? ExcelReaderFactory.CreateReader(stream)
                : ExcelReaderFactory.CreateCsvReader(stream);
            DataSet dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            foreach (DataTable dt in dataSet.Tables)
            {

                result.Merge(dt, true, MissingSchemaAction.Ignore);
            }
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
    }
}
