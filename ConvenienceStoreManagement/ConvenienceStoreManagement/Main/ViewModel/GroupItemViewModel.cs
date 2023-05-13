using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Model;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class GroupItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        public ShopItemModel item;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Count))]
        [NotifyPropertyChangedFor(nameof(NearestExpiredDate))]
        private List<GoodModel> goods = new();

        public GroupItemViewModel(ShopItemModel item)
        {
            Item = item;
        }

        public int Count => Goods.Count;

        public DateOnly? NearestExpiredDate =>
            Goods.Count > 0 ? Goods[0].ExpiredDate : null;
        public bool CompareId(string id) => id == Item.UUID;

        public void AddGood(GoodModel goods)
        {
            Goods.Add(goods);
        }
        public void AddGood(GoodModel[] goods)
        {
            Goods.AddRange(goods);
        }
    }
}
