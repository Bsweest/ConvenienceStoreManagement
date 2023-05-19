using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConvenienceStoreManagement.Tools
{
    public partial class DebounceSearch : ObservableObject
    {
        private CancellationTokenSource? cancelToken;
        private readonly Action<string, string>? FuncSearchCall;
        private readonly int DebounceTime = 250;

        [ObservableProperty]
        private string? queryParam = null;
        [ObservableProperty]
        private string[] filterSearchKeys;
        [ObservableProperty]
        private string selectedFilter;

        public DebounceSearch
            (Action<string, string>? funcSearchCall, string[] keys, int selectedIndex = 0, int debounceTime = 250)
        {
            FuncSearchCall = funcSearchCall;
            DebounceTime = debounceTime;
            FilterSearchKeys = keys;
            SelectedFilter = FilterSearchKeys[selectedIndex];
        }

        partial void OnQueryParamChanged(string? value) => StartDebounce();

        partial void OnSelectedFilterChanged(string value) => StartDebounce();

        public void StartDebounce()
        {
            if (QueryParam == null) return;
            if (FuncSearchCall == null) return;
            cancelToken?.Cancel();
            cancelToken = new();
            Task.Delay(DebounceTime, cancelToken.Token)
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        FuncSearchCall(QueryParam, SelectedFilter);
                    }
                }, TaskScheduler.Default);
        }
    }
}
