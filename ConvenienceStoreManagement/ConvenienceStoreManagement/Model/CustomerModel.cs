namespace ConvenienceStoreManagement.Model
{
    public class CustomerModel
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public int TotalSpending { get; private set; }
        public int Balance { get; private set; }

        public CustomerModel() { }
    }
}
