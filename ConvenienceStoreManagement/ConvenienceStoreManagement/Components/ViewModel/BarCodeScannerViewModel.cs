using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ShowBox;
using ConvenienceStoreManagement.Components.ViewModel.Base;
using OpenCvSharp;
using System;
using System.ComponentModel;
using System.Threading;
using ZXing.OpenCV;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class BarCodeScannerViewModel : BaseBoxViewModel
    {
        private readonly VideoCapture Capturer;
        private readonly BackgroundWorker BgWorker;
        private BarcodeReader reader = new();

        [ObservableProperty]
        private bool scanModeAdd = true;
        [ObservableProperty]
        private bool isScanning = false;

        [ObservableProperty]
        private Bitmap scanImg;
        [ObservableProperty]
        private int choosedNumber;

        [ObservableProperty]
        private string processResult = "";

        private readonly Action<int> FuncAddCart;
        private readonly Action<int> FuncRemoveCart;

        public BarCodeScannerViewModel(Action<int> funcAddCart, Action<int> funcRemoveCart)
        {
            FuncAddCart = funcAddCart;
            FuncRemoveCart = funcRemoveCart;

            Capturer = new VideoCapture();
            Capturer.FrameWidth = 500;
            Capturer.FrameHeight = 500;

            BgWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            BgWorker.DoWork += BgWorker_Working;
        }

        [RelayCommand]
        public void ToggleScanning()
        {
            if (ViewWindow == null)
            {
                BarCodeScanner.CreateBox(this);
            }

            if (IsScanning) StartScanning();
            else StopScanning();
        }

        public void StartScanning()
        {
            if (!ViewWindow.IsActive)
            {
                ViewWindow.Show();
            }
            Capturer.Open(0, VideoCaptureAPIs.ANY);
            if (!Capturer.IsOpened())
            {
                throw new Exception("capture initialization failed");
            }
            BgWorker.RunWorkerAsync();
        }

        public void StopScanning()
        {
            IsScanning = false;
            BgWorker.CancelAsync();
            Capturer.Release();
        }

        public override void OKBehaviour()
        {
            if (ScanModeAdd) FuncAddCart(ChoosedNumber);
            else FuncRemoveCart(ChoosedNumber);
        }

        public override void CloseBehaviour()
        {
            StopScanning();
            ViewWindow?.Close();
        }

        public void FindScanValue(Mat frameMat)
        {
            if (frameMat == null) return;
            var result = reader.Decode(frameMat);
            if (result != null)
            {
                int parsed;
                bool success = int.TryParse(result.Text, out parsed);
                if (success)
                {
                    ChoosedNumber = parsed;
                    OKBehaviour();
                }
            }
        }

        public void BgWorker_Working(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                using (var frameMat = Capturer.RetrieveMat())
                {
                    FindScanValue(frameMat);
                    using var memory = frameMat.ToMemoryStream();
                    ScanImg = new Bitmap(memory);
                }

                Thread.Sleep(30);
            }
        }
    }
}
