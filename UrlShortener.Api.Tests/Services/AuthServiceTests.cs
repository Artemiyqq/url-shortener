using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Implementations;
using UrlShortener.Api.Tests.TestUtilities;

namespace UrlShortener.Api.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private UrlShortenerDbContext _context = null!;
        private AuthService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new AuthService(_context, MockLogger<AuthService>.Create());

            // Seed data
            _context.Accounts.Add(new Account
            {
                Name = "TestUser",
                Login = "testuser",
                Password = new Microsoft.AspNetCore.Identity.PasswordHasher<string>().HashPassword("TestUser", "password")
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task LoginAsync_ReturnsAccount_ForValidCredentials()
        {
            // Arrange
            var loginDto = new AccountDto { Login = "testuser", Password = "password" };

            // Act
            var account = await _service.LoginAsync(loginDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account, Is.Not.Null);
                Assert.That(account.Name, Is.EqualTo("TestUser"));
            });
        }

        [Test]
        public void LoginAsync_ThrowsArgumentException_ForInvalidCredentials()
        {
            // Arrange
            var loginDto = new AccountDto { Login = "testuser", Password = "wrongpassword" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.LoginAsync(loginDto));
        }

        [Test]
        public async Task RegisterAsync_AddsNewAccount()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "NewUser",
                Login = "newuser",
                Password = "newpassword"
            };

            // Act
            await _service.RegisterAsync(registerDto);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Login == "newuser");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(account, Is.Not.Null);
                Assert.That(account!.Name, Is.EqualTo("NewUser"));
            });
        }
    }
}
