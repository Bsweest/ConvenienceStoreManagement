using CommunityToolkit.Mvvm.Input;

namespace ConvenienceStoreManagement.Components.ViewModel.Base
{
    public partial class BaseBoxViewModel : ViewModelBase
    {
        public virtual void OKBehaviour() { }

        public virtual void CloseBehaviour()
        {
            if (ViewWindow != null)
            {
                ViewWindow.Close();
            }
        }

        // Business Logic
        [RelayCommand]
        public void CloseForm()
        {
            CloseBehaviour();
        }

        [RelayCommand]
        public void ClickOK()
        {
            OKBehaviour();
        }
    }
}
