using Avalonia.Controls;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class CreateReportViewModel : PageViewModelBase
    {
        public enum ReportSheet
        {
            Product,
            Employee,
            Customer,
        }

        [ObservableProperty]
        private DateTimeOffset? choosedMonth = null;

        [ObservableProperty]
        private string _reportPath = string.Empty;

        public async Task<Dictionary<ReportSheet, List<Dictionary<string, object>>>?> RetrieveData()
        {
            if (ChoosedMonth == null) return null;
            var firstMonthDate = ((DateTimeOffset)ChoosedMonth).Date.ToString("yyyy-MM-") + "01";

            var data = new Dictionary<ReportSheet, List<Dictionary<string, object>>>();

            // Get Product Report
            var task1 = await dbManager.QueryReport
                .ReportForProductSales(firstMonthDate);
            if (task1["error"] is object error1)
            {
                dbManager.Notify(error1.ToString(), true);
                return null;
            }

            if (task1["data"] is List<Dictionary<string, object>> data1)
            {
                data.Add(ReportSheet.Product, data1);
            }

            // Get Employee Report
            var task2 = await dbManager.QueryReport
                .ReportForEmployee(firstMonthDate);

            if (task2["error"] is object error2)
            {
                dbManager.Notify(error2.ToString(), true);
                return null;
            }

            if (task2["data"] is List<Dictionary<string, object>> data2)
            {
                data.Add(ReportSheet.Employee, data2);
            }

            // Get Employee Report
            var task3 = await dbManager.QueryReport
                .ReportForCustomer(firstMonthDate);

            if (task3["error"] is object error3)
            {
                dbManager.Notify(error3.ToString(), true);
                return null;
            }

            if (task3["data"] is List<Dictionary<string, object>> data3)
            {
                data.Add(ReportSheet.Customer, data3);
            }

            return data;
        }

        [RelayCommand]
        public async void GenerateReport()
        {
            var data = await RetrieveData();
            if (data == null) return;

            using var wb = new XLWorkbook();
            {
                foreach (var item in data.Keys)
                {
                    UsingExcelPackage.WriteToSheet(wb, data[item], item.ToString());
                }
                ReportPath = UsingExcelPackage.SaveWorkbook(wb);
            }
        }

        [RelayCommand]
        public void OpenReportFolder()
        {
            var path = UsingExcelPackage.ReportFolder;
            Directory.CreateDirectory(path);
            Process.Start("explorer", path);
        }

        public override CreateReportViewModel SetViewWindow(Window viewWindow)
        {
            return base.SetViewWindow(viewWindow) as CreateReportViewModel;
        }

        public override CreateReportViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as CreateReportViewModel;
        }

        public override CreateReportViewModel FinishBuild()
        {
            return base.FinishBuild() as CreateReportViewModel;
        }
    }
}
