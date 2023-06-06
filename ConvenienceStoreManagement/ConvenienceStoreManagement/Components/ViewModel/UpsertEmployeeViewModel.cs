using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class UpsertEmployeeViewModel : BaseBoxViewModel
    {
        [ObservableProperty]
        private int? id = null;
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        private string pid = "";
        [ObservableProperty]
        private string phoneNum = "";
        [ObservableProperty]
        private string salary;

        [ObservableProperty]
        private DateTimeOffset? startDate = null;
        [ObservableProperty]
        private DateTimeOffset? endDate = null;

        [ObservableProperty]
        private EmployeeModel? employee;

        [ObservableProperty]
        private string title;

        public UpsertEmployeeViewModel(EmployeeModel? model = null)
        {
            Employee = model;
        }

        partial void OnEmployeeChanged(EmployeeModel? value)
        {
            if (value == null)
            {
                Title = "Create Employee";
                Salary = "";

            }
            else
            {
                Title = "Update Employee Id - " + value.Id;
                Id = value.Id;
                Name = value.Name;
                Pid = value.PersonId;
                PhoneNum = value.PhoneNumber;
                Salary = value.Salary;
                if (value.Contract is ContractModel cont)
                {
                    StartDate = Utils.DateOnlyToDTOS(cont.StartDate);
                    EndDate = Utils.DateOnlyToDTOS(cont.EndDate);
                }
            }
        }

        public override void OKBehaviour()
        {
            if (Employee == null) InsertEmployee();
            else UpdateEmployee();
        }

        public async void InsertEmployee()
        {
            var result = await dbManager.QueryEmployee
                .InsertEmployee(Name, PhoneNum, string.IsNullOrEmpty(Pid) ? null : Pid);
            result.ToSingle();

            if (result["error"] is object error)
            {
                dbManager.Notify(error.ToString(), true);
                return;
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                dbManager.Notify("Insert Employee Success");
                Employee = new EmployeeModel(data);
            }
        }
        public async void UpdateEmployee()
        {
            var result = await dbManager.QueryEmployee
                .UpdateEmployee(Employee.Id, Name, PhoneNum, string.IsNullOrEmpty(Pid) ? null : Pid);
            result.ToSingle();

            if (result["error"] is object error)
            {
                dbManager.Notify(error.ToString(), true);
                return;
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                dbManager.Notify("Update Employee Success");
                Employee = new EmployeeModel(data);
            }
        }

        [RelayCommand]
        public void MakeContract()
        {
            if (Employee == null) return;
            MakeContractBox.ShowBox(Employee, dbManager, ViewWindow);
        }

        [RelayCommand]
        public void ShowChangeAuth()
        {
            ChangeAuth.ShowBox(dbManager, ViewWindow);
        }
    }
}
