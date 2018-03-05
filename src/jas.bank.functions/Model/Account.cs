namespace jas.bank.functions.model
{
    public class Account
    {
        public string accountNumber { get; set; }
        public string customerId { get; set; }
        public string ownerCustomerId { get; set;  }
        public string name { get; set; }
        public string accountType { get; set; }
        public decimal available { get; set; }
        public decimal balance { get; set; }
        public decimal creditLimit { get; set; }
        public bool defaultAccount { get; set; }
    }
}