using System;

namespace ConvenienceStoreManagement.Model
{
    public class InvoiceModel
    {
        public int Id { get; private set; }
        public string CustomerID { get; private set; } = string.Empty;
        public string StaffID { get; private set; } = string.Empty;
        public DateTime PurchaseDate { get; private set; }
        public int TotalCost { get; private set; }
    }
}
