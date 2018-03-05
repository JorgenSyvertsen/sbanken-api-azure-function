using System;

namespace jas.bank.functions.model
{
    public class Transaction
    {
        public DateTime accountingDate { get; set; }
        public DateTime interestDate { get; set; }
        public string otherAccountNumber { get; set; }
        public bool otherAccountNumberSpecified { get; set; }
        public decimal amount { get; set; }
        public string text { get; set; }
        public string transactionType { get; set; }
        public int transactionTypeCode { get; set; }
        public string transactionTypeText { get; set; }
        public bool isReservation { get; set; }
        public string reservationType { get; set; }
        public string source { get; set; }
        public CardDetails cardDetails { get; set; }
        public bool cardDetailsSpecified { get; set; }
    }
}