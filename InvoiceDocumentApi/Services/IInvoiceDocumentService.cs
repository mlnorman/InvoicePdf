using InvoiceDocumentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceDocumentApi.Services
{
    public interface IInvoiceDocumentService
    {
        Task<InvoiceDocument> GetDocumentByInvoiceId(int invoiceId);
    }
}
