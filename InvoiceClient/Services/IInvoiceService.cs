using InvoiceClient.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceClient.Services
{
    public interface IInvoiceService
    {
        Task<List<Invoice>> GetAllInvoices();

        Task<byte[]> GetDocumentForInvoice(int invoiceId, string accessToken);
    }
}
