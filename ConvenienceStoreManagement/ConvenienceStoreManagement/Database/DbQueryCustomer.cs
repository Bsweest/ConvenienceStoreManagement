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
    }
}
