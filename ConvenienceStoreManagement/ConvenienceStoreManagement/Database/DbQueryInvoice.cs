using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryInvoice : DbQueryBase
    {
        public DbQueryInvoice(NpgsqlDataSource dataSource) : base(dataSource)
        {
        }

        public async Task<Dictionary<string, object?>> CreateNewInvoice
             (int cusID, int staffID, int totalCost)
        {
            var task = await BaseQueryCall(
                "INSERT INTO invoice (cus_id, staff_id, total_cost) " +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { cusID, staffID, totalCost }
            );

            return task;
        }

        public async Task<Dictionary<string, object?>> UpdateGoodAfterPurchased
             (int id, int invoice, int cost)
        {

            var task = await BaseQueryCall(
                "UPDATE good SET invoice_id = $1, cost = $2 " +
                "WHERE id = $3 RETURNING *",
                new object[] { invoice, cost, id }
            );

            return task;
        }

        public async Task<Dictionary<string, object?>> UpdateNoScanGoodAfterPurchased
             (int id, int invoice, int cost)
        {

            return await BaseQueryCall("");
        }
        public async Task<Dictionary<string, object?>> UpdateWeightProductAfterPurchased
             (int id, int invoice, int cost)
        {
            return await BaseQueryCall("");
        }
    }
}
