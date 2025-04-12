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

    /*[Test]
     public void Index_ReturnsViewResult_WithTableOfWorkoutPlans()
    {
        // Arrange
        var workoutPlans = new List<WorkoutPlan> { new WorkoutPlan(), new WorkoutPlan() };
        _mockRepo.Setup(repo => repo.GetAll()).Returns(workoutPlans.AsQueryable());

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsInstanceOf<List<WorkoutPlan>>(viewResult.Model);
        Assert.That((List<WorkoutPlan>)viewResult.Model, Has.Count.EqualTo(2));
    } */

    [Test]
    public async Task Create_ValidModel_RedirectsToIndex()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan();
        _mockRepo.Setup(repo => repo.Add(It.IsAny<WorkoutPlan>()));

        // Act
        var result = await _controller.Create(workoutPlan);

        // Assert
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectToActionResult);
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public async Task Create_InvalidModel_ReturnsViewResult()
    {
        // Arrange
        var workoutPlan = new WorkoutPlan();
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.Create(workoutPlan);

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.That(viewResult.Model, Is.EqualTo(workoutPlan));
    }

    [Test]
    public void WorkoutCreationPage_UserNotAuthenticated_ReturnsView_RedirectsToIndex()
    {
        //Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal()}
        };

        //Act
        var result = _controller.WorkoutCreationPage();

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectToActionResult);
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void WorkoutCreationPage_UserAUthenticated_ReturnsView()
    {
        //Arrange
        var mockUser = new IdentityUser {Id = "1"};
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        _mockUserRepository.Setup(repo => repo.GetIdFromIdentityId("1")).Returns(1);

        //Act
        var result = _controller.WorkoutCreationPage();

        //Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }

    [Test]
    public void WorkoutCreationActions_UserAuthenticated_ReturnsPartialView()
    {
        //Arrange
        var mockUser = new IdentityUser {Id = "1"};
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        _mockUserRepository.Setup(repo => repo.GetIdFromIdentityId("1")).Returns(1);
        
        //Act
        var result = _controller.WorkoutCreationActionsPartial();

        //Assert
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

    [Test]
    public void ExerciseSearch_ReturnsView()
    {
        // Act
        var result = _controller.ExerciseSearch();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }

    [TearDown]
    public void TearDown()
    {
        _mockRepo = null;
        _controller.Dispose();
        _controller = null;
    }
}