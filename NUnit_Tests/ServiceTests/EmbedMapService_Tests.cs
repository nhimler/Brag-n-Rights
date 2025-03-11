using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace NUnit_Tests.Service_Tests
{
    public class EmbedMapService_Tests
    {
        private Mock<HttpClient> _mockHttpClient;
        private Mock<ILogger<EmbedMapService>> _mockILogger;
        private EmbedMapService _embedMapService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockILogger = new Mock<ILogger<EmbedMapService>>();
            _embedMapService = new EmbedMapService(_mockHttpClient.Object, _mockILogger.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            var expectedApiKey = "test-api-key";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-goog-api-key", expectedApiKey);
            _embedMapService = new EmbedMapService(httpClient, _mockILogger.Object);

            // Act
            var apiKey = await _embedMapService.GetGoogleMapsApiKey();

            // Assert
            Assert.That(expectedApiKey, Is.EqualTo(apiKey));
        }
    }
}