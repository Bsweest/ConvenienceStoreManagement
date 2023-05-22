using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using System;

namespace ConvenienceStoreManagement.Auth
{
    public partial class AuthViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string username = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string errorMessage = "";

        private readonly Action<int> ChangeWindow;

        public AuthViewModel(Action<int> changeWindow)
        {
            this.ChangeWindow = changeWindow;
        }

        [RelayCommand]
        public async void Login()
        {
            if (dbManager == null) return;
            await using var connection = await dbManager.DataSource.OpenConnectionAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM employee WHERE username = $1",
                connection)
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
                string auth_pass = reader["password"].ToString();
                if (auth_pass == Password)
                {
                    dbManager.SetWorkingEmployee(new());
                    ChangeWindow(1);
                }
                else
                {
                    ErrorMessage = "Password is not correct";
                }
            }
        }
    }
}
