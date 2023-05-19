using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class CartItemViewModel : GroupItemViewModel
    {
        public CartItemViewModel(ShopItemModel item) : base(item) { }

        public bool CompareId(GoodModel g) => Item.UUID == g.Item.UUID;
    }
}
