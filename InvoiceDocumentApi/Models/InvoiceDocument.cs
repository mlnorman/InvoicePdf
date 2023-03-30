using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceDocumentApi.Models
{
    public class InvoiceDocument
    {

        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public byte[] PdfDocument { get; set; }

    }
}
