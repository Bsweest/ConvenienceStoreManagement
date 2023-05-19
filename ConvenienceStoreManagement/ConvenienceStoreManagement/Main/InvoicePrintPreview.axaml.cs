using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Main.ViewModel;
using ConvenienceStoreManagement.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Main;

public enum CreateInvoiceResult
{
    Success,
    Fail,
    Cancel,
}

public partial class InvoicePrintPreview : Window
{
    public InvoicePrintPreview()
    {
        InitializeComponent();
    }

    public static Task<CreateInvoiceResult> CreateInvoicePreview
        (ObservableCollection<CartItemViewModel> list, CustomerModel? customer, DbManager dbManager)
    {
        var viewModel = new InvoicePrintViewModel(list, customer);
        viewModel.SetDatabaseConnection(dbManager);

        InvoicePrintPreview invoicePreview = new()
        {
            DataContext = viewModel,
        };
        viewModel.InvoicePanel = invoicePreview.InvoicePanel;

        var taskCompletion = new TaskCompletionSource<CreateInvoiceResult>();
        invoicePreview.Closed += delegate { taskCompletion.TrySetResult(viewModel.Result); };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _ = invoicePreview.ShowDialog(desktop.MainWindow);
        }
        else invoicePreview.Show();

        return taskCompletion.Task;
    }
}