using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Model;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Components.ShowBox;

public partial class AddQuantityGood : Window
{
    public AddQuantityGood()
    {
        InitializeComponent();
    }

    public static void ShowBox(List<ShopItemModel> allItem, DbManager dbManager)
    {
        AddQuantityGoodViewModel viewModel = new(allItem);
        AddQuantityGood addBox = new()
        {
            DataContext = viewModel
        };
        viewModel.SetViewWindow(addBox).SetDatabaseConnection(dbManager);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            addBox.ShowDialog(desktop.MainWindow);
        }
        else addBox.Show();
    }
}