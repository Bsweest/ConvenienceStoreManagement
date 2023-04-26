namespace ConvenienceStoreManagement.Model
{
    public class ShopItemModel
    {
        public ShopItemModel(string uuid, string name, int price, string? imagePath, int discount)
        {
            UUID = uuid;
            Name = name;
            Price = price;
            Discount = discount;
            if (imagePath != null) ImagePath = imagePath;
        }

        public string UUID { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Price { get; private set; }
        public string ImagePath { get; private set; } = "avares://Assets/placeholder.jpg";
        public int Discount { get; private set; }

        public int GetCost()
        {
            return Price * (1 - Discount / 100);
        }
    }
}
