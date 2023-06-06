using Avalonia.Controls;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class MakeContractBox : Window
{
    public MakeContractBox()
    {
        InitializeComponent();
    }

    public static async void ShowBox(EmployeeModel staff, DbManager db, Window window)
    {
        var vm = new MakeContractViewModel(staff);
        var box = new MakeContractBox()
        {
            DataContext = vm,
        };
        vm.SetDatabaseConnection(db).SetViewWindow(box);

        box.ShowDialog(window);
    }
}