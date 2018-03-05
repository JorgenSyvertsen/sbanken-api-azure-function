using System;

namespace jas.bank.functions.model
{
    public class CardDetails
    {
        public string cardNumber { get; set; }
        public decimal currencyAmount { get; set; }
        public decimal currencyRate { get; set; }
        public string merchantCategoryCode { get; set; }
        public string merchantCategoryDescription { get; set; }
        public string merchantCity { get; set; }
        public string merchantName { get; set; }
        public string originalCurrencyCode { get; set; }
        public DateTime purchaseDate { get; set; }
        public string transactionId { get; set; }
    }
}