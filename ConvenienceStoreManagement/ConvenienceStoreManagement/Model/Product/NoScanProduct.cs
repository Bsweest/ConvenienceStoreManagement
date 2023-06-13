using ConvenienceStoreManagement.Model.Control;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class NoScanProduct : GoodModel
    {
        public NoScanProduct(ShopItemModel item, Dictionary<string, object> data) : base(item, data)
        {
        }

        public override void AcceptVisitor(IProductVisitor visitor)
        {
            visitor.BoughtNoScanProduct(this);
        }
    }
}
