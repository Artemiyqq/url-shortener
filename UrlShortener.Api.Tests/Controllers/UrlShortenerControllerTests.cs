using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using UrlShortener.Api.Controllers;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Tests.Controllers
{
    [TestFixture]
    public class UrlShortenerControllerTests
    {
        private Mock<IUrlShortenerService> _mockUrlShortenerService = null!;
        private Mock<ITokenService> _mockTokenService = null!;
        private UrlShortenerController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _mockUrlShortenerService = new Mock<IUrlShortenerService>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new UrlShortenerController(_mockUrlShortenerService.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task GetByIndex_ValidIndex_ReturnsOkResult()
        {
            // Arrange
            int urlIndex = 1;
            var shortUrlInfoDto = new ShortUrlInfoDto {
                CreatedBy = "Test",
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                UsageCount = 0 
            };
            _mockUrlShortenerService.Setup(s => s.GetUrlInfoDtoAsync(urlIndex)).ReturnsAsync(shortUrlInfoDto);

            // Act
            var result = await _controller.GetByIndex(urlIndex);
            var okResult = result as OkObjectResult;

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(okResult!.Value, Is.EqualTo(shortUrlInfoDto));
            });
        }

        [Test]
        public async Task GetByShortUrl_ValidShortUrl_ReturnsOkResult()
        {
            // Arrange
            string shortUrl = "abc123";
            string longUrl = "https://example.com";
            _mockUrlShortenerService.Setup(s => s.GetLongUrlAsync(shortUrl)).ReturnsAsync(longUrl);

            // Act
            var result = await _controller.GetByShortUrl(shortUrl);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            Assert.That((okResult!.Value as LongUrlDto)!.LongUrl, Is.EqualTo(longUrl));
        }

        [Test]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var shortenedUrls = new List<ShortenedUrlDto> { new ShortenedUrlDto(1, "abc123", "https://example.com") };
            _mockUrlShortenerService.Setup(s => s.GetAllUrlsAsync()).ReturnsAsync(shortenedUrls);

            // Act
            var result = await _controller.GetAll();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(okResult!.Value, Is.EqualTo(shortenedUrls));
            });
        }

        [Test]
        public async Task ShortenUrl_ValidRequest_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var longUrlDto = new LongUrlDto { LongUrl = "https://example.com" };
            var shortenedUrlDto = new ShortenedUrlDto(1, "abc123", "https://example.com");
            _mockUrlShortenerService.Setup(s => s.ShortenUrlAsync(longUrlDto.LongUrl, It.IsAny<int>())).ReturnsAsync(shortenedUrlDto);

            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "1"),
                new("id", "1"),
                new(ClaimTypes.Role, "Admin")
            ], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.ShortenUrl(longUrlDto);
            var createdResult = result.Result as CreatedAtActionResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                Assert.That(createdResult!.Value, Is.EqualTo(shortenedUrlDto));
            });
        }

        [Test]
        public async Task DeleteByIndex_ValidIndex_ReturnsOkResult()
        {
            // Arrange
            int urlIndex = 1;
            var shortenedUrl = new ShortenedUrl("https://example.com", "abc123", 1);
            _mockUrlShortenerService.Setup(s => s.GetUrlByIndexAsync(urlIndex)).ReturnsAsync(shortenedUrl);
            _mockUrlShortenerService.Setup(s => s.DeleteUrlByIndexAsync(urlIndex)).Returns(Task.CompletedTask);

            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "1"),
                new("id", "1"),
                new(ClaimTypes.Role, "Admin")
            ], "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.DeleteByIndex(urlIndex);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

    }
}