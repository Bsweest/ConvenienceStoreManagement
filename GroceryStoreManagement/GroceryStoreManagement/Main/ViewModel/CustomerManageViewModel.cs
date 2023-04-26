using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class CustomerManageViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private List<CustomerModel> gridCus = new()
        {
            new CustomerModel()
        };
    }
}
