using Avalonia.Controls;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Main;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class BarCodeScanner : Window
{
    public BarCodeScanner()
    {
        InitializeComponent();
    }

    public static BarCodeScanner CreateBox
        (BarCodeScannerViewModel viewModel, MainWindow mainWindow)
    {
        BarCodeScanner numBox = new BarCodeScanner
        {
            DataContext = viewModel
        };
        viewModel.SetViewWindow(numBox);
        numBox.Owner = mainWindow;

        numBox.Closed += delegate
        {
            viewModel.CloseBehaviour();
        };

        return numBox;
    }
}