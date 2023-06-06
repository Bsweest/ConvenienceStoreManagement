using Npgsql;
using System;
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
            return await BaseQueryCall(
               "SELECT * FROM employee",
               null
           );
        }
        public async Task<Dictionary<string, object?>> ChangeAuthEmployee(int id, string username, string password)
        {
            return await BaseQueryCall(
               "UPDATE employee SET username=$1, password=$2 " +
               "WHERE id=$3 RETURNING *",
                new object[] { username, password, id }
           );
        }

        public async Task<Dictionary<string, object?>> InsertEmployee(string name, string phonenum, string? pid)
        {
            return await BaseQueryCall(
                "INSERT INTO employee (name, phonenum, person_id) " +
                "VALUES ($1, $2, $3) RETURNING *",
                new object[] { name, phonenum, pid == null ? DBNull.Value : pid }
            );
        }
        public async Task<Dictionary<string, object?>> UpdateEmployee(int id, string name, string phonenum, string? pid)
        {
            return await BaseQueryCall(
                "UPDATE employee SET name = $1, phonenum = $2, person_id = $3 " +
                "WHERE id = $4 RETURNING *",
                new object[] { name, phonenum, pid == null ? DBNull.Value : pid, id }
            );
        }
        public async Task<Dictionary<string, object?>> ChangeEmployeeAuth(int id, string username, string password)
        {
            return await BaseQueryCall(
                "UPDATE employee SET username = $1, password = $2" +
                "WHERE id = $3 RETURNING *",
                new object[] { username, password, id }
            );
        }
    }
}
