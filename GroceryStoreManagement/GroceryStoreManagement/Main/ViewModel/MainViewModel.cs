using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryStoreManagement.Database;
using System.Collections.Generic;

namespace GroceryStoreManagement.Main.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly DbManager dbManager;

        enum MainPanel
        {
            Shop,
            Search
        }

        private readonly Dictionary<int, PageViewModelBase> Pages = new()
        {
            { (int)MainPanel.Shop, new ShopViewModel() },
            { (int)MainPanel.Search, new SearchViewModel() },
        };

        [ObservableProperty]
        private PageViewModelBase currentPage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShopPanel))]
        [NotifyPropertyChangedFor(nameof(SearchPanel))]
        private int choosedPanel = 0;

        public bool ShopPanel => ChoosedPanel == (int)MainPanel.Shop;
        public bool SearchPanel => ChoosedPanel == (int)MainPanel.Search;

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
        public void ChangeToSearch()
        {
            ChoosedPanel = (int)MainPanel.Search;
            NavigateNext();
        }
    }
}
