using Moq;
using Moq.Protected;
using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Service_Tests;

[TestFixture]
public class AiService_Tests
{
    private AiService _aiService;
    private Mock<ILogger<AiService>> _loggerMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;

    const string API_URL = "https://openrouter.ai/api/v1/chat/completions";

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<AiService>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri(API_URL)
        };
        _aiService = new AiService(_httpClient, _loggerMock.Object);
    }

    [Test]
    //Test that GetResponse returns a string when working
    public async Task Test_GetResponse()
    {
        // Arrange
        var jsonResponse = "{\"id\":\"1\",\"provider\":\"openai\",\"model\":\"gpt-3.5-turbo\",\"object\":\"chat.completion\",\"created\":1234567890,\"choices\":[{\"logprobs\":null,\"finish_reason\":\"stop\",\"native_finish_reason\":\"stop\",\"index\":0,\"message\":{\"role\":\"assistant\",\"content\":\"apple, banana, orange, grapes, watermelon\"}}],\"usage\":{\"prompt_tokens\":10,\"completion_tokens\":20,\"total_tokens\":30}}";
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
        var result = await _aiService.GetResponse("apple, banana", IAiService.AiServiceType.Suggestion);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<string>(result);
    }

    [Test]
    //Test that GetResponse returns broken message when not working
    public async Task Test_GetResponse_Broken()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        string expectedErrorMessage = "No response from AI";

        // Act
        var result = await _aiService.GetResponse("apple, banana", IAiService.AiServiceType.Suggestion);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Does.Contain(expectedErrorMessage));
    }


    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}

