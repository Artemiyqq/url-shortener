using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Implementations;
using UrlShortener.Api.Tests.TestUtilities;

namespace UrlShortener.Api.Tests.Services
{
    [TestFixture]
    public class AlgorithmSettingsServiceTests
    {
        private UrlShortenerDbContext _context = null!;
        private AlgorithmSettingsService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new AlgorithmSettingsService(_context);

            // Seed data
            _context.AlgorithmSettings.Add(new AlgorithmSettings
            {
                Id = 1,
                Length = 8,
                IncludeDigits = true,
                IncludeLowerLetters = true,
                IncludeUpperLetters = false
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAlgorithmSettingsAsync_ReturnsAlgorithmSettings()
        {
            // Act
            var result = await _service.GetAlgorithmSettingsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Length, Is.EqualTo(8));
                Assert.That(result.IncludeDigits, Is.True);
                Assert.That(result.IncludeUpperLetters, Is.False);
            });
        }

        [Test]
        public async Task UpdateAlgorithmSettingsAsync_UpdatesSettingsSuccessfully()
        {
            // Arrange
            var newSettings = new AlgorithmSettings
            {
                Id = 1,
                Length = 10,
                IncludeDigits = false,
                IncludeLowerLetters = true,
                IncludeUpperLetters = true
            };

            // Act
            await _service.UpdateAlgorithmSettingsAsync(newSettings);
            var updatedSettings = await _service.GetAlgorithmSettingsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedSettings.Length, Is.EqualTo(10));
                Assert.That(updatedSettings.IncludeUpperLetters, Is.True);
                Assert.That(updatedSettings.IncludeDigits, Is.False);
            });
        }

        [Test]
        public void UpdateAlgorithmSettingsAsync_ThrowsArgumentException_ForInvalidSettings()
        {
            // Arrange
            var invalidSettings = new AlgorithmSettings
            {
                Id = 1,
                Length = 10,
                IncludeDigits = false,
                IncludeLowerLetters = false,
                IncludeUpperLetters = false
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAlgorithmSettingsAsync(invalidSettings));
        }
    }
}
