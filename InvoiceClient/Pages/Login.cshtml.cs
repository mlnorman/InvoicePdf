using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceClient.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet() => Challenge(new AuthenticationProperties
        {
            // always redirect to index after login.  Keep it simple
            RedirectUri = "/"
        });
    }
}
