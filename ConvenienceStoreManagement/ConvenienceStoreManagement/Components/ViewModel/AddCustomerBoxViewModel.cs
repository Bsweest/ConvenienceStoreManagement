using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class AddCustomerBoxViewModel : BaseBoxViewModel
    {
        public CustomerModel? CustomerResult { get; private set; }

        [ObservableProperty]
        private string cusName = "";
        [ObservableProperty]
        private string cusPhoneNum = "";
        [ObservableProperty]
        private string? personID = null;
        [ObservableProperty]
        private string? errorMessage = null;

        public override async void OKBehaviour()
        {
            var result = await dbManager.QueryCustomer.InsertCustomer(cusName, cusPhoneNum, personID);
            result.ToSingle();

            if (result["error"] is object error)
            {
                ErrorMessage = error.ToString();
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                CustomerResult = new(data);
                ViewWindow.Close();
            }
        }
    }
}
