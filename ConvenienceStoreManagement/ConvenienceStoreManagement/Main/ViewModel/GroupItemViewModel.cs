using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class GroupItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        public ShopItemModel item;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Count))]
        private List<GoodModel> goods = new();

        public GroupItemViewModel(ShopItemModel item)
        {
            Item = item;
        }

        public int Count => Goods.Count;
        public string ImagePath => Item.ImagePath;

        public bool CompareId(string id) => id == Item.UUID;

        public void AddGood(GoodModel[] goods)
        {
            Goods.AddRange(goods);
        }
    }
}
