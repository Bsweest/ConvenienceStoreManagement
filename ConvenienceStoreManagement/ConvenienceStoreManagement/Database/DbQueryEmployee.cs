using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryEmployee : DbQueryBase
    {
        public DbQueryEmployee(NpgsqlDataSource dataSource) : base(dataSource) { }

        public async Task<bool> GrantAdminControl(string check_string)
        {
            return await PgFunctionBoolean(
                "grant_admin_control($1)",
                new object[] { check_string }
            );
        }

        public async Task<Dictionary<string, object?>> RetrieveAllEmployee()
        {
            var task = await BaseQueryCall(
               "SELECT * FROM employee",
               null
           );

            return task;
        }
    }
}
