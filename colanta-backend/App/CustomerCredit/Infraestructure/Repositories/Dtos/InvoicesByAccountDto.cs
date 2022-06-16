namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using CustomerCredit.Domain;
    public class InvoicesByAccountDto
    {
        public InvoiceDto[] data { get; set; }
    }

    public class InvoiceDto
    {
        public string id {get; set;}
        public string friendlyId {get; set;}
        public string status {get; set;}
        public decimal value {get; set;}
        public string accountId {get; set;}
        public decimal creditValue {get; set;}
        public string createdAt{get; set;}
        public string resolvedAt {get; set;}
        public string updatedAt {get; set;}
        public string originalDueDate {get; set;}
        public string dueDate {get; set;}
        public int installment {get; set;}
        public string orderId {get; set;}
        public string transactionId {get; set;}
        public int numberOfInstallments {get; set;}
        public string creditAccountId { get; set; }

        public Invoice getInvoiceFromDto()
        {
            Invoice invoice = new Invoice();
            invoice.id = id;
            invoice.friendlyId = friendlyId;
            invoice.status = status;
            invoice.value = value;
            invoice.accountId = accountId;
            invoice.creditValue = creditValue;
            invoice.dueDate = dueDate;
            invoice.installment = installment;
            invoice.createdAt = createdAt;
            invoice.updatedAt = updatedAt;
            invoice.originalDueDate = originalDueDate;
            invoice.resolvedAt = resolvedAt;
            invoice.transactionId = transactionId;
            invoice.numberOfInstallments = numberOfInstallments;
            invoice.creditAccountId = creditAccountId;
            return invoice;
        }
    }
}
