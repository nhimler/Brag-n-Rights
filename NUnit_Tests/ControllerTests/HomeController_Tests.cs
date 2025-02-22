using Microsoft.AspNetCore.Mvc.Testing;
using GymBro_App;

namespace Controller_Tests;

[TestFixture]
public class HomeController_Tests
{
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Test]
    public async Task Test_IndexPage_ContainsWelcomeText()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(html, Does.Contain("Welcome"));
        Assert.That(html, Does.Contain("src=\"GymBro.png\""));
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
}

