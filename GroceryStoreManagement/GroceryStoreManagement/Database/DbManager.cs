using Npgsql;

namespace GroceryStoreManagement.Database
{
    public class DbManager
    {
        private readonly string connString = "Host=localhost;Username=postgres;Password=Phanhuymanh123;Database=GroceryStoreManagement";
        public NpgsqlConnection? Db { get; private set; }

        public async void OpenConnection()
        {
            if (Db != null) return;
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();
            Db = await dataSource.OpenConnectionAsync();
        }
    }
}
