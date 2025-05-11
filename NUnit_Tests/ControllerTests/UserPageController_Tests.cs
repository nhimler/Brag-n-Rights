using Moq;
using GymBro_App.DAL.Abstract;
using GymBro_App.Controllers;
using GymBro_App.Models;
using GymBro_App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GymBro_App.Services;

namespace Controller_Tests;

[TestFixture]
public class UserPageController_Test
{
    private Mock<ILogger<UserPageController>> _mockLogger;
    private Mock<IUserRepository> _mockUserRepo;
    private Mock<IGymUserRepository> _mockGymUserRepo;
    private Mock<IGoogleMapsService> _mockGoogleMapsService;
    private Mock<UserManager<IdentityUser>> _mockUserManager;
    private Mock<UserInfoModel> _mockUserInfoModel;
    private UserPageController _userController;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<UserPageController>>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockGymUserRepo = new Mock<IGymUserRepository>();
        _mockGoogleMapsService = new Mock<IGoogleMapsService>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);
        _mockUserInfoModel = new Mock<UserInfoModel>();

        _userController = new UserPageController(_mockLogger.Object, _mockUserRepo.Object, _mockGymUserRepo.Object, _mockGoogleMapsService.Object,_mockUserManager.Object);

        // Mock the HttpContext with a user principal
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "User")
        }, "mock"));

        var httpContext = new DefaultHttpContext { User = user };
        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Test]
    public void Index_ShouldReturnViewResultWithUserInfoModel()
    {
        // Arrange
        var user = new User();
        _mockUserRepo.Setup(repo => repo.GetUserByIdentityUserId(It.IsAny<string>())).Returns(user);

        // Act
        var result = _userController.Index(_mockUserInfoModel.Object);

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsInstanceOf<UserInfoModel>(viewResult.Model);
    }

    [Test]
    public void ChangeInfo_ShouldReturnChangeInfoResult()
    {
        // Arrange
        string expectedViewName = "ChangeInfo";
        var userInfo = new UserInfoModel();

        var user = new User
        {
            UserId = 1,
            IdentityUserId = "12",
            Age = 25,
            Gender = "Male",
            Weight = 70.5m,
            Height = 175,
            FitnessLevel = "Beginner",
            Fitnessgoals = "Weight Loss",
            PreferredWorkoutTime = "Morning",
            Username = "testuser",
            Email = "test@user.com",
            FirstName = "Test",
            LastName = "User"
        };

        _mockUserRepo.Setup(repo => repo.GetUserByIdentityUserId(It.IsAny<string>())).Returns(user);

        // Act
        var result = _userController.ChangeInfo(userInfo);

        // Assert
        var viewResult = result as ViewResult;
        Assert.Multiple(() =>
        {
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult?.ViewName, Is.EqualTo(expectedViewName));
        });

        Assert.That(viewResult.Model, Is.InstanceOf<UserInfoModel>());

        var model = viewResult.Model as UserInfoModel;
        Assert.Multiple(() =>
        {
            Assert.That(model?.Username, Is.EqualTo(user.Username));
            Assert.That(model?.Email, Is.EqualTo(user.Email));
            Assert.That(model?.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(model?.LastName, Is.EqualTo(user.LastName));
            Assert.That(model?.Age, Is.EqualTo(user.Age));
            Assert.That(model?.Gender, Is.EqualTo(user.Gender));
            Assert.That(model?.Weight, Is.EqualTo(user.Weight));
            Assert.That(model?.Height, Is.EqualTo(user.Height));
            Assert.That(model?.FitnessLevel, Is.EqualTo(user.FitnessLevel));
            Assert.That(model?.Fitnessgoals, Is.EqualTo(user.Fitnessgoals));
            Assert.That(model?.PreferredWorkoutTime, Is.EqualTo(user.PreferredWorkoutTime));
        });
    }

    [Test]
    public void UpdateSettings_ShouldReturnRedirectToActionResult()
    {
        // Arrange
        var userInfoModel = new UserInfoModel
        {
            Age = 25,
            Gender = "Male",
            Weight = 70.5m,
            Height = 175,
            FitnessLevel = "Beginner",
            Fitnessgoals = "Weight Loss",
            PreferredWorkoutTime = "Morning"
        };

        var user = new User
        {
            UserId = 1,
            IdentityUserId = "12",
            Age = 25,
            Gender = "Male",
            Weight = 70.5m,
            Height = 175,
            FitnessLevel = "Beginner",
            Fitnessgoals = "Weight Loss",
            PreferredWorkoutTime = "Morning"
        };

        _mockUserRepo.Setup(repo => repo.GetUserByIdentityUserId(It.IsAny<string>())).Returns(user);

        // Act
        var result = _userController.UpdateUserInfo(userInfoModel);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [TearDown]
    public void TearDown()
    {
        _mockUserRepo = null;
        _userController.Dispose();
        _userController = null;
    }
}