using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryStoreManagement.Database;
using Npgsql;
using System.Threading.Tasks;

namespace GroceryStoreManagement.Auth
{
    public partial class AuthViewModel : ViewModelBase
    {
        private DbManager dbManager;

        [ObservableProperty]
        private string _username = "";

        [ObservableProperty]
        private string _password = "";

        [ObservableProperty]
        private string _errorMessage = "";

        public AuthViewModel(DbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        [RelayCommand]
        public async Task Login()
        {
            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM app_auth WHERE username = $1",
                dbManager.Db)
            {
                Parameters = { new() { Value = Username } }
            };
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                ErrorMessage = "No account found";
                return;
            }
            while (await reader.ReadAsync())
            {
                string auth_pass = reader.GetString(1);
                if (auth_pass == Password)
                {

                }
                else
                {
                    ErrorMessage = "Password is not correct";
                }
            }
        }
    }
}
