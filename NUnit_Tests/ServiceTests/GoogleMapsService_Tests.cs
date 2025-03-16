using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using GymBro_App.Models.DTOs;

namespace Service_Tests
{
    public class GoogleMapsService_Tests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Mock<ILogger<GoogleMapsService>> _mockILogger;
        private GoogleMapsService _googleMapsService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/api")
            };
            
            _mockILogger = new Mock<ILogger<GoogleMapsService>>();
            _googleMapsService = new GoogleMapsService(_httpClient, _mockILogger.Object);
        }

        [Test]
        public async Task GetGoogleMapsApiKey_ShouldReturnTheGoogleMapsApiKey()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", "test-api-key");
            var expectedApiKey = "test-api-key";
            
            // Act
            var apiKey = await _googleMapsService.GetGoogleMapsApiKey();

            // Assert
            Assert.That(expectedApiKey, Is.EqualTo(apiKey));
        }

        [Test]
        public async Task ReverseGeocode_ShouldReturnTheFormattedAddressOfASetOfLatitudeAndLongitudeCoordinates()
        {
            // Arrange
            string apiKey = Environment.GetEnvironmentVariable("GoogleMapsApiKey") ?? "";
            _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);


            var expectedAddress = "123 Main St, Test City, TS 12345";
            var latitude = 47.7510741;
            var longitude = -120.7401385;

            var root = new ReverseGeocodeDTO
            {
                Results = new List<Result>
                {
                    new Result
                    {
                        FormattedAddress = expectedAddress
                    }
                }
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
            var result = await _googleMapsService.ReverseGeocode(latitude, longitude);

            // Assert
            Assert.That(result, Is.EqualTo(expectedAddress));
        }

        [Test]
        public async Task ReverseGeocode_ShouldReturnAnEmptyString_WhenTheResponseIsNull()
        {
            // Arrange
            string apiKey = Environment.GetEnvironmentVariable("GoogleMapsApiKey") ?? "";
            _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

            var expectedAddress = "";
            var latitude = 0.0;
            var longitude = 0.0;

            var root = new ReverseGeocodeDTO
            {
                Results = new List<Result>()
            };

            var jsonResponse = JsonSerializer.Serialize(root);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(jsonResponse)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);
            
            // Act
            var result = await _googleMapsService.ReverseGeocode(latitude, longitude);

            // Assert
            Assert.That(result, Is.EqualTo(expectedAddress));
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}