using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymBro_App.Controllers;
using GymBro_App.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
                null, null, null, null, null, null, null, null);

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
        public void UserLocationPut_ShouldUpdateUsersLatitudeAndLongitude()
        {
            // Arrange
            var userDTO = new UserDTO { Latitude = 45.0m, Longitude = -122.0m };
            var user = new GymBro_App.Models.User { IdentityUserId = "1", Latitude = 0.0m, Longitude = 0.0m };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");
            _mockUserRepository.Setup(repo => repo.GetUserByIdentityUserId("1")).Returns(user);

            // Act
            var result = _userAPIController.UserLocation(userDTO);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.That(45.0m, Is.EqualTo(user.Latitude));
            Assert.That(-122.0m, Is.EqualTo(user.Longitude));
            _mockUserRepository.Verify(repo => repo.AddOrUpdate(It.Is<GymBro_App.Models.User>(u => u.Latitude == 45.0m && u.Longitude == -122.0m)), Times.Once);
        }

        [TearDown]
        public void TearDown()
        {
            _userAPIController.Dispose();
            _userAPIController = null;
        }
    }
}