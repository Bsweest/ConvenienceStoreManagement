using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Tools;
using ConvenienceStoreManagement.Tools.Printing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class InvoicePrintViewModel : ViewModelBase
    {
        public InvoicePrintViewModel() { }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InvoiceID))]
        [NotifyPropertyChangedFor(nameof(InvoiceDate))]
        private InvoiceModel invoiceModel;

        [ObservableProperty]
        private CustomerModel customer = new();

        [ObservableProperty]
        private List<GroupItemViewModel> cartList;

        public string InvoiceID
            => InvoiceModel != null ? InvoiceModel.Id.ToString() : "GENERATE";
        public string InvoiceDate
            => InvoiceModel != null ? InvoiceModel.PurchaseDate.ToString() : "GENERATE";

        public Border InvoicePanel { get; set; }

        public void PrintInvoice()
        {
            string date = Utils.GetCurrentDate();
            Directory.CreateDirectory(@$"Invoices\{date}");

            AvaloniaPrinter.ToFile(
                @$"Invoices\{date}\bill_{InvoiceID}_{Customer.Id}.pdf",
                InvoicePanel
            );
            Process.Start(new ProcessStartInfo
            {
                FileName = @$"Invoices\{date}\bill_{InvoiceID}_{Customer.Id}.pdf",
                UseShellExecute = true
            });
        }
    }
}
