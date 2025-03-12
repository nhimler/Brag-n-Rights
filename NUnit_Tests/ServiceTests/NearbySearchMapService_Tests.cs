using GymBro_App.Services;
using GymBro_App.Models.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Moq.Protected;

namespace Service_Tests
{
    public class NearbySearchMapService_Tests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Mock<ILogger<NearbySearchMapService>> _mockILogger;
        private NearbySearchMapService _nearbySearchMapService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockILogger = new Mock<ILogger<NearbySearchMapService>>();
            _nearbySearchMapService = new NearbySearchMapService(_httpClient, _mockILogger.Object);
        }

        [Test]
        public async Task FindNearbyGyms_ShouldReturnAListOfNearbyGyms()
        {
            // Arrange
            var latitude = 45.0;
            var longitude = -122.0;
            var expectedPlaces = new List<PlaceDTO>
            {
                new PlaceDTO { DisplayName = new DisplayName { Text = "Gym 1" }, FormattedAddress = "Address 1" },
                new PlaceDTO { DisplayName = new DisplayName { Text = "Gym 2" }, FormattedAddress = "Address 2" }
            };

            var root = new NearbySearchMapService.Root
            {
                Places = expectedPlaces
            };

            var jsonResponse = JsonSerializer.Serialize(root);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _nearbySearchMapService.FindNearbyGyms(latitude, longitude);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedPlaces.Count));
            Assert.That(result[0].DisplayName.Text, Is.EqualTo(expectedPlaces[0].DisplayName.Text));
            Assert.That(result[0].FormattedAddress, Is.EqualTo(expectedPlaces[0].FormattedAddress));
            Assert.That(result[1].DisplayName.Text, Is.EqualTo(expectedPlaces[1].DisplayName.Text));
            Assert.That(result[1].FormattedAddress, Is.EqualTo(expectedPlaces[1].FormattedAddress));
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}