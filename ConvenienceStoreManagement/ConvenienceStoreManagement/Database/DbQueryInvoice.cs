using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Tools;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryInvoice : DbQueryBase
    {
        public DbQueryInvoice(NpgsqlDataSource dataSource) : base(dataSource)
        {
        }

        public async Task<Dictionary<string, object?>> CreateNewInvoice
             (int cusID, int staffID, int totalCost, ObservableCollection<CartItemViewModel> listCart)
        {
            var task = await BaseQueryCall(
                "INSERT INTO invoice (cus_id, staff_id, total_cost)" +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { cusID, staffID, totalCost }
            );
            task.ToSingle();

            if (task["error"] is object)
            {
                return task;
            }

            if (task["data"] is Dictionary<string, object> data)
            {
                int[] arrID = Utils.GetArrayIdFromListCart(listCart);
                string param = string.Join(", ", arrID.Select(e => e.ToString()).ToArray());

                var task2 = await BaseQueryCall(
                    "UPDATE good SET invoice_id = $1 " +
                    "WHERE id = ANY('{" + param + "}')",
                    new object[] { (int)data["id"] });

                if (task2["error"] is object error) { }
            }

            return task;
        }
    }
}
