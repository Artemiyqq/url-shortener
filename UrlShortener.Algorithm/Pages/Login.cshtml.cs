using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrlShortener.Algorithm.Services.Contracts;

namespace UrlShortener.Algorithm.Pages
{
    public class LogInModel(IAuthService authService) : PageModel
    {
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public string Login { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string token = await _authService.LoginAsync(Login, Password);
                CookieOptions cookieOptions = new()
                { 
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(14)
                };
                Response.Cookies.Append("AuthToken", token, cookieOptions);
                return RedirectToPage("/Index");
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Error connecting to the API: " + ex.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
