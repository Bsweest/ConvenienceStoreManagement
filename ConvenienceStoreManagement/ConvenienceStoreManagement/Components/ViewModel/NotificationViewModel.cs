using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class NotificationViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<KeyValuePair<string, bool>> notiMessages = new();

        public void NotifyMessage(string msg, bool isError = false)
        {
            var noti = new KeyValuePair<string, bool>(msg, isError);
            NotiMessages.Add(noti);
            Task.Delay(5000).ContinueWith(_ => NotiMessages.Remove(noti));
        }

    }
}
