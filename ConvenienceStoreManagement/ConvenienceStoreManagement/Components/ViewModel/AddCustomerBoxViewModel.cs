using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class AddCustomerBoxViewModel : BaseBoxViewModel
    {
        private bool IsAdd = true;
        private CustomerModel UpdateModel;
        public CustomerModel? CustomerResult { get; private set; }

        [ObservableProperty]
        private string cusName = "";
        [ObservableProperty]
        private string cusPhoneNum = "";
        [ObservableProperty]
        private string? personID = null;
        [ObservableProperty]
        private string? errorMessage = null;

        [ObservableProperty]
        private string title = "Add Customer Message Box";

        public override void OKBehaviour()
        {
            if (IsAdd) Insert();
            else Update();
        }
        public void SetUpdateCustomer(CustomerModel? model)
        {
            if (model == null) return;
            IsAdd = false;
            UpdateModel = model;
            Title = "Update Customer Id - " + UpdateModel.Id;
            UpdateModel = model;
            CusName = model.FullName;
            CusPhoneNum = model.PhoneNumber;
            PersonID = model.PersonId;
        }

        public async void Insert()
        {
            var result = await dbManager.QueryCustomer
                .InsertCustomer(cusName, cusPhoneNum, personID);
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

        public async void Update()
        {
            var result = await dbManager.QueryCustomer
                .UpdateCustomer(UpdateModel.Id, cusName, cusPhoneNum, personID);
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
