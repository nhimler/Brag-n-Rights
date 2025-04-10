using GymBro_App.Controllers;
using GymBro_App.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using GymBro_App.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Controller_Tests
{
    public class UserApiController_Tests
    {
        private Mock<ILogger<UserAPIController>> _mockLogger;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private UserAPIController _userAPIController;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<UserAPIController>>();
            _mockUserRepository = new Mock<IUserRepository>();
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

            _userAPIController = new UserAPIController(_mockLogger.Object, _mockUserRepository.Object, _mockUserManager.Object);

            // Mock the HttpContext with a user principal
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "User")
            }, "mock"));

            var httpContext = new DefaultHttpContext { User = user };
            _userAPIController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Test]
        public async Task UpdateProfilePicture_ShouldReturnNotFoundWhenUserNotFound()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "fake image content";
            var fileName = "profile.jpg";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);

            // Mocking a user that does not exist
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("123");
            _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("123")).Returns((GymBro_App.Models.User)null);

            // Act
            var result = await _userAPIController.UpdateProfilePicture(mockFile.Object);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task UpdateProfilePicture_ShouldReturnBadRequestWhenProfilePictureNullOrEmpty()
        {
            // Arrange
            IFormFile? mockFile = null; // Simulating a null file

            // Act
            var result = await _userAPIController.UpdateProfilePicture(mockFile);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateProfilePicture_ShouldUpdateUserProfilePictureWhenPictureIsValid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "fake image content";
            var fileName = "profile.jpg";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);

            // Needed to mock CopyToAsync used in the controller
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((targetStream, cancellationToken) =>
                {
                    return stream.CopyToAsync(targetStream, cancellationToken);
                });

            // Mocking a user with a null profile picture
            var user = new GymBro_App.Models.User
            {
                IdentityUserId = "123",
                ProfilePicture = null
            };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("123");
            _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("123")).Returns(user);

            // Act
            var result = await _userAPIController.UpdateProfilePicture(mockFile.Object);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(user.ProfilePicture, Is.Not.Null);
            Assert.That(user.ProfilePicture.Length, Is.EqualTo(content.Length));
        }


        // Removed test because method has been removed from UserAPIController
        // [Test]
        // public void UserLocationPut_ShouldUpdateUsersLatitudeAndLongitude()
        // {
        //     // Arrange
        //     var userDTO = new UserDTO { Latitude = 45.0m, Longitude = -122.0m };
        //     var user = new GymBro_App.Models.User { IdentityUserId = "1", Latitude = 0.0m, Longitude = 0.0m };
        //     var expectedLatitude = 45.0m;
        //     var expectedLongitude = -122.0m;

        //     _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
        //     _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns(user);

        //     // Act
        //     var result = _userAPIController.UserLocation(userDTO);

        //     // Assert
        //     Assert.IsInstanceOf<OkResult>(result);
        //     Assert.That(expectedLatitude, Is.EqualTo(user.Latitude));
        //     Assert.That(expectedLongitude, Is.EqualTo(user.Longitude));
        //     _mockUserRepository.Verify(repo => repo.AddOrUpdate(It.Is<GymBro_App.Models.User>(u => u.Latitude == 45.0m && u.Longitude == -122.0m)), Times.Once);
        // }

        [TearDown]
        public void TearDown()
        {
            _userAPIController.Dispose();
            _userAPIController = null!;
        }
    }
}