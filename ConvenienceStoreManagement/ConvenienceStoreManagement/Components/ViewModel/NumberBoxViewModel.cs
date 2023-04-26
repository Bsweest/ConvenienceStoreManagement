using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel.Base;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class NumberBoxViewModel : BaseBoxViewModel
    {
        [ObservableProperty]
        private int choosedNumber = 0;

        [ObservableProperty]
        private string _title = "";

        public NumberBoxViewModel(string title)
        {
            Title = title;
        }

        [RelayCommand]
        public void BtnOK()
        {

        }
    }

}
