using GymBro_App.Controllers;
using GymBro_App.DTO;
using GymBro_App.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Controller_Tests;

[TestFixture]
public class AiAPIController_Tests
{

    public Mock<IAiService> _mockService;
    public Mock<ILogger<AiAPIController>> _mockLogger;
    public AiAPIController _controller;

    [SetUp] 
    public void SetUp()
    {
        _mockService = new Mock<IAiService>();
        _mockLogger = new Mock<ILogger<AiAPIController>>();
        _controller = new AiAPIController(_mockService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task Suggest_ReturnsString()
    {
        // Arrange
        var query = "chicken, rice, broccoli";
        var response = "Chicken and Rice Bowl\nBroccoli Stir Fry\nChicken Salad\nRice and Broccoli Casserole\nChicken Fried Rice";
        _mockService.Setup(service => service.GetResponse(query)).ReturnsAsync(response);

        // Act
        var result = await _controller.Suggest(query);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.EqualTo(response));
    }

    [Test]
    public async Task Suggest_ReturnsBadRequest_WhenQueryIsNullOrEmpty()
    {
        // Arrange
        string query = null;
        string expectedErrorMessage = "Query cannot be null or empty.";

        // Act
        var result = await _controller.Suggest(query);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        Assert.That(badRequestResult.Value, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task Suggest_BrokenService_ReturnsInternalServerError()
    {
        // Arrange
        var query = "chicken, rice, broccoli";
        _mockService.Setup(service => service.GetResponse(query)).ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.Suggest(query);

        // Assert
        Assert.IsInstanceOf<ObjectResult>(result);
        var statusCodeResult = result as ObjectResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
    }
}