using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class ShopItemModel
    {
        public ShopItemModel(Dictionary<string, object> data)
        {
            UUID = data["id"].ToString();
            Name = data["name"].ToString();
            Price = (int)data["price"];
            if (!string.IsNullOrEmpty(data["image_path"].ToString()))
                ImagePath = data["image_path"].ToString();
        }


        public ShopItemModel(string uuid, string name, int price, string? imagePath)
        {
            UUID = uuid;
            Name = name;
            Price = price;
            if (!string.IsNullOrEmpty(imagePath)) ImagePath = imagePath;
        }

        public string UUID { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Price { get; private set; }
        public string ImagePath { get; private set; } = "avares://Assets/placeholder.png";
        public int Discount { get; private set; }

        public int GetCost()
        {
            return Price * (1 - Discount / 100);
        }
    }
}
