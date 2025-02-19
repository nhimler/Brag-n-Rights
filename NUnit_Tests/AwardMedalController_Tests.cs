using NUnit.Framework;
using Moq;
using GymBro_App.Controllers;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GymBro_App.Models.DTOs;

[TestFixture]
public class AwardMedalControllerTests
{
    private Mock<IAwardMedalService> _mockAwardMedalService;
    private AwardMedalController _controller;
    private ClaimsPrincipal _user;

    [SetUp]
    public void Setup()
    {
        _mockAwardMedalService = new Mock<IAwardMedalService>();
        _controller = new AwardMedalController(_mockAwardMedalService.Object);

        _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user123")
        }, "mock"));
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    private void SetUser(ClaimsPrincipal user)
    {
        _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Test]
    public async Task AwardMedals_ShouldReturnView_WhenUserIsAuthenticatedAndMedalsAreAwarded()
    {
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

        var result = await _controller.AwardMedals();

        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.AreEqual("AwardedMedals", viewResult.ViewName);
        var model = viewResult.Model as AwardMedal;
        Assert.IsInstanceOf<AwardMedal>(model);
        Assert.AreEqual(1, model.AwardedMedals.Count);
    }

    [Test]
    public async Task AwardMedals_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        SetUser(null);

        var result = await _controller.AwardMedals();

        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public async Task AwardMedals_ShouldReturnNoMedalsView_WhenNoMedalsAreAwarded()
    {
        SetUser(_user);
        var awardMedalsResult = new AwardMedal
        {
            UserId = 123,
            AwardedMedals = new List<AwardMedalDetails>()
        };

        _mockAwardMedalService.Setup(s => s.AwardUserdMedalsAsync("user123")).ReturnsAsync(awardMedalsResult);

        var result = await _controller.AwardMedals();

        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.AreEqual("NoMedals", viewResult.ViewName);
    }
}
