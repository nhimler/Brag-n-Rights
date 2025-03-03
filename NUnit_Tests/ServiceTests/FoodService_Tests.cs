using Moq;
using Moq.Protected;
using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Service_Tests;

[TestFixture]
public class FoodService_Tests
{
    private FoodService _foodService;
    private Mock<ILogger<FoodService>> _loggerMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;

    const string ACCESS_TOKEN_REQUEST_URL = "https://oauth.fatsecret.com/connect/token";
    const string FOOD_API_URL = "https://platform.fatsecret.com/rest/server.api";

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<FoodService>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://platform.fatsecret.com/rest/server.api")
        };
        _foodService = new FoodService(_httpClient, _loggerMock.Object, "a", "b");
    }

    [Test]
    //Test that GetFoodsAsync returns a list of FoodDTOs
    public async Task Test_GetFoodsAsync()
    {
        // Arrange
        var jsonResponse = "{\"foods\":{\"food\":[{\"food_id\":\"1\",\"food_name\":\"apple\",\"food_description\":\"fruit\"},{\"food_id\":\"2\",\"food_name\":\"banana\",\"food_description\":\"fruit\"}]}}";
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString() == ACCESS_TOKEN_REQUEST_URL), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"access_token\": \"mocked-access-token\"}")
            });

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString() == FOOD_API_URL), ItExpr.IsAny<CancellationToken>())

            .ReturnsAsync(responseMessage);

        // Act
        var result = await _foodService.GetFoodsAsync("apple");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].FoodId, Is.EqualTo("1"));
        Assert.That(result[0].FoodName, Is.EqualTo("apple"));
        Assert.That(result[0].FoodDescription, Is.EqualTo("fruit"));
        Assert.That(result[1].FoodId, Is.EqualTo("2"));
        Assert.That(result[1].FoodName, Is.EqualTo("banana"));
        Assert.That(result[1].FoodDescription, Is.EqualTo("fruit"));
    }

    [Test]
    //Test that GetFoodsAsync returns an empty list when the response is not successful
    public async Task Test_GetFoodsAsync_EmptyList()
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
        var result = await _foodService.GetFoodsAsync("apple");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    //Test that GetFoodAsync returns a FoodDTO
    public async Task Test_GetFoodAsync()
    {
        // Arrange
        var jsonResponse = "{\"food\":{\"food_id\":\"1\",\"food_name\":\"apple\",\"food_description\":\"fruit\"}}";
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString() == ACCESS_TOKEN_REQUEST_URL), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"access_token\": \"mocked-access-token\"}")
            });

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString() == FOOD_API_URL), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _foodService.GetFoodAsync("1");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.FoodId, Is.EqualTo("1"));
        Assert.That(result.FoodName, Is.EqualTo("apple"));
    }

    [Test]
    //Test that GetFoodAsync returns a null FoodDTO when the response is not successful
    public async Task Test_GetFoodAsync_Null()
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
        var result = await _foodService.GetFoodAsync("1");

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    //Test that GetNewAccessTokenAsync sets the access token and expiration
    public async Task Test_GetNewAccessTokenAsync()
    {
        // Arrange
        var jsonResponse = "{\"access_token\":\"123\",\"expires_in\":86400}";
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
        await _foodService.GetNewAccessTokenAsync();

        // Assert
        Assert.That(_foodService._accessTokenExpiration, Is.GreaterThanOrEqualTo(DateTime.Now.AddHours(22)));
    }

    [Test]
    //Test that GetNewAccessTokenAsync logs an error when the response is not successful
    public async Task Test_GetNewAccessTokenAsync_Error()
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
        await _foodService.GetNewAccessTokenAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}

