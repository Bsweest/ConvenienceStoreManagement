using Avalonia.Controls;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class ChangeAuth : Window
{
    public ChangeAuth()
    {
        InitializeComponent();
    }

    public static void ShowBox(DbManager db, Window window)
    {
        var vm = new ChangeAuthViewModel();
        var box = new ChangeAuth()
        {
            DataContext = vm
        };
        vm.SetDatabaseConnection(db).SetViewWindow(box);

        box.ShowDialog(window);
    }
}