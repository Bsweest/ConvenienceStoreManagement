using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class EmployeeManageViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<EmployeeModel>? gridItems = null;

        [ObservableProperty]
        private EmployeeModel? selectedEmployee;

        private readonly ObservableCollection<EmployeeModel> AllEmployee = new();
        [ObservableProperty]
        private DebounceSearch debounceSearchEmployee;

        public EmployeeManageViewModel()
        {
            var filterKeys = new string[] { "ID", "Name", "PhoneNumber", "PersonID" };
            DebounceSearchEmployee = new(FilterEmployee, filterKeys, 2);
        }

        public override EmployeeManageViewModel SetViewWindow(Window viewWindow)
        {
            return base.SetViewWindow(viewWindow) as EmployeeManageViewModel;
        }
        public override EmployeeManageViewModel SetDatabaseConnection(DbManager dbManager)
        {
            return base.SetDatabaseConnection(dbManager) as EmployeeManageViewModel;
        }
        public override EmployeeManageViewModel FinishBuild()
        {
            RetrieveAllEmployee();
            return this;
        }

        public void FilterEmployee(string queryParam, string filter)
        {
            SelectedEmployee = null;
            if (queryParam == "")
            {
                GridItems = AllEmployee;
            }
            else
            {
                var filteredItem = new ObservableCollection<EmployeeModel>(
                    AllEmployee.Where(e => e.GetProperty(filter).ToLower().Contains(queryParam.ToLower()))
                );

                GridItems = filteredItem;
            }
        }

        [RelayCommand]
        public async void RetrieveAllEmployee()
        {
            AllEmployee.Clear();
            var result = await dbManager.QueryEmployee.RetrieveAllEmployee();

            if (result["data"] is List<Dictionary<string, object>> data)
            {
                foreach (var each in data)
                {
                    ContractModel? contract = null;
                    if (each["contract_id"] is int contID)
                    {
                        var contractTask = await dbManager.QueryContract.GetContractInfo(contID);
                        contractTask.ToSingle();
                        if (contractTask["data"] is Dictionary<string, object> contData)
                        {
                            contract = new(contData);
                        }

                    }
                    EmployeeModel model = new(each, contract);

                    AllEmployee.Add(model);
                }
            }
            GridItems = null;
            GridItems = AllEmployee;
        }

        [RelayCommand]
        public void InsertEmployee()
        {
            UpsertEmployee.ShowBox(null, dbManager);
        }

        [RelayCommand]
        public void UpdateEmployee()
        {
            if (SelectedEmployee == null) return;
            UpsertEmployee.ShowBox(SelectedEmployee, dbManager);
        }
    }
}
