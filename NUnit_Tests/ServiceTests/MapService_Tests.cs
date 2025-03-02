using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace NUnit_Tests.Service_Tests
{
    public class MapService_Tests
    {
        private Mock<HttpClient> _mockHttpClient;
        private Mock<ILogger<MapService>> _mockILogger;
        private MapService _mapService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockILogger = new Mock<ILogger<MapService>>();
            _mapService = new MapService(_mockHttpClient.Object, _mockILogger.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            var expectedApiKey = "test-api-key";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-goog-api-key", expectedApiKey);
            _mapService = new MapService(httpClient, _mockILogger.Object);

            // Act
            var apiKey = await _mapService.GetGoogleMapsApiKey();

            // Assert
            Assert.That(expectedApiKey, Is.EqualTo(apiKey));
        }
    }
}