using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ItemManageViewModel : PageViewModelBase
    {
        public static string ImageFolder { get; private set; } = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ConvenienceStoreManagement\";

        [ObservableProperty]
        private DrawerShopItemViewModel drawerItemVM = new();

        [ObservableProperty]
        private string choosedFolder = "";

        [ObservableProperty]
        private List<GroupItemViewModel> gridItems = new();

        private List<GroupItemViewModel> ListBaseGroupItem = new();
        private List<ShopItemModel> AllShopItem = new();

        [ObservableProperty]
        private GroupItemViewModel? selectedItem;

        public ItemManageViewModel()
        {
            ChoosedFolder = ImageFolder;
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

                    var taskGoods = await dbManager.QueryItems.GetRemainingGood(shopItem.UUID);
                    if (taskGoods["data"] is List<Dictionary<string, object>> listGood)
                    {
                        foreach (var good in listGood)
                        {
                            grpItem.AddGood(new GoodModel(shopItem, good));
                        }
                    }

                    AllShopItem.Add(shopItem);
                    ListBaseGroupItem.Add(grpItem);
                }
            }

            GridItems = ListBaseGroupItem;
        }

        [RelayCommand]
        public void AddQuantity()
        {
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
            DrawerItemVM.IsAdd = false;
            DrawerItemVM.ToggleDrawer();
        }
    }
}
