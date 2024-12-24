using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortener.Algorithm.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToPage("/Index");
        }
    }
}