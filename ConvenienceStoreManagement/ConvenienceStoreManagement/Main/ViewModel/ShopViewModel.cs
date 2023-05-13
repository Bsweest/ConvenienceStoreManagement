using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ShopViewModel : PageViewModelBase
    {
        // Current Customer
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasCustomer))]
        private CustomerModel? customer;

        private readonly MainWindow MainWindow;

        public bool HasCustomer => Customer != null;
        public string ButtonContent => HasCustomer ? "Customer: " + Customer.FullName : "Guest Customer";

        // Side Drawer Content
        [ObservableProperty]
        private DrawerUserViewModel drawerUserVM = new();

        //Cart
        [ObservableProperty]
        private BarCodeScannerViewModel scannerVM = new();

        [ObservableProperty]
        private List<ShopItemViewModel> cartList = new() { };

        public ShopViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            BarCodeScanner.CreateBox(scannerVM, mainWindow);
        }

        public override ShopViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as ShopViewModel;
        }

        [RelayCommand]
        public void ToggleScanning()
        {
            ScannerVM.ToggleScanning();
        }

        [RelayCommand]
        public void PrintInvoice()
        {
            InvoicePrintPreview.CreateInvoicePreview();
        }
    }
}
