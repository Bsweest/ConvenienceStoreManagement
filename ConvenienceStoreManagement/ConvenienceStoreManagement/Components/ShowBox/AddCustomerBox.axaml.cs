using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ConvenienceStoreManagement.Components.ViewModel;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class AddCustomerBox : Window
{
    public enum AddCustomerResult
    {
        Success,
        Fail,
        Cancel,
    }

    public AddCustomerBox()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Task<AddCustomerResult> ShowBox()
    {
        AddCustomerBoxViewModel viewModel = new();
        var addBox = new AddCustomerBox
        {
            DataContext = viewModel
        };
        viewModel.SetParentWindow(addBox);

        var taskCompletion = new TaskCompletionSource<AddCustomerResult>();
        addBox.Closed += delegate { taskCompletion.TrySetResult(viewModel.Result); };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            addBox.ShowDialog(desktop.MainWindow);
        }
        else addBox.Show();

        return taskCompletion.Task;
    }
}
