using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;

namespace UrlShortener.Api.Tests.TestUtilities
{
    public static class TestDbContextFactory
    {
        public static UrlShortenerDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UrlShortenerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new UrlShortenerDbContext(options);
        }
    }
}
