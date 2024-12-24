using Microsoft.Extensions.Logging;
using Moq;

namespace UrlShortener.Api.Tests.TestUtilities
{
    public static class MockLogger<T>
    {
        public static ILogger<T> Create()
        {
            return new Mock<ILogger<T>>().Object;
        }
    }
}