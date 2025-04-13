using Moq;
using GymBro_App.DAL.Abstract;
using GymBro_App.Controllers;
using GymBro_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Controller_Tests;

[TestFixture]
public class WorkoutsController_Tests
{
    private Mock<IWorkoutPlanRepository> _mockRepo;
    private WorkoutsController _controller;
    private Mock<UserManager<IdentityUser>> _mockUserManager;
    private Mock<ILogger<WorkoutsController>> _mockLogger;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IWorkoutPlanRepository>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
        _mockLogger = new Mock<ILogger<WorkoutsController>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _controller = new WorkoutsController(
            _mockRepo.Object,
            _mockLogger.Object,
            _mockUserManager.Object,
            _mockUserRepository.Object);

        // Default authenticated user in context
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "User")
        }, "mock"));
        var httpContext = new DefaultHttpContext { User = user };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Test]
    public async Task Create_ValidModel_RedirectsToIndex()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan();
        var identityUser = new IdentityUser { Id = "1" };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(identityUser);
        _mockUserRepository.Setup(repo => repo.GetIdFromIdentityId("1"))
                           .Returns(1);

        // Act
        var result = await _controller.Create(workoutPlan);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

        _mockRepo.Verify(repo => repo.Add(It.Is<WorkoutPlan>(wp =>
            wp.UserId == 1 &&
            wp.IsCompleted == 0 &&                                  
            !string.IsNullOrEmpty(wp.ApiId) && wp.ApiId.StartsWith("local-")
        )), Times.Once);
    }

    [Test]
    public async Task Create_WhenUserNotAuthenticated_RedirectsToIndex()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };
        var workoutPlan = new WorkoutPlan();

        // Act
        var result = await _controller.Create(workoutPlan);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public async Task Create_WhenUserNotFound_RedirectsToIndex()
    {
        // Arrange
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync((IdentityUser)null);
        var workoutPlan = new WorkoutPlan();

        // Act
        var result = await _controller.Create(workoutPlan);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void WorkoutCreationPage_UserNotAuthenticated_ReturnsRedirectToIndex()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = _controller.WorkoutCreationPage();

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void WorkoutCreationPage_UserAuthenticated_ReturnsViewWithWorkoutPlan()
    {
        // Arrange
        var identityUser = new IdentityUser { Id = "1" };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(identityUser);
        _mockUserRepository.Setup(repo => repo.GetIdFromIdentityId("1"))
                           .Returns(1);

        // Act
        var result = _controller.WorkoutCreationPage();

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        var model = viewResult.Model as WorkoutPlan;
        Assert.IsNotNull(model);
        Assert.That(model.UserId, Is.EqualTo(1));
    }

    [Test]
    public void WorkoutCreationActions_UserAuthenticated_ReturnsPartialView()
    {
        // Arrange
        var identityUser = new IdentityUser { Id = "1" };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(identityUser);
        _mockUserRepository.Setup(repo => repo.GetIdFromIdentityId("1"))
                           .Returns(1);

        // Act
        var result = _controller.WorkoutCreationActionsPartial();

        // Assert
        var partialViewResult = result as PartialViewResult;
        Assert.IsNotNull(partialViewResult);
        Assert.That(partialViewResult.ViewName, Is.EqualTo("_WorkoutCreationActions"));
    }

    [Test]
    public void WorkoutCreationActionsPartial_UserNotAuthenticated_ReturnsContent()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
        };

        // Act
        var result = _controller.WorkoutCreationActionsPartial();

        // Assert
        var contentResult = result as ContentResult;
        Assert.IsNotNull(contentResult);
        Assert.That(contentResult.Content, Is.EqualTo(""));
    }
    
    [TearDown]
    public void TearDown()
    {
        _mockRepo = null;
        _controller.Dispose();
        _controller = null;
    }
}