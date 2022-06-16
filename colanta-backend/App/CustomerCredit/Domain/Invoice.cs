namespace colanta_backend.App.CustomerCredit.Domain
{
    public class Invoice
    {
            public string id   {get; set;}
            public string friendlyId {get; set;}
            public string status {get; set;}
            public decimal value {get; set;}
            public string accountId {get; set;}
            public decimal creditValue {get; set;}
            public string createdAt {get; set;}
            public string updatedAt {get; set;}
            public string? resolvedAt { get; set;}
            public string originalDueDate {get; set;}
            public string dueDate {get; set;}
            public int installment {get; set;}
            public string transactionId {get; set;}
            public int numberOfInstallments {get; set;}
            public string creditAccountId { get; set; }
    }
}
