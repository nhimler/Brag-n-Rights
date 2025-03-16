using GymBro_App.Controllers;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using GymBro_App.Models;
using System.Security.Claims;
using GymBro_App.Models.DTOs;

namespace ControllerTests
{
    [TestFixture]
    public class AwardMedalController_Tests
    {
        private Mock<IAwardMedalService> _mockAwardMedalService;
        private Mock<IOAuthService> _mockOAuthService;
        private AwardMedalController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAwardMedalService = new Mock<IAwardMedalService>();
            _mockOAuthService = new Mock<IOAuthService>();
            _controller = new AwardMedalController(_mockAwardMedalService.Object, _mockOAuthService.Object);
        }


        [Test]
        public async Task AwardMedals_UserIsNotAuthenticated_ReturnsUnauthorized()
        {
            // Arrange: Simulate an unauthenticated user (no claims)
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() // No authentication
            };

            // Act: Call the controller method
            var result = await _controller.AwardMedals();

            // Assert: Check that the result is Unauthorized (HTTP 401)
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

        [Test]
        public async Task AwardMedals_UserDoesNotHaveFitbitToken_ReturnsConnectFitbitView()
        {
            // Arrange: Setup mocks
            var identityId = "test-user-id";
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, identityId)
                    }))
                }
            };

            // Simulate that the user does not have a Fitbit token
            _mockOAuthService.Setup(service => service.UserHasFitbitToken(identityId)).ReturnsAsync(false);

            // Act: Call the controller method
            var result = await _controller.AwardMedals();

            // Assert: Check that the result is the "ConnectFitbit" view
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("ConnectFitbit", viewResult.ViewName);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
}
