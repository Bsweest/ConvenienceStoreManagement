using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class GoodModel : IEquatable<GoodModel>, IComparable<GoodModel>
    {
        public GoodModel(ShopItemModel item, Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            Item = item;
            ImportDate = Utils.GetDateOnlyFromDb(data["mfg_date"]);
            ExpiredDate = Utils.GetDateOnlyFromDb(data["expired_date"]);
            Cost = data["cost"] != DBNull.Value ? (int)data["cost"] : null;
        }

        public GoodModel(int id, ShopItemModel item, DateOnly importDate, DateOnly expiredDate)
        {
            Id = id;
            Item = item;
            ImportDate = importDate;
            ExpiredDate = expiredDate;
        }

        public int Id { get; private set; }
        public ShopItemModel Item { get; private set; }
        public DateOnly ImportDate { get; private set; }
        public DateOnly ExpiredDate { get; private set; }
        public InvoiceModel? Invoice { get; private set; }
        public int? Cost { get; private set; }

        public int CompareTo(GoodModel? other)
            => other != null ? this.ExpiredDate.CompareTo(other.ExpiredDate) : 1;

        public bool Equals(GoodModel? other)
            => other != null && this.ExpiredDate.Equals(other.ExpiredDate);
    }
}
