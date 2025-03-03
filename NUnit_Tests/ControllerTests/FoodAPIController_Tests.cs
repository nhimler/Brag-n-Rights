using GymBro_App.Controllers;
using GymBro_App.DTO;
using GymBro_App.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Controller_Tests;

[TestFixture]
public class FoodAPIController_Tests
{

    public Mock<IFoodService> _mockService;
    public Mock<ILogger<FoodAPIController>> _mockLogger;
    public FoodAPIController _controller;

    [SetUp] 
    public void SetUp()
    {
        _mockService = new Mock<IFoodService>();
        _mockLogger = new Mock<ILogger<FoodAPIController>>();
        _controller = new FoodAPIController(_mockService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task Search_ReturnsListOfFoodDTOs()
    {
        // Arrange
        var foods = new List<FoodDTO>
        {
            new FoodDTO { FoodId = "0001", FoodName = "apple", FoodDescription = "fruit" },
            new FoodDTO { FoodId = "0002", FoodName = "banana", FoodDescription = "fruit" }
        };
        _mockService.Setup(service => service.GetFoodsAsync("apple")).ReturnsAsync(foods);

        // Act
        var result = await _controller.Search("apple");

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.EqualTo(foods));
    }

    [Test]
    public async Task GetById_ReturnsFoodDTO()
    {
        // Arrange
        var food = new FoodDTO { FoodId = "0001", FoodName = "apple", FoodDescription = "fruit" };
        _mockService.Setup(service => service.GetFoodAsync("0001")).ReturnsAsync(food);

        // Act
        var result = await _controller.GetById("0001");

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.EqualTo(food));
    }
}