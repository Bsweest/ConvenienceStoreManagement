using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ItemManageViewModel : PageViewModelBase
    {
        public static string ImageFolder { get; private set; } = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ConvenienceStoreManagement\ItemImages\";

        [ObservableProperty]
        private DrawerShopItemViewModel drawerItemVM = new();

        [ObservableProperty]
        private string choosedFolder = "";

        [ObservableProperty]
        private List<GroupItemViewModel>? gridItems = null;
        [ObservableProperty]
        private GroupItemViewModel? selectedItem;

        private readonly List<ShopItemModel> AllShopItem = new();
        private readonly List<GroupItemViewModel> ListBaseGroupItem = new();

        [ObservableProperty]
        private DebounceSearch debounceSearchItem;

        public ItemManageViewModel()
        {
            //Create Working Folder
            Directory.CreateDirectory(ImageFolder);

            ChoosedFolder = ImageFolder;
            var filterKeys = new string[] { "ID", "Name" };
            debounceSearchItem = new(FilterShopItem, filterKeys, 1);
        }
        public override ItemManageViewModel SetViewWindow(Window viewWindow)
        {
            return base.SetViewWindow(viewWindow) as ItemManageViewModel;
        }

        public override ItemManageViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as ItemManageViewModel;
        }

        public override ItemManageViewModel FinishBuild()
        {
            DrawerItemVM.SetViewWindow(ViewWindow).SetDatabaseConnection(dbManager);
            GetAllShopItem();
            return this;
        }

        private void FilterShopItem(string queryParam, string selectedFilter)
        {
            selectedItem = null;
            if (queryParam == "")
            {
                GridItems = ListBaseGroupItem;
            }
            else if (queryParam == "*:expiry")
            {
                var filterdItems = ListBaseGroupItem
                    .Where(x =>
                        x.Goods[0].ExpiredDate == DateOnly.FromDateTime(DateTime.Now)
                    ).ToList();
                GridItems = filterdItems;
            }
            else
            {
                var filteredItems = ListBaseGroupItem
                    .Where(x => x.Item.GetProperty(selectedFilter).ToLower().Contains(queryParam.ToLower()))
                    .ToList();
                GridItems = filteredItems;
            }
        }

        [RelayCommand]
        public async void GetAllShopItem()
        {
            var results = await dbManager.QueryItems.GetAllItem();

            if (results["data"] is List<Dictionary<string, object>> listData)
            {
                AllShopItem.Clear();
                ListBaseGroupItem.Clear();
                foreach (var item in listData)
                {
                    ShopItemModel shopItem = new(item);
                    GroupItemViewModel grpItem = new(shopItem);

                    var taskGoods = await dbManager.QueryItems.GetRemainingGood(new Guid(shopItem.UUID));
                    if (taskGoods["data"] is List<Dictionary<string, object>> listGood)
                    {
                        foreach (var good in listGood)
                        {
                            var newGood = new GoodModel(shopItem, good);
                            grpItem.AddGood(newGood);
                        }
                    }

                    AllShopItem.Add(shopItem);
                    ListBaseGroupItem.Add(grpItem);
                }
            }
            GridItems = null;
            GridItems = ListBaseGroupItem;
        }

        [RelayCommand]
        public void AddQuantity()
        {
            if (dbManager == null) throw new Exception("Db Connection not found!");
            AddQuantityGood.ShowBox(AllShopItem, dbManager);
        }

        [RelayCommand]
        public void OpenDrawerAddItem()
        {
            DrawerItemVM.IsAdd = true;
            DrawerItemVM.ToggleDrawer();
        }

        [RelayCommand]
        public void OpenDrawerUpdateItem()
        {
            if (SelectedItem == null) return;
            DrawerItemVM.IsAdd = false;
            DrawerItemVM.ToggleDrawer(SelectedItem.Item);
        }

        [RelayCommand]
        public void CheckExpiry()
        {
            DebounceSearchItem.QueryParam = "*:expiry";
        }
    }
}
