using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
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
        private CustomerModel? selectedEmployee;

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

        public async void RetrieveAllEmployee()
        {
            AllEmployee.Clear();
            var result = await dbManager.QueryEmployee.RetrieveAllEmployee();

            if (result["data"] is List<Dictionary<string, object>> data)
            {
                foreach (var each in data)
                {
                    EmployeeModel model = new(each);
                    AllEmployee.Add(model);
                }
            }
            GridItems = null;
            GridItems = AllEmployee;
        }
    }
}
