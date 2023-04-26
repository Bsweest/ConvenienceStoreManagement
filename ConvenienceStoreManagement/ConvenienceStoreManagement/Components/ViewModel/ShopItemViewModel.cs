using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class ShopItemViewModel : GroupItemViewModel
    {
        public ShopItemViewModel(ShopItemModel item) : base(item)
        {
        }

        public int TotalCost => Item.GetCost() * Count;

        [RelayCommand]
        public async void ShowNumberBox()
        {
        }
    }
}
