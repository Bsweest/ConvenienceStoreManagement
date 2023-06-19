using ConvenienceStoreManagement.Model.Control;
using ConvenienceStoreManagement.Model.Product;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class GoodModel : IEquatable<GoodModel>, IComparable<GoodModel>
    {
        public int Id { get; private set; }
        public ShopItemModel Item { get; private set; }
        public DateOnly ImportDate { get; private set; }
        public DateOnly ExpiredDate { get; private set; }

        public int InvoiceID = 0;
        public InvoiceModel? Invoice { get; private set; }
        public int? Cost { get; private set; }

        public GoodModel(ShopItemModel item, Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            Item = item;
            Cost = data["cost"] != DBNull.Value ? (int)data["cost"] : null;

            ImportDate = Utils.GetDateOnlyFromDb(data["mfg_date"]);
            ExpiredDate = Utils.GetDateOnlyFromDb(data["expired_date"]);
        }

        public static GoodModel? FactoryProduct(ShopItemModel item, Dictionary<string, object> data)
        {
            switch (item.Type)
            {
                case (int)ProductType.SimpleProduct:
                    return new SimpleProduct(item, data);
                case (int)ProductType.NoScanProduct:
                    return new NoScanProduct(item, data);
                case (int)ProductType.WeightProduct:
                    return new WeightProduct(item, data);
            }

            return null;
        }

        public virtual void AcceptVisitor(IProductVisitor visitor) { }

        public int CompareTo(GoodModel? other)
            => other != null ? this.ExpiredDate.CompareTo(other.ExpiredDate) : 1;

        public bool Equals(GoodModel? other)
            => other != null && this.ExpiredDate.Equals(other.ExpiredDate);
    }
}
