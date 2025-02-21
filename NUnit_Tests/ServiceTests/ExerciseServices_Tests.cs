using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Service_Tests
{
    [TestFixture]
    public class ExerciseServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private Mock<ILogger<ExerciseService>> _loggerMock;
        private ExerciseService _exerciseService;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://exercisedb.p.rapidapi.com")
            };
            _loggerMock = new Mock<ILogger<ExerciseService>>();
            _exerciseService = new ExerciseService(_httpClient, _loggerMock.Object);
        }

        [Test]
        public async Task GetExercisesAsync_ShouldReturnCorrectIds_WhenResponseIsSuccessful()
        {
            // Arrange
            var jsonResponse = "[ { \"id\": \"0001\", \"name\": \"sit-up\" }, { \"id\": \"0002\", \"name\": \"sit-up\" } ]";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
            
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _exerciseService.GetExercisesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo("0001"));
            Assert.That(result[1].Id, Is.EqualTo("0002"));
        }

        [Test]
        public async Task GetExercisesAsync_ShouldReturnEmptyList_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _exerciseService.GetExercisesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetExerciseAsync_ShouldReturnExerciseList_WhenResponseIsSuccessful()
        {
            // Arrange
            var jsonResponse = "[ { \"id\": \"0001\", \"name\": \"sit-up\" } ]";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
            
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _exerciseService.GetExerciseAsync("sit-up");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo("0001"));
            Assert.That(result[0].Name, Is.EqualTo("sit-up"));
        }

        [Test]
        public async Task GetExerciseAsync_ShouldReturnEmptyList_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };
            
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _exerciseService.GetExerciseAsync("sit-up");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
