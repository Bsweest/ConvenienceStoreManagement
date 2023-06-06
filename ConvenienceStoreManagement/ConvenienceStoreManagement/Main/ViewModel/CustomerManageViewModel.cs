using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class CustomerManageViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<CustomerModel>? gridCus = null;
        [ObservableProperty]
        private CustomerModel? selectedCustomer;

        private readonly ObservableCollection<CustomerModel> AllCustomer = new();

        [ObservableProperty]
        private DebounceSearch debounceSearchCustomer;

        public CustomerManageViewModel()
        {
            var filterKeys = new string[] { "ID", "FullName", "PhoneNumber", "PersonID" };
            debounceSearchCustomer = new(FilterCustomer, filterKeys, 2);
        }
        public override CustomerManageViewModel SetViewWindow(Window viewWindow)
        {
            return base.SetViewWindow(viewWindow) as CustomerManageViewModel;
        }

        public override CustomerManageViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as CustomerManageViewModel;
        }

        public override CustomerManageViewModel FinishBuild()
        {
            RetrieveAllCustomer();
            return this;
        }

        public void FilterCustomer(string queryParam, string filter)
        {
            SelectedCustomer = null;
            if (queryParam == "")
            {
                GridCus = AllCustomer;
            }
            else
            {
                var filteredItem = new ObservableCollection<CustomerModel>(
                    AllCustomer.Where(e => e.GetProperty(filter).ToLower().Contains(queryParam.ToLower()))
                );

                GridCus = filteredItem;
            }
        }

        [RelayCommand]
        public async void RetrieveAllCustomer()
        {
            AllCustomer.Clear();
            var result = await dbManager.QueryCustomer.GetAllCustomer();

            if (result["data"] is List<Dictionary<string, object>> data)
            {
                foreach (var each in data)
                {
                    CustomerModel model = new(each);
                    AllCustomer.Add(model);
                }
            }
            GridCus = null;
            GridCus = AllCustomer;
        }

        [RelayCommand]
        public async void ShowCreateCustomer()
        {
            var insertResult = await AddCustomerBox.ShowBox(dbManager);
            if (insertResult != null)
            {
                AllCustomer.Insert(0, insertResult);
            }
        }
        [RelayCommand]
        public async void ShowUpdateCustomer()
        {
            if (SelectedCustomer == null) return;
            var insertResult = await AddCustomerBox.ShowBox(dbManager, SelectedCustomer);
            if (insertResult != null)
            {
                RetrieveAllCustomer();
            }
        }
        [RelayCommand]
        public async void TransactionBalance()
        {
            if (SelectedCustomer == null || dbManager == null) return;
            var result = await NumberBox.ShowBox("Change Customer Balance", SelectedCustomer, dbManager);
            if (result != 0)
            {
                SelectedCustomer.SetNewBalance(result);
                GridCus = null;
                GridCus = AllCustomer;
            }
        }
    }
}
