using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Database;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly DbManager dbManager;

        enum MainPanel
        {
            Shop,
            ItemManage,
            CustomerManage,
        }

        private readonly Dictionary<int, PageViewModelBase> Pages = new()
        {
            { (int)MainPanel.Shop, new ShopViewModel() },
            { (int)MainPanel.CustomerManage, new CustomerManageViewModel() },
            { (int)MainPanel.ItemManage, new ItemManageViewModel() },
        };

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

        public MainViewModel(DbManager dbManager)
        {
            this.dbManager = dbManager;
            NavigateNext();
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
