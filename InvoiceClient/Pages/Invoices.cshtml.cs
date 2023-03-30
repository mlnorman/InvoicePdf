using InvoiceClient.Models;
using InvoiceClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace InvoiceClient.Pages
{
    public class InvoicesModel : PageModel
    {
        private readonly IInvoiceService _invoiceService;
        
        public List<Invoice>? Invoices { get; set; } = new List<Invoice>();
        
        public InvoicesModel(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task OnGetAsync()
        {
            //get all invoices
            Invoices = await _invoiceService.GetAllInvoices();
        }

        public async Task<IActionResult> OnGetDownload(int invoiceId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // making an assumption there will always be an associated document for the invoice
            var document = await _invoiceService.GetDocumentForInvoice(invoiceId, accessToken);

            // not setting a file name so it opens in the browser.  
            // uncomment this and remove target="_blank" from the action to just 
            // open file in a new tab 
            //return File(document, "application/pdf");          

            // This causes the file dialog to open so you can just save it
            return File(document, MediaTypeNames.Application.Pdf, $"Invoice_{invoiceId}.pdf");          
        }



    }
}
