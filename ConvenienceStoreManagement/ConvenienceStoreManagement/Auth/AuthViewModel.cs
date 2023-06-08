using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using ExCSS;
using System;
using System.Collections.Generic;

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
            ErrorMessage = string.Empty;
            if (dbManager == null) return;


            var result = await dbManager.QueryEmployee.Login(Username);
            result.ToSingle();

            if (result["error"] is object error)
            {
                ErrorMessage = error.ToString();
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                var employee = new EmployeeModel(data);

                if (employee.Password == Password)
                {
                    dbManager.SetWorkingEmployee(employee);
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
