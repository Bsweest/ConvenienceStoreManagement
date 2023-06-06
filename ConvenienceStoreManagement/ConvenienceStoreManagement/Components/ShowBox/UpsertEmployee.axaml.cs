using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class UpsertEmployee : Window
{
    public UpsertEmployee()
    {
        InitializeComponent();
    }

    public static void ShowBox(EmployeeModel? model, DbManager db)
    {
        var vm = new UpsertEmployeeViewModel(model);

        var box = new UpsertEmployee()
        {
            DataContext = vm
        };
        vm.SetDatabaseConnection(db).SetViewWindow(box);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            box.ShowDialog(desktop.MainWindow);
        }
        else box.Show();
    }
}