using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ConvenienceStoreManagement.Auth;
using ConvenienceStoreManagement.Database;
using ConvenienceStoreManagement.Main;
using ConvenienceStoreManagement.Main.ViewModel;

namespace ConvenienceStoreManagement
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
            };
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