using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class NumberBoxViewModel : BaseBoxViewModel
    {
        [ObservableProperty]
        private CustomerModel customer;

        [ObservableProperty]
        private int choosedNumber = 0;

        [ObservableProperty]
        private string _title = "";

        [ObservableProperty]
        private string purpose = "";
        [ObservableProperty]
        private string errorMessage = "";

        [ObservableProperty]
        private bool enableFunction = true;

        public NumberBoxViewModel() { }

        public override void CloseBehaviour()
        {
            ChoosedNumber = 0;
            base.CloseBehaviour();
        }

        partial void OnChoosedNumberChanged(int value)
        {
            if (ChoosedNumber < 0)
            {
                if (Customer.Balance + ChoosedNumber < 0)
                {
                    EnableFunction = false;
                    ErrorMessage = "Not Enough Balance";
                    return;
                }
            }
            ErrorMessage = "";
            EnableFunction = true;
        }
        public async override void OKBehaviour()
        {
            if (!EnableFunction) return;
            var result = await dbManager.QueryCustomer.TransactionBalance(
                Customer.Id,
                dbManager.WorkingEmployee.Id,
                ChoosedNumber
            );
            if (result["error"] is object error)
            {
                ErrorMessage = error.ToString();
            }
            if (result["data"] is Dictionary<string, object>)
            {
                ViewWindow.Close();
            }
        }
    }

}
