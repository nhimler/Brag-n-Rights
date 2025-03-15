using GymBro_App.Controllers;
using GymBro_App.Services;
using GymBro_App.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControllerTests
{
    public class GoogleMapsAPIController_Tests
    {
        private Mock<IGoogleMapsService> _mockGoogleMapsService;
        private Mock<ILogger<GoogleMapsAPIController>> _mockILogger;
        private Mock<INearbySearchMapService> _mockNearbySearchMapService;
        private GoogleMapsAPIController _googleMapsAPIController;

        [SetUp]
        public void SetUp()
        {
            _mockGoogleMapsService = new Mock<IGoogleMapsService>();
            _mockILogger = new Mock<ILogger<GoogleMapsAPIController>>();
            _mockNearbySearchMapService = new Mock<INearbySearchMapService>();
            _googleMapsAPIController = new GoogleMapsAPIController(_mockGoogleMapsService.Object, _mockILogger.Object, _mockNearbySearchMapService.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            var expectedApiKey = "test-api-key";
            var expectedStatusCode = 200;
            _mockGoogleMapsService.Setup(service => service.GetGoogleMapsApiKey()).ReturnsAsync(expectedApiKey);

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

        [Test]
        public async Task GetNearbyPlaces_ShouldReturnAJsonObjectWithPlacesNearTheLatitudeAndLongitude()
        {
            // Arrange
            var latitude = 45.0;
            var longitude = -122.0;
            var expectedPlaces = new List<PlaceDTO>
            {
                new PlaceDTO { DisplayName = new DisplayName { Text = "Gym 1" }, FormattedAddress = "Address 1" },
                new PlaceDTO { DisplayName = new DisplayName { Text = "Gym 2" }, FormattedAddress = "Address 2" }
            };
            _mockNearbySearchMapService.Setup(service => service.FindNearbyGyms(latitude, longitude)).ReturnsAsync(expectedPlaces);

            // Act
            var result = await _googleMapsAPIController.GetNearbyPlaces(latitude, longitude) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.IsInstanceOf<OkObjectResult>(result);
            var placesResult = result.Value as List<PlaceDTO>;
            
            Assert.That(placesResult, Is.Not.Null);
            Assert.That(placesResult.Count, Is.EqualTo(expectedPlaces.Count));
            Assert.That(placesResult[0].DisplayName.Text, Is.EqualTo(expectedPlaces[0].DisplayName.Text));
            Assert.That(placesResult[0].FormattedAddress, Is.EqualTo(expectedPlaces[0].FormattedAddress));
            Assert.That(placesResult[1].DisplayName.Text, Is.EqualTo(expectedPlaces[1].DisplayName.Text));
            Assert.That(placesResult[1].FormattedAddress, Is.EqualTo(expectedPlaces[1].FormattedAddress));
        }

        [Test]
        public async Task ReverseGeocode_ShouldReturnTheFormattedAddressOfASetOfLatitudeAndLongitudeCoordinatesAsAJsonObject()
        {
            // Arrange
            var latitude = 47.7510741;
            var longitude = -120.7401385;
            var expectedAddress = "123 Main St, Test City, TS 12345";
            _mockGoogleMapsService.Setup(service => service.ReverseGeocode(latitude, longitude)).ReturnsAsync(expectedAddress);

            // Act
            var result = await _googleMapsAPIController.ReverseGeocode(latitude, longitude) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var addressResult = result.Value?.GetType().GetProperty("address")?.GetValue(result.Value, null);
            Console.WriteLine($"Result: {addressResult}");
            Console.WriteLine($"expectedAddress: {expectedAddress}");
            Assert.That(addressResult, Is.EqualTo(expectedAddress));
        }
    }
}