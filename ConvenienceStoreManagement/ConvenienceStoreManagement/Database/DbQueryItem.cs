using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryItem
    {
        private readonly NpgsqlDataSource DataSource;
        public DbQueryItem(NpgsqlDataSource dataSource)
        {
            this.DataSource = dataSource;
        }

        public async Task<Dictionary<string, object?>> BaseQueryCall
            (string queryString, object[]? parameters)
        {
            await using var connection = await DataSource.OpenConnectionAsync();
            Dictionary<string, object?> task = new()
            {
                { "data", null },
                { "error", null },
            };
            List<Dictionary<string, object>> data = new();

            await using var cmd = new NpgsqlCommand(queryString, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new NpgsqlParameter() { Value = param });
                }
            }

            try
            {
                var rsReader = await cmd.ExecuteReaderAsync();
                while (await rsReader.ReadAsync())
                {
                    data.Add(Enumerable.Range(0, rsReader.FieldCount)
                                .ToDictionary(i => rsReader.GetName(i), i => rsReader.GetValue(i)));
                }
                task["data"] = data;
            }
            catch (Exception ex)
            {
                task["error"] = ex.ToString();
            }

            return task;
        }

        public async Task<Dictionary<string, object?>> GetAllItem()
        {
            var task = await BaseQueryCall("SELECT * FROM shopitem", null);
            return task;
        }

        public async Task<Dictionary<string, object?>> GetRemainingGood(string itemID)
        {
            var task = await BaseQueryCall(
                "SELECT * FROM good " +
                "WHERE itemid=$1 AND expired_date > current_date " +
                "ORDER BY expired_date ASC"
                , new object[] { itemID });
            return task;
        }

        public async Task<Dictionary<string, object?>> AddItem
            (string name, string imagePath = "", int price = 0)
        {
            var task = await BaseQueryCall(
                "INSERT INTO shopitem (name, image_path, price)" +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { name, imagePath, price }
            );
            return task;
        }

        public async Task<Dictionary<string, object?>> UpdateItem
           (string uuid, string name, string imagePath = "", int price = 0)
        {

            var task = await BaseQueryCall(
                "UPDATE shopitem SET name=$1, image_path=$2, price=$3" +
                "WHERE id=$5 RETURNING *",
                new object[] { name, imagePath, price, uuid }
            );

            return task;
        }

        public async Task<Dictionary<string, object?>> AddGood
            (string goodId, string itemID, DateOnly mfd, DateOnly expired)
        {
            var task = await BaseQueryCall(
                "INSERT INTO good (id, itemid, mfg_date, expired_date) " +
                "VALUES ($1, $2, $3, $4) RETURNING *",
                new object[] { goodId, itemID, mfd, expired }
                );

            return task;
        }
    }
}
