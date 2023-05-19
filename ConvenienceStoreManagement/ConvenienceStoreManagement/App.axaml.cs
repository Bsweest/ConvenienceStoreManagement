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

        private AuthenticationHandler authWindow;
        private MainWindow mainWindow;

        public override void Initialize()
        {
            InitWindow();
            AvaloniaXamlLoader.Load(this);
        }

        public void InitWindow()
        {
            //Action<int> changeWindow = index => ChangeWindow(index);

            authWindow = new AuthenticationHandler
            {
                DataContext = new AuthViewModel(ChangeWindow)
                                    .SetDatabaseConnection(dbManager),
            };
            mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(ChangeWindow)
                                            .SetViewWindow(mainWindow)
                                            .SetDatabaseConnection(dbManager)
                                            .FinishBuild();
        }

        public void ChangeWindow(int index)
        {
            Window nextWindow = authWindow;
            Window? preWindow;
            switch (index)
            {
                case 0:
                    nextWindow = authWindow;
                    break;
                case 1:
                    nextWindow = mainWindow;
                    break;
            }

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
            ChangeWindow(1);
            base.OnFrameworkInitializationCompleted();
        }
    }
}