using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class CustomerModel
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string PersonId { get; private set; } = string.Empty;
        public int TotalSpending { get; private set; }
        public int Balance { get; private set; }

        public CustomerModel(Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            FullName = (string)data["fullname"];
            PhoneNumber = (string)data["phonenum"];
            if (data["person_id"] is string personalId) PersonId = personalId;
            if (data["total_spending"] is int totalSpend) TotalSpending = totalSpend;
            if (data["balance"] is int balance) Balance = balance;
        }

        public string? GetProperty(string propertyName)
        {
            if (propertyName == "ID") return Id.ToString();
            if (propertyName == "FullName") return FullName;
            if (propertyName == "PhoneNumber") return PhoneNumber;
            if (propertyName == "PersonID") return PersonId;
            return null;
        }
    }
}
