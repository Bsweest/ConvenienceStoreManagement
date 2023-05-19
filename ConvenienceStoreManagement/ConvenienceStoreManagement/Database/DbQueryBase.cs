using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryBase
    {
        private readonly NpgsqlDataSource DataSource;
        public DbQueryBase(NpgsqlDataSource dataSource)
        {
            this.DataSource = dataSource;
        }

        public async Task<Dictionary<string, object?>> BaseQueryCall
            (string queryString, object[]? parameters = null)
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
    }
}
