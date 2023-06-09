using ConvenienceStoreManagement.Model.Control;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model.Product
{
    public class WeightProduct : GoodModel
    {
        public WeightProduct(ShopItemModel item, Dictionary<string, object> data) : base(item, data)
        {
        }
        public override void AcceptVisitor(IProductVisitor visitor)
        {
            visitor.BoughtWeightProduct(this);
        }
    }
}
