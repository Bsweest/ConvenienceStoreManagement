using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class GroupItemViewModel : ViewModelBase
    {
        [ObservableProperty]
        public ShopItemModel item;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Count))]
        private ObservableCollection<GoodModel> goods = new();

        public GroupItemViewModel(ShopItemModel item)
        {
            Item = item;
        }

        [ObservableProperty]
        private int count;

        [ObservableProperty]
        private int totalCost;

        public string? NearestExpiredDate =>
            Goods.Count > 0 ? Goods[0].ExpiredDate.ToString() : null;
        public bool CompareId(string id) => id == Item.UUID;

        public void AddGood(GoodModel goods)
        {
            Goods.Add(goods);
            Count = Goods.Count;
            TotalCost = Goods.Count * Item.GetCost();
        }
        public void AddGood(GoodModel[] goods)
        {
            Goods.AddRange(goods);
            Count = Goods.Count;
            TotalCost = Goods.Count * Item.GetCost();
        }

        public bool RemoveGoodById(int id)
        {
            try
            {
                var toRemove = Goods.Single(good => good.Id == id);
                Goods.Remove(toRemove);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int[] GetAllGoodID()
        {
            return Goods.Select(e => e.Id).ToArray();
        }
    }
}
