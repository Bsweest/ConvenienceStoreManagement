using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Model.Control;
using ConvenienceStoreManagement.Tools;
using MessageBox.Avalonia;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ShopViewModel : PageViewModelBase
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasChoosed))]
        [NotifyPropertyChangedFor(nameof(ButtonCusNameContent))]
        private CustomerModel? customer;
        public bool HasChoosed => Customer != null;

        public string ButtonCusNameContent
            => Customer != null ? "Customer: " + Customer.FullName : "Guest Customer";

        // Side Drawer Content
        [ObservableProperty]
        private DrawerUserViewModel drawerUserVM;

        //Cart
        [ObservableProperty]
        private BarCodeScannerViewModel scannerVM;

        [ObservableProperty]
        private ListCartControl listCartCtrl = new();

        public ShopViewModel() { }

        public override ShopViewModel SetViewWindow(Window viewWindow)
        {
            return base.SetViewWindow(viewWindow) as ShopViewModel;
        }

        public override ShopViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as ShopViewModel;
        }

        public override ShopViewModel FinishBuild()
        {
            DrawerUserVM = new(SetupPayingCustomer);
            DrawerUserVM.SetDatabaseConnection(dbManager);
            ScannerVM = new(AddToShoppingCart, RemoveFromCartByID);
            BarCodeScanner.CreateBox(ScannerVM, ViewWindow as MainWindow);
            return this;
        }

        public void SetupPayingCustomer(CustomerModel? model) => Customer = model;


        [RelayCommand]
        public void ToggleScanning()
        {
            ScannerVM.ToggleScanning();
        }

        [RelayCommand]
        public async void PrintInvoice()
        {
            var result = await InvoicePrintPreview.CreateInvoicePreview(ListCartCtrl.ListCart, Customer, dbManager);
            if (result == CreateInvoiceResult.Success)
            {
                Customer = null;
                ClearCart();
            }
        }

        [RelayCommand]
        public void ClearCart()
        {
            ListCartCtrl.ClearCart();
        }

        public async void AddToShoppingCart(int goodID)
        {
            if (dbManager == null) { return; }
            if (dbManager.QueryItems == null) { return; }

            var result = await dbManager.QueryItems.GetGoodWithItem(goodID);
            result.ToSingle();
            if (result["error"] is object)
            {
                var errorBox = MessageBoxManager
                    .GetMessageBoxStandardWindow("Good Not Found", "Can not find the Good ID");
                _ = errorBox.Show();
                ScannerVM.ProcessResult = $"Fail To Find ID: {goodID}";
                return;
            };

            if (result["data"] is Dictionary<string, object> data)
            {
                ShopItemModel itemModel = new(data, true);
                GoodModel goodModel = new(itemModel, data);
                if (goodModel.Cost != null)
                {
                    ScannerVM.ProcessResult = "Product is already bought!";
                    return;
                }
                if (ListCartCtrl.CheckExistedInCart(goodID))
                {
                    ScannerVM.ProcessResult = "Product is already in Cart";
                    return;
                }

                ListCartCtrl.AddGood(goodModel);
                ScannerVM.ProcessResult = $"Success Add {itemModel.Name}, ID: {goodID}";
            }
        }

        public void RemoveFromCartByID(int id)
        {
            if (ListCartCtrl.RemoveGoodById(id))
            {
                ScannerVM.ProcessResult = $"ID: {id} is Removed from shopping cart";
            }
            else
            {
                ScannerVM.ProcessResult = $"Fail To Remove ID {id}";
            }
        }
    }
}
