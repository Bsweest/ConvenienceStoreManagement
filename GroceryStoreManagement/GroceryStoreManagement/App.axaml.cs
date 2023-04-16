using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GroceryStoreManagement.Auth;
using GroceryStoreManagement.Database;
using GroceryStoreManagement.Main;
using GroceryStoreManagement.Main.ViewModel;

namespace GroceryStoreManagement
{
    public partial class App : Application
    {
        private readonly DbManager dbManager = new();

        private AuthenticationHandler? authWindow;
        private MainWindow? mainWindow;
        public override void Initialize()
        {
            InitWindow();
            AvaloniaXamlLoader.Load(this);
            dbManager.OpenConnection();
        }

        public void InitWindow()
        {
            authWindow = new AuthenticationHandler
            {
                DataContext = new AuthViewModel(dbManager),
            };
            mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(dbManager),
            }; ;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}