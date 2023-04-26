using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Database;
using System;

namespace ConvenienceStoreManagement.Auth
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

        private Action<int> changeWindow;

        public AuthViewModel(DbManager dbManager, Action<int> changeWindow)
        {
            this.dbManager = dbManager;
            this.changeWindow = changeWindow;
        }

        [RelayCommand]
        public void Login()
        {
            //await using var cmd = new NpgsqlCommand(
            //    "SELECT * FROM app_auth WHERE username = $1",
            //    dbManager.Db)
            //{
            //    Parameters = { new() { Value = Username } }
            //};
            //await using var reader = await cmd.ExecuteReaderAsync();
            //if (!reader.HasRows)
            //{
            //    ErrorMessage = "No account found";
            //    return;
            //}
            //while (await reader.ReadAsync())
            //{
            //    string auth_pass = reader.GetString(1);
            //    if (auth_pass == Password)
            //    {

            //    }
            //    else
            //    {
            //        ErrorMessage = "Password is not correct";
            //    }
            //}
            changeWindow(1);
        }
    }
}
