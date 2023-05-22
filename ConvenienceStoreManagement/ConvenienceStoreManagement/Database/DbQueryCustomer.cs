using ConvenienceStoreManagement.Tools;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryCustomer : DbQueryBase
    {
        public DbQueryCustomer(NpgsqlDataSource dataSource) : base(dataSource) { }

        public async Task<Dictionary<string, object?>> GetAllCustomer()
        {
            var task = await BaseQueryCall(
                "SELECT * FROM customer",
                null
            );

            return task;
        }
        public async Task<Dictionary<string, object?>> SearchOneCustomer(string phonenum)
        {
            var task = await BaseQueryCall(
                "SELECT * FROM customer " +
                "WHERE phonenum=$1",
                new object[] { phonenum }
            );

            return task;
        }
        public async Task<Dictionary<string, object?>> InsertCustomer(string cusname, string phonenum, string? pid = null)
        {
            var task = await BaseQueryCall(
                "INSERT INTO customer (fullname, phonenum, person_id) " +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { cusname, phonenum, pid }
            );

            return task;
        }
        public async Task<Dictionary<string, object?>> UpdateCustomer(int id, string cusname, string phonenum, string? pid = null)
        {
            var task = await BaseQueryCall(
                "UPDATE customer SET cusname=$2, phonenum=$3, person_id=$4 " +
                "WHERE id=$1 RETURNING *",
                new object[] { id, cusname, phonenum, pid }
            );

            return task;
        }
        public async Task<Dictionary<string, object?>> TransactionBalance(int cus, int staff, int amount)
        {
            var task = await BaseQueryCall(
                "INSERT INTO transaction (cus_id, staff_id, amount) " +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { cus, staff, amount }
            );

            task.ToSingle();
            if (task["data"] is Dictionary<string, object>)
            {
                await BaseQueryCall(
                    "UPDATE customer SET balance = balance + $1 " +
                    "WHERE id = $2",
                    new object[] { amount, cus }
                );
            }

            return task;
        }
    }
}
