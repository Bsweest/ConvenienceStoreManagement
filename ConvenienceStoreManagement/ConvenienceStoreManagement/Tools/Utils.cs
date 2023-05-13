using ExcelDataReader;
using System;
using System.Data;
using System.IO;

namespace ConvenienceStoreManagement.Tools
{
    public static class Utils
    {
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyymmdd");
        }

        public static DataTable ReadDataFile(string filePath, bool isExcel = true)
        {
            DataTable result = new();
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = isExcel
                ? ExcelReaderFactory.CreateReader(stream)
                : ExcelReaderFactory.CreateCsvReader(stream);
            do
            {
                while (reader.Read())
                {
                    // Read Only 1 Sheet
                    result.Load(reader);
                    return result;
                }
            } while (reader.NextResult());

            return result;
        }
    }
}
