using Xunit;
using Moq;
using GymBro_App.Controllers;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GymBro_App.Models.DTOs;

public class AwardMedalControllerTests
{
    private readonly Mock<IAwardMedalService> _mockAwardMedalService;
    private readonly AwardMedalController _controller;
    private readonly ClaimsPrincipal _user;

    public AwardMedalControllerTests()
    {
        _mockAwardMedalService = new Mock<IAwardMedalService>();
        _controller = new AwardMedalController(_mockAwardMedalService.Object);

        // Simulate a user with a claim for 'NameIdentifier' (IdentityId)
        _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user123") // simulate authenticated user with 'user123' as identityId
        }, "mock"));
    }

    // Helper method to simulate the controller being executed with an authenticated user
    private void SetUser(ClaimsPrincipal user)
    {
        _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task AwardMedals_ShouldReturnView_WhenUserIsAuthenticatedAndMedalsAreAwarded()
    {
        // Arrange
        SetUser(_user);
        var awardMedalsResult = new AwardMedal
        {
            UserId = 123,
            AwardedMedals = new List<AwardMedalDetails>
            {
                new AwardMedalDetails { MedalId = 1, MedalName = "Gold Medal" }
            }
        };

        _mockAwardMedalService.Setup(s => s.AwardUserdMedalsAsync("user123")).ReturnsAsync(awardMedalsResult);

        // Act
        var result = await _controller.AwardMedals();

        // Assert
        var viewResult = Xunit.Assert.IsType<ViewResult>(result);
        Xunit.Assert.Equal("AwardedMedals", viewResult.ViewName);
        var model = Xunit.Assert.IsType<AwardMedal>(viewResult.Model);
        Xunit.Assert.Equal(1, model.AwardedMedals.Count); // Should return one awarded medal
    }

    [Fact]
    public async Task AwardMedals_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        SetUser(null); // Simulating an unauthenticated user

        // Act
        var result = await _controller.AwardMedals();

        // Assert
        var unauthorizedResult = Xunit.Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task AwardMedals_ShouldReturnNoMedalsView_WhenNoMedalsAreAwarded()
    {
        // Arrange
        SetUser(_user);
        var awardMedalsResult = new AwardMedal
        {
            UserId = 123,
            AwardedMedals = new List<AwardMedalDetails>() // No medals awarded
        };

        _mockAwardMedalService.Setup(s => s.AwardUserdMedalsAsync("user123")).ReturnsAsync(awardMedalsResult);

        // Act
        var result = await _controller.AwardMedals();

        // Assert
        var viewResult = Xunit.Assert.IsType<ViewResult>(result);
        Xunit.Assert.Equal("NoMedals", viewResult.ViewName);
    }
}
