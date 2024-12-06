using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Services.Implementations
{
    public class AuthService(UrlShortenerDbContext context): IAuthService
    {
        private readonly UrlShortenerDbContext _context = context;
        public async Task<int> Login(AccountDto accountDto)
        {
            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Login == accountDto.Login && 
                                                                                a.Password == accountDto.Password);
            if (account is null) return -1;

            return account.Id;
        }
    }
}