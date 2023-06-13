using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using MessageBox.Avalonia;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class DrawerShopItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool isClosed = true;

        [ObservableProperty]
        private string processResult = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UsageText))]
        private bool isAdd = true;

        [ObservableProperty]
        private string[] productTypes = new string[]
        {
            ProductType.SimpleProduct.ToString(),
            ProductType.NoScanProduct.ToString(),
            ProductType.WeightProduct.ToString(),
        };

        [ObservableProperty]
        private string selectedType;

        public string UsageText =>
            IsAdd ? "Add New Shop Item" : "Update Item";

        //Form Input
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ItemID))]
        private ShopItemModel? itemModel;

        public string ItemID => ItemModel != null ? ItemModel.UUID : "";
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        private string? imagePath;
        [ObservableProperty]
        private int price;

        private string? imageAbsPath;

        public DrawerShopItemViewModel() { }

        partial void OnItemModelChanged(ShopItemModel? value)
        {
            if (value != null)
            {
                Name = value.Name;
                ImagePath = value.ImagePath;
                Price = value.Price;
            }
            else
            {
                Name = "";
                ImagePath = null;
                Price = 0;
            }
        }

        [RelayCommand]
        public void ToggleDrawer(ShopItemModel? model = null)
        {
            IsClosed = !IsClosed;
            if (IsAdd) ClearItem();
            ItemModel = model;
            ProcessResult = "";
        }

        [RelayCommand]
        public void UpsertItem()
        {
            if (ItemModel == null) InsertNewItem();
            else UpdateChoosedItem();
        }

        [RelayCommand]
        public void ClearItem()
        {
            ItemModel = null;
            Name = "";
            ImagePath = null;
            Price = 0;
        }

        public async void InsertNewItem()
        {
            string pushImage = string.IsNullOrEmpty(imageAbsPath) ? "" : "storage://" + Name + ".png";
            int type = (int)(ProductType)Enum.Parse(typeof(ProductType), SelectedType);
            var result = await dbManager.QueryItems.AddItem(Name, pushImage, Price, type);
            result.ToSingle();

            if (result["error"] is object error)
            {
                ProcessResult = "Error Happened When Inserted";
                var errorBox = MessageBoxManager
                    .GetMessageBoxStandardWindow("Error While Adding Item", error.ToString());
                _ = errorBox.Show();
                return;
            };

            if (result["data"] is Dictionary<string, object> data)
            {
                ProcessResult = "Insert Success";
                CopyOverrideFile(data["name"].ToString());
                ItemModel = new(data);
            }
        }

        public async void UpdateChoosedItem()
        {
            string pushImage = string.IsNullOrEmpty(imageAbsPath) ? "" : "storage://" + Name + ".png";
            var result = await dbManager.QueryItems.UpdateItem(ItemID, Name, pushImage, Price);
            result.ToSingle();

            if (result["error"] is object error)
            {
                ProcessResult = "Error Happened When Updated";
                var errorBox = MessageBoxManager
                    .GetMessageBoxStandardWindow("Error While Update Item", error.ToString());
                _ = errorBox.Show();
                return;
            };

            if (result["data"] is Dictionary<string, object> data)
            {
                ProcessResult = "Update Success";
                CopyOverrideFile(data["name"].ToString());
                ItemModel = new(data);
            }
        }

        public void CopyOverrideFile(string itemName)
        {
            if (!string.IsNullOrEmpty(imageAbsPath))
            {
                string destFilePath = ItemManageViewModel.ImageFolder + itemName + ".png";
                File.Copy(imageAbsPath, destFilePath, true);
                imageAbsPath = null;
            }
        }

        public async void SetAbsoluteImagePath()
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filters.Add(new FileDialogFilter()
            {
                Name = "PNG",
                Extensions = { "png" }
            });
            fileDialog.Filters.Add(new FileDialogFilter()
            {
                Name = "JPG",
                Extensions = { "jpeg", "jpg" }
            });
            var result = await fileDialog.ShowAsync(ViewWindow);
            if (result != null)
            {
                imageAbsPath = result[0];
                ImagePath = imageAbsPath;
            }
        }
    }
}