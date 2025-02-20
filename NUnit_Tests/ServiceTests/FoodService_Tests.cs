using Moq;
using GymBro_App.DTO;
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

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<FoodService>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://platform.fatsecret.com/rest/server.api")
        };
        _foodService = new FoodService(_httpClient, _loggerMock.Object);
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
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
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
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
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

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}

