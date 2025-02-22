// using Microsoft.AspNetCore.Mvc.Testing;
// using GymBro_App;
using Moq;
using Microsoft.AspNetCore.Identity;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Controller_Tests;

[TestFixture]
public class MealPlanController_Tests
{
    // private WebApplicationFactory<Program> _factory;
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IMealPlanRepository> _mealPlanRepositoryMock;
    private Mock<ILogger<MealPlanController>> _loggerMock;
    private Mock<IFoodRepository> _foodRepositoryMock;
    private Mock<IMealRepository> _mealRepositoryMock;
    private MealPlanController _controller;
    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null
        );

        _userRepositoryMock = new Mock<IUserRepository>();
        _mealPlanRepositoryMock = new Mock<IMealPlanRepository>();
        _loggerMock = new Mock<ILogger<MealPlanController>>();
        _foodRepositoryMock = new Mock<IFoodRepository>();
        _mealRepositoryMock = new Mock<IMealRepository>();

        // _factory = new WebApplicationFactory<Program>();
        _controller = new MealPlanController(_loggerMock.Object, _foodRepositoryMock.Object, _mealRepositoryMock.Object, _mealPlanRepositoryMock.Object, _userManagerMock.Object, _userRepositoryMock.Object);
    }

    [Test]
    public void Test_FoodSearchRedirectsWhenNotAuthenticated()
    {
        var result = _controller.CreateMeal();

        Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        var redirectResult = (RedirectToActionResult)result;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void Test_FoodSearchRedirectsWhenMealNotFound()
    {
        var user = new IdentityUser { UserName = "testuser", Email = "testuser@example.com" };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(ur => ur.GetIdFromIdentityId(It.IsAny<string>())).Returns(0);
        _mealPlanRepositoryMock.Setup(mp => mp.GetFirstMealPlanForUser(It.IsAny<int>())).Returns((MealPlan?)null);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act: Call the CreateMeal action
        var result = _controller.CreateMeal();

        // Assert: Verify that the result is a redirect to "Index"
        Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        var redirectResult = (RedirectToActionResult)result;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void Test_FoodSearchReturnsWhenMealExists()
    {
        var user = new IdentityUser { UserName = "testuser", Email = "testuser@example.com" };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        _userRepositoryMock.Setup(ur => ur.GetIdFromIdentityId(It.IsAny<string>())).Returns(0);

        var mealPlan = new MealPlan { MealPlanId = 123 };
        _mealPlanRepositoryMock.Setup(mp => mp.GetFirstMealPlanForUser(0)).Returns(mealPlan);

        var meal = new Meal { MealId = 123 };
        _mealPlanRepositoryMock.Setup(mp => mp.FirstMeal(mealPlan.MealPlanId)).Returns(meal);

        // Set up the HttpContext.User to simulate the authenticated user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        var result = _controller.CreateMeal();

        Assert.That(result, Is.TypeOf<ViewResult>());

        var viewResult = (ViewResult)result;

        Assert.That(viewResult.ViewName, Is.EqualTo("CreateMeal"));
    }


    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
        // _factory.Dispose();
    }
}

