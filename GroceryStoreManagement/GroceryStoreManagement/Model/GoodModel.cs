using System;

namespace ConvenienceStoreManagement.Model
{
    public class GoodModel : IEquatable<GoodModel>, IComparable<GoodModel>
    {
        public GoodModel(int id, ShopItemModel item, DateOnly importDate, DateOnly expiredDate)
        {
            Id = id;
            Item = item;
            Cost = item.GetCost();
            ImportDate = importDate;
            ExpiredDate = expiredDate;
        }

        public int Id { get; private set; }
        public ShopItemModel Item { get; private set; }
        public int Cost { get; private set; }
        public DateOnly ImportDate { get; private set; }
        public DateOnly ExpiredDate { get; private set; }

        public int CompareTo(GoodModel? other)
            => other != null ? this.ExpiredDate.CompareTo(other.ExpiredDate) : 1;

        public bool Equals(GoodModel? other)
            => other != null && this.ExpiredDate.Equals(other.ExpiredDate);
    }
}
