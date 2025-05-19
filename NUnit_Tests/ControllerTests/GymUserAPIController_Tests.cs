using GymBro_App.Controllers;
using GymBro_App.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using GymBro_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Controller_Tests
{
    public class GymUserAPIController_Tests
    {
        private Mock<ILogger<GymUserAPIController>> _mockLogger;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<IGymUserRepository> _mockGymUserRepository;
        private GymUserAPIController _gymUserAPIController;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<GymUserAPIController>>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockGymUserRepository = new Mock<IGymUserRepository>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object,
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object
                );

            _gymUserAPIController = new GymUserAPIController(_mockLogger.Object, _mockUserRepository.Object, _mockGymUserRepository.Object, _mockUserManager.Object);

            // Mock the HttpContext with a user principal
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "User")
            }, "mock"));

            var httpContext = new DefaultHttpContext { User = user };
            _gymUserAPIController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        // TODO: Add these tests back when the methods are implemented in the GymUserAPIController
        // [Test]
        // public async Task BookmarkGym_ShouldReturnOkWhenGymIsSuccessfullyBookmarked()
        // {
        //     // Arrange
        //     string gymPlaceId = "gym123";
        //     var user = new User
        //     {
        //         UserId = 1,
        //         IdentityUserId = "1"
        //     };

        //     _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
        //     _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns(user);
        //     _mockGymUserRepository.Setup(repo => repo.GetAllGymUsersByUserId(1)).Returns(new List<GymUser>());

        //     // Act
        //     var result = await _gymUserAPIController.BookmarkGym(gymPlaceId);

        //     // Assert
        //     Assert.IsInstanceOf<OkObjectResult>(result);
        //     var okResult = result as OkObjectResult;
        //     Assert.That(okResult.Value, Is.EqualTo("Gym bookmarked successfully."));
        //     _mockGymUserRepository.Verify(repo => repo.AddOrUpdate(It.Is<GymUser>(g => g.ApiGymId == gymPlaceId && g.UserId == 1)), Times.Once);
        // }

        // [Test]
        // public async Task BookmarkGym_ShouldReturnBadRequestWhenGymIsAlreadyBookmarked()
        // {
        //     // Arrange
        //     string gymPlaceId = "gym123";
        //     var user = new User
        //     {
        //         UserId = 1,
        //         IdentityUserId = "1"
        //     };

        //     var existingGymUsers = new List<GymUser>
        //     {
        //         new GymUser { GymUserId = 1, ApiGymId = "gym123", UserId = 1 }
        //     };

        //     _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
        //     _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns(user);
        //     _mockGymUserRepository.Setup(repo => repo.GetAllGymUsersByUserId(1)).Returns(existingGymUsers);
        //     _mockGymUserRepository.Setup(repo => repo.IsGymBookmarked(gymPlaceId, user.UserId)).Returns(true);

        //     // Act
        //     var result = await _gymUserAPIController.BookmarkGym(gymPlaceId);

        //     // Assert
        //     Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //     var badRequestResult = result as BadRequestObjectResult;
        //     Assert.That(badRequestResult.Value, Is.EqualTo("Gym already bookmarked."));
        //     _mockGymUserRepository.Verify(repo => repo.AddOrUpdate(It.IsAny<GymUser>()), Times.Never);
        // }

        // [Test]
        // public async Task BookmarkGym_ShouldReturnNotFoundWhenUserIsNotFound()
        // {
        //     // Arrange
        //     string gymPlaceId = "gym123";

        //     _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
        //     _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns((User)null);

        //     // Act
        //     var result = await _gymUserAPIController.BookmarkGym(gymPlaceId);

        //     // Assert
        //     Assert.IsInstanceOf<NotFoundObjectResult>(result);
        //     var notFoundResult = result as NotFoundObjectResult;
        //     Assert.That(notFoundResult.Value, Is.EqualTo("User not found."));
        //     _mockGymUserRepository.Verify(repo => repo.AddOrUpdate(It.IsAny<GymUser>()), Times.Never);
        // }

        [Test]
        public async Task DeleteBookmark_ShouldDeleteBookmarkSuccessfully()
        {
            // Arrange
            string gymPlaceId = "gym123";
            var user = new User
            {
                UserId = 1,
                IdentityUserId = "1"
            };

            var existingGymUsers = new List<GymUser>
            {
                new GymUser { GymUserId = 1, ApiGymId = "gym123", UserId = 1 }
            };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
            _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns(user);
            _mockGymUserRepository.Setup(repo => repo.GetAllGymUsersByUserId(1)).Returns(existingGymUsers);
            _mockGymUserRepository.Setup(repo => repo.IsGymBookmarked(gymPlaceId, user.UserId)).Returns(true);

            // Act
            var result = await _gymUserAPIController.DeleteGymBookmark(gymPlaceId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Gym bookmark deleted successfully."));
        }

        [TearDown]
        public void TearDown()
        {
            _gymUserAPIController.Dispose();
            _gymUserAPIController = null!;
        }
    }
}