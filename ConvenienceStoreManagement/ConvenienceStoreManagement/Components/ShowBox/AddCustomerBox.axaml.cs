using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class AddCustomerBox : Window
{
    public AddCustomerBox()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Task<CustomerModel?> ShowBox(DbManager db, CustomerModel? model = null)
    {
        var viewModel = new AddCustomerBoxViewModel();
        viewModel.SetUpdateCustomer(model);

        var addBox = new AddCustomerBox
        {
            DataContext = viewModel
        };
        viewModel.SetViewWindow(addBox).SetDatabaseConnection(db);

        var taskCompletion = new TaskCompletionSource<CustomerModel?>();
        addBox.Closed += delegate { taskCompletion.TrySetResult(viewModel.CustomerResult); };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            addBox.ShowDialog(desktop.MainWindow);
        }
        else addBox.Show();

        return taskCompletion.Task;
    }
}
