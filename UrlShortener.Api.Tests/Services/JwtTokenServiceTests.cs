using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UrlShortener.Api.Services.Implementations;

namespace UrlShortener.Api.Tests.Services
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private JwtTokenService _service = null!;
        private Mock<IConfiguration> _mockConfiguration = null!;
        private Mock<ILogger<JwtTokenService>> _mockLogger = null!;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<JwtTokenService>>();

            _mockConfiguration.SetupGet(c => c["JWT:Key"]).Returns("+AmTBn6Toddu5oGaYEuEo/vkTLiWBNo6XJxRZ5XN0DQ=");
            _mockConfiguration.SetupGet(c => c["JWT:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.SetupGet(c => c["JWT:Audience"]).Returns("TestAudience");

            _service = new JwtTokenService(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Test]
        public void GenerateToken_ShouldReturnValidToken()
        {
            // Act
            var token = _service.GenerateToken(1, true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(token, Is.Not.Null);
                Assert.That(token, Is.InstanceOf<string>());
            });
        }

        [Test]
        public void IsValidToken_ShouldReturnTrue_ForValidToken()
        {
            // Arrange
            var token = _service.GenerateToken(1, true);

            // Act
            var isValid = _service.IsValidToken(token);

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void IsValidToken_ShouldReturnFalse_ForInvalidToken()
        {
            // Act
            var isValid = _service.IsValidToken("invalid_token");

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
