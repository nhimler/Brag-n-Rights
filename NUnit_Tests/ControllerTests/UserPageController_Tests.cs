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

[TestFixture]
public class UserPageController_Test
{
    private Mock<ILogger<UserPageController>> _mockLogger;
    private Mock<IUserRepository> _mockUserRepo;
    private Mock<UserManager<IdentityUser>> _mockUserManager;
    private Mock<UserInfoModel> _mockUserInfoModel;
    private UserPageController _userController;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<UserPageController>>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);
        _mockUserInfoModel = new Mock<UserInfoModel>();

        _userController = new UserPageController(_mockLogger.Object, _mockUserRepo.Object, _mockUserManager.Object);

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

    [TearDown]
    public void TearDown()
    {
        _mockUserRepo = null;
        _userController.Dispose();
        _userController = null;
    }
}