using CommunityToolkit.Mvvm.ComponentModel;

namespace GroceryStoreManagement.Main.ViewModel
{
    public partial class ShopViewModel : PageViewModelBase
    {
        public ShopViewModel() { }

        [ObservableProperty]
        private string[] listItem = { "da", "dsa", "f", "fd", "fds" };
    }
}
