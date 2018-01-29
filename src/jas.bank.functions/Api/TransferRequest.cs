namespace jas.bank.functions.api
{
    public class TransferRequest
    {
        public string fromAccount { get; set; }
        public string toAccount { get; set; }
        public string message { get; set; }
        public decimal amount { get; set; }
    }
}