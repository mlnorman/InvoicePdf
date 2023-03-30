using InvoiceClient.Infrastructure;
using InvoiceClient.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceClient.Services
{
    // no need to use any patterns like unit of work for a project
    // that simply does 1 read
    public class InvoiceService : IInvoiceService
    {
        private readonly InvoiceContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(InvoiceContext context, IConfiguration configuration, ILogger<InvoiceService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<Invoice>> GetAllInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        public async Task<byte[]> GetDocumentForInvoice(int invoiceId, string accessToken)
        {
            // create a new http client and set the access token in the authorization header
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var content = await client.
                GetByteArrayAsync($"{_configuration.GetValue<string>("InvoiceApiPath")}{invoiceId}");

                return content;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting document from API call {Environment.NewLine}{ex.Message}");
                throw;
            }

        }
    }
}
