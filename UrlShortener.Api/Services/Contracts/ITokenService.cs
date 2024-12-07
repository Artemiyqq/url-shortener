namespace UrlShortener.Api.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(int accountId, bool isAdmin);
        bool IsValidToken(string token);
    }
}