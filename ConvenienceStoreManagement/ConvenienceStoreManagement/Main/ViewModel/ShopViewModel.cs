using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ShopViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private string test;
        // Current Customer
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasCustomer))]
        private CustomerModel? customer;

        public bool HasCustomer => Customer != null;
        public string ButtonContent => HasCustomer ? "Customer: " + Customer.FullName : "Guest Customer";

        // Side Drawer Content
        [ObservableProperty]
        private DrawerUserViewModel drawerUserVM = new();

        //Cart
        [ObservableProperty]
        private BarCodeScannerViewModel scannerVM = new();

        [ObservableProperty]
        private List<ShopItemViewModel> cartList = new()
        {
            new ShopItemViewModel(new ShopItemModel("uuid", "name", 20, null, 20))
        };

        public ShopViewModel()
        {
            BarCodeScanner.CreateBox(scannerVM);
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
