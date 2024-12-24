using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Controllers;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;
using Newtonsoft.Json.Linq;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock = null!;
    private Mock<ITokenService> _tokenServiceMock = null!;
    private AuthController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _controller = new AuthController(_authServiceMock.Object, _tokenServiceMock.Object);
    }

    [Test]
    public async Task Login_ShouldReturnTokenOnValidLogin()
    {
        // Arrange
        var accountDto = new AccountDto { Login = "user", Password = "pass" };
        var account = new Account { Id = 1, Name = "User", Login = "user", Password = "pass", IsAdmin = false };
        var token = "mocked-jwt-token"; 

        _authServiceMock.Setup(s => s.LoginAsync(accountDto)).ReturnsAsync(account);
        _tokenServiceMock.Setup(s => s.GenerateToken(account.Id, account.IsAdmin)).Returns(token);

        // Act
        var result = await _controller.Login(accountDto);

        // Assert
        var okResult = result as OkObjectResult;
        // Parse the result value to TokenDto


        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));
            var value = JObject.FromObject(okResult.Value!);
            Assert.That(value["token"]?.ToString(), Is.EqualTo(token));
        });
    }

    [Test]
    public async Task Login_ShouldReturnBadRequestOnInvalidCredentials()
    {
        // Arrange
        var accountDto = new AccountDto { Login = "user", Password = "wrong" };
        _authServiceMock.Setup(s => s.LoginAsync(accountDto)).ThrowsAsync(new Exception("Invalid credentials"));

        // Act
        var result = await _controller.Login(accountDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult!.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public async Task Register_ShouldReturnCreatedOnValidRegistration()
    {
        // Arrange
        var registerDto = new RegisterDto { Name = "User", Login = "user", Password = "password" };

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var createdResult = result as NoContentResult;
        Assert.Multiple(() =>
        {
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult!.StatusCode, Is.EqualTo(204));
        });
    }

    [Test]
    public async Task CheckToken_ShouldReturnOkForValidToken()
    {
        // Arrange
        var tokenDto = new TokenDto { Token = "valid-token" };
        _tokenServiceMock.Setup(s => s.IsValidToken(tokenDto.Token)).Returns(true);

        // Act
        var result = await _controller.CheckToken(tokenDto);

        // Assert
        var okResult = result as OkResult;
        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public async Task CheckToken_ShouldReturnBadRequestForInvalidToken()
    {
        // Arrange
        var tokenDto = new TokenDto { Token = "invalid-token" };
        _tokenServiceMock.Setup(s => s.IsValidToken(tokenDto.Token)).Returns(false);

        // Act
        var result = await _controller.CheckToken(tokenDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult!.StatusCode, Is.EqualTo(400));
        });
    }
}