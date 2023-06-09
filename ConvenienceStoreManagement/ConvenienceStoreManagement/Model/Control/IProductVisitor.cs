using ConvenienceStoreManagement.Model.Product;

namespace ConvenienceStoreManagement.Model.Control
{
    public interface IProductVisitor
    {
        public void BoughtSimpleProduct(SimpleProduct product);
        public void BoughtNoScanProduct(NoScanProduct product);
        public void BoughtWeightProduct(WeightProduct product);
    }
}
