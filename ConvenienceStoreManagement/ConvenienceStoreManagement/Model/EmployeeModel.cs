using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    public class EmployeeModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string PersonId { get; private set; } = string.Empty;
        public int Salary { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;

        public EmployeeModel() { }

        public EmployeeModel(Dictionary<string, object> data)
        {
            Id = (int)data["id"];
            Name = (string)data["name"];
            PhoneNumber = (string)data["phonenum"];
            Salary = (int)data["salary"];
            if (data["person_id"] is string pid) PersonId = pid;
            if (data["username"] is string user) Username = user;
            if (data["password"] is string pass) Password = pass;
        }

        public string? GetProperty(string propertyName)
        {
            if (propertyName == "ID") return Id.ToString();
            if (propertyName == "FullName") return Name;
            if (propertyName == "PhoneNumber") return PhoneNumber;
            if (propertyName == "PersonID") return PersonId;
            return null;
        }
    }
}
