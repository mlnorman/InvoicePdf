

namespace InvoiceClient.Models
{
    public class Invoice 
    {

        public int InvoiceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime InvoicePeriodStart { get; set; }

        public DateTime InvoicePeriodEnd { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public bool DocumentExists { get; set; }
    }
}
