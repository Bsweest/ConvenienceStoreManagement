using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class InvoiceModel
    {
        public int Id { get; private set; }
        public int CustomerID { get; private set; }
        public int StaffID { get; private set; }
        public DateTime PurchaseTime { get; private set; }
        public int TotalCost { get; private set; }

        public InvoiceModel(Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            CustomerID = (int)data["cus_id"];
            StaffID = (int)data["staff_id"];
            TotalCost = (int)data["total_cost"];
            PurchaseTime = (DateTime)data["purchase_time"];
        }
    }
}
