using Moq;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Implementations;
using UrlShortener.Api.Services.Contracts;
using UrlShortener.Api.Data;
using UrlShortener.Api.Tests.TestUtilities;

namespace UrlShortener.Api.Tests.Services
{
    [TestFixture]
    public class UrlShortenerServiceTests
    {
        private UrlShortenerDbContext _dbContext = null!;
        private Mock<IAlgorithmSettingsService> _mockAlgorithmSettingsService = null!;
        private UrlShortenerService _urlShortenerService = null!;

        [SetUp]
        public void SetUp()
        {
            _dbContext = TestDbContextFactory.CreateInMemoryDbContext();
            _mockAlgorithmSettingsService = new Mock<IAlgorithmSettingsService>();
            _urlShortenerService = new UrlShortenerService(_dbContext, _mockAlgorithmSettingsService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldCreateShortenedUrl_WhenValidInput()
        {
            // Arrange
            var longUrl = "https://example.com";
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            _mockAlgorithmSettingsService
                .Setup(service => service.GetAlgorithmSettingsAsync())
                .ReturnsAsync(new AlgorithmSettings
                {
                    Length = 6,
                    IncludeDigits = true,
                    IncludeLowerLetters = true,
                    IncludeUpperLetters = false
                });

            // Act
            var result = await _urlShortenerService.ShortenUrlAsync(longUrl, account.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.LongUrl, Is.EqualTo(longUrl));
            });
        }

        [Test]
        public async Task ShortenUrlAsync_ShouldThrowException_WhenUrlAlreadyExists()
        {
            // Arrange
            var longUrl = "https://example.com";
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            _dbContext.ShortenedUrls.Add(new ShortenedUrl(longUrl, "short1", account.Id));
            await _dbContext.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _urlShortenerService.ShortenUrlAsync(longUrl, account.Id));
            Assert.That(ex!.Message, Is.EqualTo("URL already exists"));
        }

        [Test]
        public async Task GetUrlByIndexAsync_ShouldReturnShortenedUrl_WhenValidId()
        {
            // Arrange
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            var shortenedUrl = new ShortenedUrl("https://example.com", "short1", account.Id);
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _urlShortenerService.GetUrlByIndexAsync(shortenedUrl.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.LongUrl, Is.EqualTo(shortenedUrl.LongUrl));
                Assert.That(result.ShortUrl, Is.EqualTo(shortenedUrl.ShortUrl));
            });
        }

        [Test]
        public async Task GetUrlByIndexAsync_ShouldThrowException_WhenInvalidId()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _urlShortenerService.GetUrlByIndexAsync(999));
            Assert.That(ex!.Message, Is.EqualTo("Invalid URL id"));
        }

        [Test]
        public async Task GetLongUrlAsync_ShouldReturnOriginalUrl_WhenShortUrlExists()
        {
            // Arrange
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            var shortenedUrl = new ShortenedUrl("https://example.com", "short1", account.Id);
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _urlShortenerService.GetLongUrlAsync("short1");

            // Assert
            Assert.That(result, Is.EqualTo("https://example.com"));
        }

        [Test]
        public async Task GetLongUrlAsync_ShouldIncrementUsageCount_WhenShortUrlExists()
        {
            // Arrange
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            var shortenedUrl = new ShortenedUrl("https://example.com", "short1", account.Id) { UsageCount = 0 };
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            // Act
            await _urlShortenerService.GetLongUrlAsync("short1");

            // Assert
            var updatedUrl = await _dbContext.ShortenedUrls.FirstAsync(x => x.ShortUrl == "short1");
            Assert.That(updatedUrl.UsageCount, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteUrlByIndexAsync_ShouldRemoveUrl_WhenValidId()
        {
            // Arrange
            var account = new Account { Id = 1, Name = "Test User", Login = "test", Password = "password" };
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            var shortenedUrl = new ShortenedUrl("https://example.com", "short1", account.Id);
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            // Act
            await _urlShortenerService.DeleteUrlByIndexAsync(shortenedUrl.Id);

            // Assert
            var result = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == shortenedUrl.Id);
            Assert.That(result, Is.Null);
        }
    }
}