using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class AddQuantityGoodViewModel : BaseBoxViewModel
    {
        [ObservableProperty]
        private string filePath = "";
        [ObservableProperty]
        private string resultMessage = "";

        [ObservableProperty]
        private int goodID;
        [ObservableProperty]
        private int mFG_Date;
        [ObservableProperty]
        private int expired_Date;

        [ObservableProperty]
        private string[] arrItemName;

        public AddQuantityGoodViewModel(List<ShopItemModel> allItem)
        {
            arrItemName = allItem.Select(item => item.Name).ToArray();
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
            }
        }

        public override async void OKBehaviour()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            List<string> errorList = new List<string>();
            int numSuccess = 0;

            if (string.IsNullOrEmpty(FilePath))
            {
                DataTable dtfile = Utils.ReadDataFile(FilePath);

            }
            foreach (var dataRow in dt.Select())
            {
                var result = await dbManager.QueryItems.AddGood(
                    dataRow["id"].ToString(),
                    dataRow["itemid"].ToString(),
                    (DateOnly)dataRow["mfg_date"],
                    (DateOnly)dataRow["expired_date"]
                );

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
