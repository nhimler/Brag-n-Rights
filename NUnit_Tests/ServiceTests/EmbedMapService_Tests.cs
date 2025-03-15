using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Service_Tests
{
    public class GoogleMapsService_Tests
    {
        private Mock<HttpClient> _mockHttpClient;
        private Mock<ILogger<GoogleMapsService>> _mockILogger;
        private GoogleMapsService _googleMapsService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockILogger = new Mock<ILogger<GoogleMapsService>>();
            _googleMapsService = new GoogleMapsService(_mockHttpClient.Object, _mockILogger.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            var expectedApiKey = "test-api-key";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-goog-api-key", expectedApiKey);
            _googleMapsService = new GoogleMapsService(httpClient, _mockILogger.Object);

            // Act
            var apiKey = await _googleMapsService.GetGoogleMapsApiKey();

            // Assert
            Assert.That(expectedApiKey, Is.EqualTo(apiKey));
        }
    }
}