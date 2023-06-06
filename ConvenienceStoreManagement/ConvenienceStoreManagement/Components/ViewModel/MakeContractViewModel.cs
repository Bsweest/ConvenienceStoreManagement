using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using MessageBox.Avalonia.DTO;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class MakeContractViewModel : BaseBoxViewModel
    {
        [ObservableProperty]
        private EmployeeModel employee;
        [ObservableProperty]
        private DateTimeOffset? startDate = null;
        [ObservableProperty]
        private DateTimeOffset? endDate = null;
        [ObservableProperty]
        private int salary = 0;

        public MakeContractViewModel(EmployeeModel staff)
        {
            Employee = staff;
        }

        [RelayCommand]
        public async void RemoveContract()
        {
            if (Employee == null) return;
            var yesnoBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNoCancel,
                    ContentTitle = "Remove Contract",
                    ContentHeader = "Do you want to Remove contract from this employee: " + Employee.Name,
                    ContentMessage = "Confirm Remove!",
                });
            var button = await yesnoBox.Show();

            if (button == MessageBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var result = await dbManager.QueryContract.RemoveContract(Employee.Contract.Id);
                result.ToSingle();
                if (result["error"] is object error)
                {
                    dbManager.Notify(error.ToString(), true);
                }

                if (result["data"] is Dictionary<string, object> data)
                {
                    dbManager.Notify("Contract has been removed");
                    Employee?.SetupContract(null);
                    CloseBehaviour();
                }
            }
        }

        public override async void OKBehaviour()
        {
            if (StartDate == null || EndDate == null) return;
            var result = await dbManager.QueryContract.NewContract(
                Employee.Id,
                Salary,
                (DateTimeOffset)StartDate,
                (DateTimeOffset)EndDate
            );
            result.ToSingle();

            if (result["error"] is object error)
            {
                dbManager.Notify(error.ToString(), true);
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                var cont = new ContractModel(data);
                dbManager.Notify($"Create Contract Successful, ID: {cont.Id}");
                Employee?.SetupContract(cont);
                CloseBehaviour();
            }
        }
    }
}
