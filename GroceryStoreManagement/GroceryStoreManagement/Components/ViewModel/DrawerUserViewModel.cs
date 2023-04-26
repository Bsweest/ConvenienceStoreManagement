using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Model;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class DrawerUserViewModel : ViewModelBase
    {
        public DrawerUserViewModel() { }

        // Choose Customer Drawer
        [ObservableProperty]
        private bool isClosed = true;

        [ObservableProperty]
        private bool isGuestCheck = true;

        // Query Customer
        [ObservableProperty]
        private CustomerModel findCustomer;

        [ObservableProperty]
        private bool showNotFound = false;

        [RelayCommand]
        public void ToggleChooseDrawer()
        {
            IsClosed = !IsClosed;
            if (IsClosed)
            {
                IsGuestCheck = true;
                ShowNotFound = false;
            }
        }

        [RelayCommand]
        public void QueryCustomerPhoneNum()
        {
            if (FindCustomer != null)
            {

            }
            else
            {
                ShowNotFound = true;
            }
        }

        [RelayCommand]
        public async void ShowCreateCustomer()
        {
            _ = await AddCustomerBox.ShowBox();
        }
    }
}
