using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryItem : DbQueryBase
    {
        public DbQueryItem(NpgsqlDataSource dataSource) : base(dataSource) { }

        public async Task<Dictionary<string, object?>> GetAllItem()
        {
            var task = await BaseQueryCall("SELECT * FROM shopitem", null);
            return task;
        }

        public async Task<Dictionary<string, object?>> SearchMatchingItem(string matchText)
        {
            var task = await BaseQueryCall(
                "SELECT * FROM shopitem " +
                "WHERE name LIKE %" + matchText + "%"
            );
            return task;
        }


        public async Task<Dictionary<string, object?>> GetRemainingGood(Guid itemID)
        {
            var task = await BaseQueryCall(
                "SELECT * FROM good " +
                "WHERE itemid=$1 " +
                "AND expired_date > current_date AND invoice_id is null " +
                "ORDER BY expired_date ASC",
                new object[] { itemID }
            );
            return task;
        }

        public async Task<Dictionary<string, object?>> AddItem
            (string name, string imagePath = "", int price = 0)
        {
            var task = await BaseQueryCall(
                "INSERT INTO shopitem (name, image_path, price) " +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { name, imagePath, price }
            );
            return task;
        }

        public async Task<Dictionary<string, object?>> UpdateItem
           (string uuid, string name, string imagePath = "", int price = 0)
        {

            var task = await BaseQueryCall(
                "UPDATE shopitem SET name=$1, image_path=$2, price=$3 " +
                "WHERE id = $4 RETURNING *",
                new object[] { name, imagePath, price, uuid }
            );

            return task;
        }

        public async Task<Dictionary<string, object?>> AddGood
            (int goodId, Guid itemID, DateOnly mfd, DateOnly expired)
        {
            var task = await BaseQueryCall(
                "INSERT INTO good (id, itemid, mfg_date, expired_date) " +
                "VALUES ($1, $2, $3, $4) RETURNING *",
                new object[] { goodId, itemID, mfd, expired }
                );

            return task;
        }

        public async Task<Dictionary<string, object?>> GetGoodWithItem(int goodId)
        {
            var task = await BaseQueryCall(
                "select G.*, S.name, S.price, S.image_path from good G " +
                "JOIN shopitem S on G.itemid = S.id " +
                "where G.id = $1",
                new object[] { goodId }
            );

            return task;
        }

        public async Task<Dictionary<string, object?>> GetShopItem(int itemId)
        {
            var task = await BaseQueryCall(
                "SELECT * FROM shopitem " +
                "WHERE id=$1",
                new object[] { itemId }
                );

            return task;
        }
    }
}
