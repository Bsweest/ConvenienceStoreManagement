using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Database
{
    public class DbQueryReport : DbQueryBase
    {
        public DbQueryReport(NpgsqlDataSource dataSource) : base(dataSource) { }

        public async Task<Dictionary<string, object?>> ReportForProductSales(string firstMonthDate)
        {
            return await BaseQueryCall(
                "SELECT G.id, I.id as invoice_id, S.id as item_id, S.name as item_name, G.cost, I.cus_id, I.staff_id, I.purchase_time FROM good G " +
                "JOIN invoice I on I.id = G.invoice_id " +
                "JOIN shopitem S on G.itemid = S.id " +
                $"WHERE I.purchase_time >= DATE '{firstMonthDate}' " +
                $"AND I.purchase_time < DATE '{firstMonthDate}' + INTERVAL '1 MONTH' " +
                "ORDER by I.id ASC"
            );
        }

        public async Task<Dictionary<string, object?>> ReportForEmployee(string firstMonthDate)
        {
            return await BaseQueryCall(
                "SELECT E.id, E.name, C.id as contract_id, C.salary, C.start_date, C.end_date, COUNT(I.*) as sale_count , SUM(I.total_cost) as total_revenue " +
                "FROM employee E " +
                "JOIN invoice I on I.staff_id = E.id " +
                "LEFT OUTER JOIN contract C on C.staff_id = E.id " +
                $"WHERE I.purchase_time >= DATE '{firstMonthDate}' " +
                $"AND I.purchase_time < DATE '{firstMonthDate}' + INTERVAL '1 MONTH' " +
                "GROUP BY E.id, C.id"
            );
        }
        public async Task<Dictionary<string, object?>> ReportForCustomer(string firstMonthDate)
        {
            return await BaseQueryCall(
                "SELECT C.id, C.fullname, C.phonenum, COUNT(I.*) as invoice_count, SUM(I.total_cost) as sum_spending, SUM(T.amount) as transaction_amount " +
                "FROM customer C " +
                "JOIN invoice I on I.cus_id = C.id " +
                "LEFT OUTER JOIN transaction T on T.cus_id = C.id " +
                $"WHERE I.purchase_time >= DATE '{firstMonthDate}' " +
                $"AND I.purchase_time < DATE '{firstMonthDate}' + INTERVAL '1 MONTH' " +
                "GROUP BY C.id"
            );
        }
    }
}
