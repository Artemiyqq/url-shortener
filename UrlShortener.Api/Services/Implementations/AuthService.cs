using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;
using Microsoft.AspNetCore.Identity;


namespace UrlShortener.Api.Services.Implementations
{
    public class AuthService(UrlShortenerDbContext context, ILogger<AuthService> logger) : IAuthService
    {
        private readonly UrlShortenerDbContext _context = context;
        private readonly PasswordHasher<string> _passwordHasher = new();
        private readonly ILogger<AuthService> _logger = logger;

        private const string InvalidCredentialsMessage = "Invalid credentials";

        public async Task<Account> LoginAsync(AccountDto accountDto)
        {
            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Login == accountDto.Login);

            if (account == null)
            {
                _logger.LogWarning("Login failed for user: {Login}", accountDto.Login);
                throw new ArgumentException(InvalidCredentialsMessage);
            }

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(account.Name,
                                                                                                 account.Password,
                                                                                                 accountDto.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                throw new ArgumentException(InvalidCredentialsMessage);
            }

            return account;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Accounts.AnyAsync(a => a.Login == registerDto.Login)) throw new ArgumentException("Login already exists");

            Account account = new()
            {
                Name = registerDto.Name,
                Login = registerDto.Login,
                Password = _passwordHasher.HashPassword(registerDto.Name, registerDto.Password),
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task RegisterAdminAsync(RegisterDto registerDto)
        {
            await RegisterAsync(registerDto);
            Account account = await _context.Accounts.FirstAsync(x => x.Login == registerDto.Login);

            account.IsAdmin = true;
            await _context.SaveChangesAsync();
            return;
        }
    }
}