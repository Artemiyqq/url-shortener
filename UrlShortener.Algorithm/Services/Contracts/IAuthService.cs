namespace UrlShortener.Algorithm.Services.Contracts
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string login, string password);
        Task<bool> IsTokenValidAsync(string token);
    }
}