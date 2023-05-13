using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using MessageBox.Avalonia;
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

        partial void OnItemModelChanged(ShopItemModel? model)
        {
            if (model != null)
            {
                Name = model.Name;
                ImagePath = model.ImagePath;
                Price = model.Price;
            }
            else
            {
                Name = "";
                ImagePath = null;
                Price = 0;
            }
        }

        [RelayCommand]
        public void ToggleDrawer()
        {
            IsClosed = !IsClosed;
            if (IsAdd) ClearItem();
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
            var task = await dbManager.QueryItems.AddItem(Name, pushImage, Price);
            var result = task.ToSingle();

            if (result["error"] != null)
            {
                ProcessResult = "Error Happened When Inserted";
                var errorBox = MessageBoxManager
                    .GetMessageBoxStandardWindow("Error While Adding Item", result["error"].ToString());
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
            var task = await dbManager.QueryItems.UpdateItem(ItemID, Name, pushImage, Price);
            var result = task.ToSingle();

            if (result["error"] != null)
            {
                ProcessResult = "Error Happened When Updated";
                var errorBox = MessageBoxManager
                    .GetMessageBoxStandardWindow("Error While Update Item", result["error"].ToString());
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