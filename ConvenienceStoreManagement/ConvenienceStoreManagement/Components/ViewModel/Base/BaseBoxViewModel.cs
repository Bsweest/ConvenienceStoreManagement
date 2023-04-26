using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace ConvenienceStoreManagement.Components.ViewModel.Base
{
    public partial class BaseBoxViewModel : ViewModelBase
    {
        protected Window? Parent;

        public void SetParentWindow(Window parent)
        {
            Parent = parent;
        }

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

        public virtual void OKBehaviour()
        {

        }

        public virtual void CloseBehaviour()
        {
            if (Parent != null)
            {
                Parent.Close();
            }
        }
    }
}
