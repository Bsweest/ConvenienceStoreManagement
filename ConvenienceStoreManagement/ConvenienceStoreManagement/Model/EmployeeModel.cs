using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ConvenienceStoreManagement.Model
{
    //id SERIAL primary key,
    //name text not null,
    //phonenum text not null unique,
    //contract_id integer references contract,
    //person_id text unique,
    //username text unique,
    //password text
    public partial class EmployeeModel : ObservableObject
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string PersonId { get; private set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Salary))]
        private ContractModel? contract;
        public string? Username { get; private set; }
        public string? Password { get; private set; }

        public EmployeeModel() { }

        public string Salary
            => Contract != null ? Contract.Salary.ToString() : "No Contract";

        public EmployeeModel(Dictionary<string, object> data, ContractModel? cont = null)
        {
            Id = (int)data["id"];
            Name = (string)data["name"];
            PhoneNumber = (string)data["phonenum"];
            if (data["person_id"] is string pid) PersonId = pid;
            if (data["username"] is string user) Username = user;
            if (data["password"] is string pass) Password = pass;
            Contract = cont;
        }

        public void SetupContract(ContractModel? cont = null)
        {
            Contract = cont;
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
