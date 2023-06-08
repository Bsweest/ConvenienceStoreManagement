using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly Action<int> ChangeWindow;

        enum MainPanel
        {
            Shop,
            ItemManage,
            CustomerManage,
            EmployeeManage,
            CreateReport,
        }

        private Dictionary<int, PageViewModelBase> Pages;

        [ObservableProperty]
        private PageViewModelBase currentPage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShopPanel))]
        [NotifyPropertyChangedFor(nameof(ItemPanel))]
        [NotifyPropertyChangedFor(nameof(CustomerManagePanel))]
        [NotifyPropertyChangedFor(nameof(EmployeeManagePanel))]
        [NotifyPropertyChangedFor(nameof(CreateReportPanel))]
        private int choosedPanel = 0;

        public bool ShopPanel => ChoosedPanel == (int)MainPanel.Shop;
        public bool ItemPanel => ChoosedPanel == (int)MainPanel.ItemManage;
        public bool CustomerManagePanel => ChoosedPanel == (int)MainPanel.CustomerManage;
        public bool EmployeeManagePanel => ChoosedPanel == (int)MainPanel.EmployeeManage;
        public bool CreateReportPanel => ChoosedPanel == (int)MainPanel.CreateReport;

        [ObservableProperty]
        NotificationViewModel notify = new();

        public MainViewModel(Action<int> changeWindow)
        {
            this.ChangeWindow = changeWindow;
        }

        public override ViewModelBase FinishBuild()
        {
            if (ViewWindow == null || dbManager == null) return this;
            dbManager.SetupNofifycation(Notify);
            Pages = new()
            {
                {
                    (int)MainPanel.Shop,
                    new ShopViewModel()
                        .SetViewWindow(ViewWindow)
                        .SetDatabaseConnection(dbManager)
                        .FinishBuild()
                },
                {
                    (int)MainPanel.CustomerManage,
                    new CustomerManageViewModel()
                        .SetViewWindow(ViewWindow)
                        .SetDatabaseConnection(dbManager)
                        .FinishBuild()
                },
                {
                    (int)MainPanel.ItemManage,
                    new ItemManageViewModel()
                        .SetViewWindow(ViewWindow)
                        .SetDatabaseConnection(dbManager)
                        .FinishBuild()
                },
                {
                    (int)MainPanel.EmployeeManage,
                    new EmployeeManageViewModel()
                        .SetViewWindow(ViewWindow)
                        .SetDatabaseConnection(dbManager)
                        .FinishBuild()
                },
                {
                    (int)MainPanel.CreateReport,
                    new CreateReportViewModel()
                        .SetViewWindow(ViewWindow)
                        .SetDatabaseConnection(dbManager)
                        .FinishBuild()
                }
            };
            CurrentPage = Pages[0];
            return this;
        }

        partial void OnChoosedPanelChanged(int value)
        {
            CurrentPage = Pages[value];
        }


        [RelayCommand]
        public void ChangeToShop()
            => ChoosedPanel = (int)MainPanel.Shop;

        [RelayCommand]
        public void ChangeToCustomerManage()
            => ChoosedPanel = (int)MainPanel.CustomerManage;

        [RelayCommand]
        public void ChangeToItemManage()
            => ChoosedPanel = (int)MainPanel.ItemManage;

        [RelayCommand]
        public void ChangeToEmployeeManage()
            => ChoosedPanel = (int)MainPanel.EmployeeManage;
        [RelayCommand]
        public void ChangeToCreateReport()
            => ChoosedPanel = (int)MainPanel.CreateReport;
    }
}
