using Moq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Controllers;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Tests
{
    [TestFixture]
    public class AlgorithmSettingsControllerTests
    {
        private Mock<IAlgorithmSettingsService> _algorithmSettingsServiceMock = null!;
        private AlgorithmSettingsController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _algorithmSettingsServiceMock = new Mock<IAlgorithmSettingsService>();
            _controller = new AlgorithmSettingsController(_algorithmSettingsServiceMock.Object);
        }

        [Test]
        public async Task Get_ShouldReturnOkWithAlgorithmSettings()
        {
            // Arrange
            var algorithmSettings = new AlgorithmSettings
            {
                Length = 8,
                IncludeDigits = true,
                IncludeLowerLetters = true,
                IncludeUpperLetters = false
            };

            _algorithmSettingsServiceMock.Setup(s => s.GetAlgorithmSettingsAsync())
                .ReturnsAsync(algorithmSettings);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(algorithmSettings));
            });
        }

        [Test]
        public async Task Get_ShouldReturnBadRequestOnException()
        {
            // Arrange
            _algorithmSettingsServiceMock.Setup(s => s.GetAlgorithmSettingsAsync())
                .ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.Get();

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(badRequestResult, Is.Not.Null);
                Assert.That(badRequestResult!.StatusCode, Is.EqualTo(400));
            });
        }

        [Test]
        public async Task Update_ShouldReturnOkOnValidUpdate()
        {
            // Arrange
            var algorithm = new AlgorithmSettings
            {
                Length = 8,
                IncludeDigits = true,
                IncludeLowerLetters = true,
                IncludeUpperLetters = false
            };

            // Act
            var result = await _controller.Update(algorithm);

            // Assert
            var okResult = result as OkResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.StatusCode, Is.EqualTo(200));
            });
        }

        [Test]
        public async Task Update_ShouldReturnBadRequestOnInvalidModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Length", "Invalid length");

            var algorithm = new AlgorithmSettings
            {
                Length = 0, // Invalid length
                IncludeDigits = true
            };

            // Act
            var result = await _controller.Update(algorithm);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(badRequestResult, Is.Not.Null);
                Assert.That(badRequestResult!.StatusCode, Is.EqualTo(400));
            });
        }
    }
}