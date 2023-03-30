using InvoiceDocumentApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

namespace InvoiceDocumentApi.Controllers
{
    // only allow users with admin role to access 
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class InvoiceDocumentController : Controller
    {
        private readonly IInvoiceDocumentService _documentService;
        private readonly ILogger<InvoiceDocumentController> _logger;

        public InvoiceDocumentController(IInvoiceDocumentService documentService, ILogger<InvoiceDocumentController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var invoiceDocument = await _documentService.GetDocumentByInvoiceId(id);

            if (invoiceDocument == null)
            {
                _logger.LogInformation($"Document for invoice # {id} not found.");
                return NotFound();
            }


            // returning a File, then you could get the file with something like postman
            // Could just return a Resource with the byte array for use with the client.
            return File(invoiceDocument.PdfDocument, MediaTypeNames.Application.Pdf, $"invoice_{id}.pdf");
            
        }
    }
}
