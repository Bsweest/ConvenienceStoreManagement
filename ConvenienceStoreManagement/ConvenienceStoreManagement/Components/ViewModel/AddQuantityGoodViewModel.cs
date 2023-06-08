using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class AddQuantityGoodViewModel : BaseBoxViewModel
    {
        // Dt contains Goods To Add
        private DataTable dt = new();

        [ObservableProperty]
        private string filePath = "";

        private bool IsExcel = true;
        [ObservableProperty]
        private string resultMessage = "";

        [ObservableProperty]
        private int goodID;
        [ObservableProperty]
        private DateTimeOffset? mFG_Date;
        [ObservableProperty]
        private DateTimeOffset? expired_Date;

        [ObservableProperty]
        private List<ShopItemModel> allItem;
        [ObservableProperty]
        private ShopItemModel? selectedItem;

        public AddQuantityGoodViewModel(List<ShopItemModel> allItem)
        {
            AllItem = allItem;
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("itemid", typeof(string));
            dt.Columns.Add("mfg_date", typeof(DateTime));
            dt.Columns.Add("expired_date", typeof(DateTime));
        }

        [RelayCommand]
        public async void ChooseFileToProcess()
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filters.Add(new FileDialogFilter()
            {
                Name = "Excel",
                Extensions = { "xlsx", "xlsm", "xlsb", "xls", "xlt" }
            });
            fileDialog.Filters.Add(new FileDialogFilter()
            {
                Name = "Csv",
                Extensions = { "csv" }
            });
            var result = await fileDialog.ShowAsync(ViewWindow);
            if (result != null)
            {
                FilePath = result[0];
                IsExcel = Path.GetExtension(FilePath) != ".csv";
            }
        }

        public override async void OKBehaviour()
        {
            dt.Rows.Clear();
            List<string> errorList = new();
            int numSuccess = 0;

            if (GoodID != 0 && SelectedItem != null && MFG_Date != null && Expired_Date != null)
            {
                dt.Rows.Add(
                    GoodID, SelectedItem.UUID,
                    ((DateTimeOffset)MFG_Date).Date,
                    ((DateTimeOffset)Expired_Date).Date
                );
            }

            if (!string.IsNullOrEmpty(FilePath))
            {
                var readSuccess = UsingExcelPackage.ReadAndAddDataFile(dt, FilePath, IsExcel);
                if (!readSuccess)
                {
                    ResultMessage = "You're opening the excel file!";
                    return;
                }
            }

            foreach (var dataRow in dt.Select())
            {
                var result = await dbManager.QueryItems.AddGood(
                    (int)dataRow["id"],
                    new Guid(dataRow["itemid"].ToString()),
                    DateOnly.FromDateTime((DateTime)dataRow["mfg_date"]),
                    DateOnly.FromDateTime((DateTime)dataRow["expired_date"])
                );
                result.ToSingle();

                if (result["error"] is object error)
                {
                    errorList.Add(dataRow["id"].ToString() + ": " + error.ToString());
                };

                if (result["data"] is Dictionary<string, object> data)
                {
                    numSuccess++;
                }
            }
            ResultMessage = $"Success Add {numSuccess} good and Fail {errorList.Count} times";
        }
    }
}
