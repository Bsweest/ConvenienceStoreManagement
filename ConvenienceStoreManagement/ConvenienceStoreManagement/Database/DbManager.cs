using Npgsql;

namespace ConvenienceStoreManagement.Database
{
    public class DbManager
    {
        private readonly string connString = "Host=localhost;Username=postgres;Password=Phanhuymanh123;Database=ConvenienceStoreManagement";

        public int WorkingEmployee { get; private set; } = 1;

        public NpgsqlDataSource DataSource { get; private set; }
        public DbQueryItem? QueryItems { get; private set; }
        public DbQueryCustomer? QueryCustomer { get; private set; }
        public DbQueryInvoice? QueryInvoice { get; private set; }

        public DbManager()
        {
            SetupDatabase();
        }

        public void SetupDatabase()
        {
            if (DataSource != null) return;
            var datasourcebuilder = new NpgsqlDataSourceBuilder(connString);
            DataSource = datasourcebuilder.Build();
            QueryItems = new(DataSource);
            QueryCustomer = new(DataSource);
            QueryInvoice = new(DataSource);
        }

        public void SetWorkingEmployee(int id) { WorkingEmployee = id; }
    }
}
