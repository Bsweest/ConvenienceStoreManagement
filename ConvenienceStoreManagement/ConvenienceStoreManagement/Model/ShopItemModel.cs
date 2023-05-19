using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class ShopItemModel
    {
        public ShopItemModel(Dictionary<string, object> data, bool inGoodData = false)
        {
            UUID = inGoodData ? data["itemid"].ToString() : data["id"].ToString();
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
        public int Discount { get; private set; } = 0;

        public int GetCost()
        {
            return Price * (1 - Discount / 100);
        }

        public string? GetProperty(string key)
        {
            if (key == "ID") return UUID;
            if (key == "Name") return Name;
            return null;
        }
    }
}
