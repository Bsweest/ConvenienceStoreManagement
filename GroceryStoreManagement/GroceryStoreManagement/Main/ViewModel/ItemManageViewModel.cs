using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class ItemManageViewModel : PageViewModelBase
    {
        public static string ImageFolder { get; private set; } = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ConvenienceStoreManagement\";

        [ObservableProperty]
        private string choosedFolder = "";

        public ItemManageViewModel()
        {
            ChoosedFolder = ImageFolder;
        }

        [ObservableProperty]
        private List<GroupItemViewModel> gridItems = new();

        public void LoadLocalsetting()
        {

        }
    }
}
