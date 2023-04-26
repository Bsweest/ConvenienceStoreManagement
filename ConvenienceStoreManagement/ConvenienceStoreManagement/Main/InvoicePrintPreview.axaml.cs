using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ConvenienceStoreManagement.Main.ViewModel;

namespace ConvenienceStoreManagement.Main;

public partial class InvoicePrintPreview : Window
{
    public InvoicePrintPreview()
    {
        InitializeComponent();
    }

    public static void CreateInvoicePreview()
    {
        InvoicePrintViewModel viewModel = new();

        InvoicePrintPreview invoicePreview = new()
        {
            DataContext = viewModel,
        };
        viewModel.InvoicePanel = invoicePreview.InvoicePanel;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            invoicePreview.ShowDialog(desktop.MainWindow);
        }
        else invoicePreview.Show();
    }
}