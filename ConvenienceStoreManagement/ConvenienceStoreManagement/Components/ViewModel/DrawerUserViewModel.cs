using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Model;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class DrawerUserViewModel : ViewModelBase
    {
        Action<CustomerModel?> ChoosePayingCustomer;
        public DrawerUserViewModel(Action<CustomerModel?> action)
        {
            ChoosePayingCustomer = action;
        }

        // Choose Customer Drawer
        [ObservableProperty]
        private bool isClosed = true;

        [ObservableProperty]
        private bool isGuestCheck = true;

        // Query Customer
        [ObservableProperty]
        private CustomerModel? findCustomer;

        [ObservableProperty]
        private bool showNotFound = false;
        [ObservableProperty]
        private string searchPhoneNum = "";

        partial void OnFindCustomerChanged(CustomerModel? value)
        {
            if (value != null)
            {
                ShowNotFound = false;
                IsGuestCheck = false;
            }
            else
            {

            }
        }

        [RelayCommand]
        public void ToggleChooseDrawer()
        {
            IsClosed = !IsClosed;
            if (IsClosed)
            {
                IsGuestCheck = true;
                ShowNotFound = false;
                ChoosePayingCustomer(FindCustomer);
                FindCustomer = null;
            }
        }

        [RelayCommand]
        public async void QueryCustomerPhoneNum()
        {
            ShowNotFound = false;
            var result = await dbManager.QueryCustomer.SearchOneCustomer(SearchPhoneNum);

            if (result["data"] is List<Dictionary<string, object>> listData)
            {
                if (listData.Count == 1)
                    FindCustomer = new(listData[0]);
                else ShowNotFound = true;
            }
        }

        [RelayCommand]
        public void ClearCustomer()
        {
            FindCustomer = null;
        }

        [RelayCommand]
        public async void ShowCreateCustomer()
        {
            var insertResult = await AddCustomerBox.ShowBox(dbManager);
            if (insertResult != null)
            {
                IsGuestCheck = false;
                FindCustomer = insertResult;
            }
        }
    }
}
