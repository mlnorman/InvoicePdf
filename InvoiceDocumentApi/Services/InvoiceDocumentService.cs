using InvoiceDocumentApi.Infrastructure;
using InvoiceDocumentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceDocumentApi.Services
{
    // no need to any patterns like unit of work for a project
    // that simply does 1 read.
    public class InvoiceDocumentService : IInvoiceDocumentService
    {
        private readonly InvoiceDocumentContext _context;
        private readonly ILogger<InvoiceDocumentService> _logger;

        public InvoiceDocumentService(InvoiceDocumentContext context,ILogger<InvoiceDocumentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InvoiceDocument> GetDocumentByInvoiceId(int invoiceId)
        {
            _logger.LogDebug($"Retreiving document for invoice id {invoiceId}.");

            return await _context.InvoiceDocuments.FindAsync(invoiceId);
        }
    }
}
