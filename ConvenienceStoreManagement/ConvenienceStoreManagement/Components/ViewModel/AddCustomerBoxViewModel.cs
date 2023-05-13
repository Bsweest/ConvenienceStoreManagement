using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using static ConvenienceStoreManagement.Components.ShowBox.AddCustomerBox;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class AddCustomerBoxViewModel : BaseBoxViewModel
    {
        public AddCustomerResult Result { get; private set; } = AddCustomerResult.Cancel;

        [ObservableProperty]
        private string cusName = "";
        [ObservableProperty]
        private string cusPhoneNum = "";

        public override void OKBehaviour()
        {

        }
    }
}
