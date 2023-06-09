using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConvenienceStoreManagement.Components.ViewModel;
using ConvenienceStoreManagement.Model;
using ConvenienceStoreManagement.Model.Control;
using ConvenienceStoreManagement.Model.Product;
using ConvenienceStoreManagement.Tools;
using ConvenienceStoreManagement.Tools.Printing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace ConvenienceStoreManagement.Main.ViewModel
{
    public partial class InvoicePrintViewModel : ViewModelBase, IProductVisitor
    {
        public static string InvoicesFolder { get; private set; } = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ConvenienceStoreManagement\Invoices\";
        public CreateInvoiceResult Result { get; private set; } = CreateInvoiceResult.Cancel;

        public InvoicePrintViewModel(ObservableCollection<CartItemViewModel> list, CustomerModel? customer)
        {
            CartList = list;
            Customer = customer;
            foreach (var item in list)
            {
                TotalCost += item.TotalCost;
            }
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InvoiceID))]
        [NotifyPropertyChangedFor(nameof(InvoiceDate))]
        private InvoiceModel invoice;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomerID))]
        [NotifyPropertyChangedFor(nameof(CustomerName))]
        private CustomerModel? customer;

        [ObservableProperty]
        private ObservableCollection<CartItemViewModel> cartList;

        public string InvoiceID
            => Invoice != null ? Invoice.Id.ToString() : "GENERATE";
        public string InvoiceDate
            => Invoice != null ? Invoice.PurchaseTime.ToString() : "GENERATE";
        public string CustomerID
            => Customer != null ? Customer.Id.ToString() : "GUEST";
        public string CustomerName
            => Customer != null ? Customer.FullName : "GUEST";

        [ObservableProperty]
        private int totalCost = 0;

        public Border InvoicePanel { get; set; }

        [RelayCommand]
        public async void PrintInvoice()
        {
            if (dbManager == null) return;
            if (dbManager.QueryInvoice == null) return;

            var result = await dbManager.QueryInvoice.CreateNewInvoice(
                Customer != null ? Customer.Id : 0,
                dbManager.WorkingEmployee.Id,
                TotalCost
            );
            result.ToSingle();

            if (result["error"] is not null)
            {
                Result = CreateInvoiceResult.Fail;
                ViewWindow?.Close();
                return;
            }

            if (result["data"] is Dictionary<string, object> data)
            {
                string date = Utils.GetCurrentDate();
                string dateFolder = InvoicesFolder + "\\" + date;
                Invoice = new(data);
                UpdateEachGood(Invoice.Id);
                Directory.CreateDirectory(dateFolder);

                AvaloniaPrinter.ToFile(
                    @$"{dateFolder}\bill_{Invoice.Id}_{CustomerID}.pdf",
                    InvoicePanel
                );
                Process.Start(new ProcessStartInfo
                {
                    FileName = @$"{dateFolder}\bill_{Invoice.Id}_{CustomerID}.pdf",
                    UseShellExecute = true
                });

                Result = CreateInvoiceResult.Success;
                ViewWindow?.Close();
            }
        }

        public async void UpdateEachGood(int invoiceID)
        {
            foreach (var item in CartList)
            {
                foreach (var good in item.Goods)
                {
                    good.InvoiceID = invoiceID;
                    good.AcceptVisitor(this);
                }
            }
        }

        public async void BoughtSimpleProduct(SimpleProduct product)
        {
            await dbManager.QueryInvoice.
                UpdateGoodAfterPurchased(product.Id, product.InvoiceID, product.Item.GetCost());
        }

        public async void BoughtNoScanProduct(NoScanProduct product)
        {
            await dbManager.QueryInvoice
                .UpdateNoScanGoodAfterPurchased(product.Id, product.InvoiceID, product.Item.GetCost());
        }
        public async void BoughtWeightProduct(WeightProduct product)
        {
            await dbManager.QueryInvoice
                .UpdateWeightProductAfterPurchased(product.Id, product.InvoiceID, product.Item.GetCost());
        }
    }
}
