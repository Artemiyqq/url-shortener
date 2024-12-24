using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services.Contracts
{
    public interface IAuthService
    {
        Task<Account> LoginAsync(AccountDto accountDto);
        Task RegisterAsync(RegisterDto registerDto);
        Task RegisterAdminAsync(RegisterDto registerDto);
        
    }
}