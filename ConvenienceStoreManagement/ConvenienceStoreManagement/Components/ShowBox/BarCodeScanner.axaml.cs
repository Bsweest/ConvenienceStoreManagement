using Avalonia.Controls;
using ConvenienceStoreManagement.Components.ViewModel;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class BarCodeScanner : Window
{
    public BarCodeScanner()
    {
        InitializeComponent();
    }

    public static BarCodeScanner CreateBox
        (BarCodeScannerViewModel viewModel)
    {
        var numBox = new BarCodeScanner
        {
            DataContext = viewModel
        };
        viewModel.SetViewWindow(numBox);

        numBox.Closing += delegate
        {
            viewModel.StopScanning();
            viewModel.SetViewWindow(null);
        };

        return numBox;
    }
}