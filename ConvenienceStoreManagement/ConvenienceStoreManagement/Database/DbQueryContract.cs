using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryContract : DbQueryBase
    {
        public DbQueryContract(NpgsqlDataSource dataSource) : base(dataSource)
        {
        }

        public async Task<Dictionary<string, object?>> GetContractInfo(int id)
        {
            return await BaseQueryCall(
                "SELECT * FROM contract " +
                "WHERE id=$1",
                new object[] { id }
            );
        }
        public async Task<Dictionary<string, object?>> NewContract
            (int staff_id, int salary, DateTimeOffset s, DateTimeOffset e)
        {
            var task = await BaseQueryCall(
                "INSERT INTO contract (staff_id, salary, start_date, end_date) " +
                "VALUES ($1, $2, $3, $4) RETURNING *",
                new object[] { staff_id, salary, s.Date, e.Date }
            );

            if (task["data"] is List<Dictionary<string, object>> listData)
            {
                await BaseQueryCall(
                    "UPDATE employee SET contract_id=$1 " +
                    "WHERE id=$2",
                    new object[] { (int)listData[0]["id"], staff_id }
                );
            }

            return task;
        }
        public async Task<Dictionary<string, object?>> RemoveContract(int id)
        {
            var task = await BaseQueryCall(
                "UPDATE employee SET contract_id = null " +
                "WHERE contract_id = $1",
                new object[] { id }
            );

            if (task["error"] is object) return task;

            task["data"] = new List<Dictionary<string, object>>() { new Dictionary<string, object>() };
            return task;
        }
    }
}
