using ConvenienceStoreManagement.Tools;
using System;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class ContractModel
    {
        public int Id { get; private set; }
        public int Salary { get; private set; }
        public int StaffId { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        public ContractModel(Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            Salary = (int)data["salary"];
            StaffId = (int)data["staff_id"];
            StartDate = Utils.GetDateOnlyFromDb(data["start_date"]);
            EndDate = Utils.GetDateOnlyFromDb(data["end_date"]);
        }
    }
}
