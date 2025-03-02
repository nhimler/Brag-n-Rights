using GymBro_App.Controllers;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControllerTests
{
    public class GoogleMapsAPIController_Tests
    {
        private Mock<IMapService> _mockMapService;
        private Mock<ILogger<GoogleMapsAPIController>> _mockILogger;
        private GoogleMapsAPIController _googleMapsAPIController;

        [SetUp]
        public void SetUp()
        {
            _mockMapService = new Mock<IMapService>();
            _mockILogger = new Mock<ILogger<GoogleMapsAPIController>>();
            _googleMapsAPIController = new GoogleMapsAPIController(_mockMapService.Object, _mockILogger.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            var expectedApiKey = "test-api-key";
            var expectedStatusCode = 200;
            _mockMapService.Setup(service => service.GetGoogleMapsApiKey()).ReturnsAsync(expectedApiKey);

            // Act
            var result = await _googleMapsAPIController.GetGoogleMapsApiKey() as OkObjectResult;
            var apiKeyResult = result?.Value ?? new {};
            var apiKeyProperty = apiKeyResult.GetType().GetProperty("apiKey");
            var apiKeyValue = apiKeyProperty?.GetValue(apiKeyResult);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(expectedStatusCode, Is.EqualTo(result.StatusCode));
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(expectedApiKey, Is.EqualTo(apiKeyValue));
        }
    }
}