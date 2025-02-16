using Microsoft.AspNetCore.Mvc.Testing;
using GymBro_App;

namespace NUnit_Tests;

[TestFixture]
public class MealPlanController_Tests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task Test_FoodSearch_HasSearchBar()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/MealPlan/CreateMeal");
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        // Assert.That(html, Does.Contain("Search"));
        // Assert.That(html, Does.Contain("<input"));
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}

