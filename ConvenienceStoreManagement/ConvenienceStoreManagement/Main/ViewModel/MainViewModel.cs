using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        }

        private Dictionary<int, PageViewModelBase> Pages;

        [ObservableProperty]
        private PageViewModelBase currentPage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShopPanel))]
        [NotifyPropertyChangedFor(nameof(ItemPanel))]
        [NotifyPropertyChangedFor(nameof(CustomerManagePanel))]
        private int choosedPanel = 0;

        public bool ShopPanel => ChoosedPanel == (int)MainPanel.Shop;
        public bool ItemPanel => ChoosedPanel == (int)MainPanel.ItemManage;
        public bool CustomerManagePanel => ChoosedPanel == (int)MainPanel.CustomerManage;

        public MainViewModel(Action<int> changeWindow)
        {
            this.ChangeWindow = changeWindow;
        }

        public override ViewModelBase FinishBuild()
        {
            if (ViewWindow == null || dbManager == null) return this;
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
            };

            NavigateNext();
            return this;
        }

        private void NavigateNext()
        {
            CurrentPage = Pages[ChoosedPanel];
        }


        [RelayCommand]
        public void ChangeToShop()
        {
            ChoosedPanel = (int)MainPanel.Shop;
            NavigateNext();
        }

        [RelayCommand]
        public void ChangeToCustomerManage()
        {
            ChoosedPanel = (int)MainPanel.CustomerManage;
            NavigateNext();
        }

        [RelayCommand]
        public void ChangeToItemManage()
        {
            ChoosedPanel = (int)MainPanel.ItemManage;
            NavigateNext();
        }
    }
}
