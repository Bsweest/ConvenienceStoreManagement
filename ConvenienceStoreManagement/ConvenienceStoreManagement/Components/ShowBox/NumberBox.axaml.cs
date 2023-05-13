using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ConvenienceStoreManagement.Components.ViewModel;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class NumberBox : Window
{
    public NumberBox()
    {
        InitializeComponent();
    }

    public static Task<int> ShowBox(string title)
    {
        NumberBoxViewModel viewModel = new(title);
        var numBox = new NumberBox
        {
            DataContext = viewModel
        };
        viewModel.SetViewWindow(numBox);

        var taskCompletion = new TaskCompletionSource<int>();
        numBox.Closed += delegate
        {
            taskCompletion.TrySetResult(viewModel.ChoosedNumber);
        };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            numBox.ShowDialog(desktop.MainWindow);
        }
        else numBox.Show();

        return taskCompletion.Task;
    }
}