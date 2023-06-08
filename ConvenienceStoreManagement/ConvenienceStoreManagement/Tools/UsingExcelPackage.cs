using ClosedXML.Excel;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ConvenienceStoreManagement.Tools
{
    public class UsingExcelPackage
    {
        public static string ReportFolder { get; private set; } = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ConvenienceStoreManagement\Reports\";

        public static bool ReadAndAddDataFile(DataTable result, string filePath, bool isExcel = true)
        {
            try
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

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static XLWorkbook WriteToSheet(XLWorkbook? wb, List<Dictionary<string, object>> listData, string sheetname)
        {
            if (wb == null)
            {
                wb = new XLWorkbook();
            }
            var dt = Utils.ToDataTable(listData);
            wb.Worksheets.Add(dt, sheetname);

            return wb;
        }

        public static string SaveWorkbook(XLWorkbook wb)
        {
            string path = ReportFolder + "\\MasterReport_" + DateTime.Now.ToString("yyyy-MM") + ".xlsx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            wb.SaveAs(path);
            return path;
        }
    }
}
