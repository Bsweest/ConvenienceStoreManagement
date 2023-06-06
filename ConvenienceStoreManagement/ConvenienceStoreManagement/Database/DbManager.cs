using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Model;
using Npgsql;

namespace ConvenienceStoreManagement.Database
{
    public class DbManager
    {
        private readonly string connString = "Host=localhost;Username=postgres;Password=Phanhuymanh123;Database=ConvenienceStoreManagement";

        public EmployeeModel WorkingEmployee { get; private set; } = new();

        public NpgsqlDataSource DataSource { get; private set; }
        public DbQueryItem? QueryItems { get; private set; }
        public DbQueryCustomer? QueryCustomer { get; private set; }
        public DbQueryInvoice? QueryInvoice { get; private set; }
        public DbQueryEmployee? QueryEmployee { get; private set; }
        public DbQueryContract? QueryContract { get; private set; }

        private NotificationViewModel Notification;

        public DbManager()
        {
            if (DataSource != null) return;
            var datasourcebuilder = new NpgsqlDataSourceBuilder(connString);
            DataSource = datasourcebuilder.Build();
            QueryItems = new(DataSource);
            QueryCustomer = new(DataSource);
            QueryInvoice = new(DataSource);
            QueryEmployee = new(DataSource);
            QueryContract = new(DataSource);
        }

        public void SetWorkingEmployee(EmployeeModel model) => WorkingEmployee = model;

        public void SetupNofifycation(NotificationViewModel vm)
        {
            Notification = vm;
        }

        public void Notify(string msg, bool isError = false)
            => Notification.NotifyMessage(msg, isError);

    }
}
