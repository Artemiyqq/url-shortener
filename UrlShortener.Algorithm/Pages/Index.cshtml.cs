using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrlShortener.Algorithm.Models;
using UrlShortener.Algorithm.Services.Contracts;

namespace UrlShortener.Algorithm.Pages
{
    public class IndexModel(IAlgorithmSettingsService algorithmSettingsService, IAuthService authService) : PageModel
    {
        private readonly IAlgorithmSettingsService _algorithmSettingsService = algorithmSettingsService;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public bool IsAuthenticated { get; set; } = false;

        [BindProperty]
        public AlgorithmSettings AlgorithmSettingsProperty { get; set; } = new ();

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                string? token = Request.Cookies["AuthToken"];
                if (token is not null)
                {
                    bool isTokenValid = await _authService.IsTokenValidAsync(token);
                    if (!isTokenValid)
                    {
                        return Redirect("/Login");
                    }
                    else IsAuthenticated = true;
                }

                AlgorithmSettingsProperty = await _algorithmSettingsService.GetAlgorithmSettingsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while trying to get algorithm settings";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string? token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "You must be logged in to update algorithm settings.";
                    return Page();
                }

                await _algorithmSettingsService.UpdateAlgorithmSettingsAsync(AlgorithmSettingsProperty, token);
            
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
