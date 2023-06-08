using Avalonia;
using Avalonia.Controls;
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
            AvaloniaXamlLoader.Load(this);
        }

        public Window FactoryWindow(int x)
        {
            switch (x)
            {
                default:
                    return new AuthenticationHandler
                    {
                        DataContext = new AuthViewModel(ChangeWindow)
                                    .SetDatabaseConnection(dbManager),
                    };
                case 1:
                    mainWindow = new MainWindow();
                    mainWindow.DataContext =
                        new MainViewModel(ChangeWindow)
                            .SetViewWindow(mainWindow)
                            .SetDatabaseConnection(dbManager)
                            .FinishBuild();
                    return mainWindow;
            }
        }

        public void ChangeWindow(int index)
        {
            Window nextWindow = FactoryWindow(index);
            Window? preWindow;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                preWindow = desktop.MainWindow;
                desktop.MainWindow = nextWindow;
                nextWindow.Show();
                preWindow?.Close();
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ChangeWindow(0);
            base.OnFrameworkInitializationCompleted();
        }
    }
}