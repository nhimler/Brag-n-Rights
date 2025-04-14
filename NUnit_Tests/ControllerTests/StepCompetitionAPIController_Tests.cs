using GymBro_App.Controllers;
using GymBro_App.DAL.Abstract;
using GymBro_App.Models;
using GymBro_App.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Controller_Tests
{
    [TestFixture]
    public class StepCompetitionAPIController_Tests
    {
        private Mock<IStepCompetitionRepository> _mockRepo;
        private StepCompetitionAPIController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IStepCompetitionRepository>();
            _controller = new StepCompetitionAPIController(_mockRepo.Object);
        }

        private void SetUser(string identityId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, identityId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task SearchUser_ReturnsUnauthorized_IfIdentityMissing()
        {
            // No user is set
            var result = await _controller.SearchUser("john");

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        [Test]
        public async Task SearchUser_ReturnsOk_WithExpectedUsers()
        {
            SetUser("user123");
            var expectedUsers = new List<string> { "john", "jane" };

            _mockRepo.Setup(r => r.SearchUsersWithTokenAsync("jo", "user123"))
                     .ReturnsAsync(expectedUsers);

            var result = await _controller.SearchUser("jo") as OkObjectResult;

            Assert.IsNotNull(result);
            var users = result.Value as List<string>;
            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count, Is.EqualTo(2));
            Assert.That(users, Does.Contain("john"));
        }

        [Test]
        public async Task StartCompetition_ReturnsBadRequest_IfCreationFails()
        {
            SetUser("user123");

            _mockRepo.Setup(r => r.CreateCompetitionAsync("user123"))
                     .ReturnsAsync((StepCompetition)null);

            var result = await _controller.StartCompetition(new List<string>());

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetUserCompetitions_ReturnsUnauthorized_IfNoIdentity()
        {
            // No identity set
            var result = await _controller.GetUserCompetitions();

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        [Test]
        public async Task GetUserCompetitions_ReturnsOk_WithCompetitions()
        {
            SetUser("user456");

            var competitions = new List<UserCompetitionViewModel>
            {
                new UserCompetitionViewModel { CompetitionID = 1 },
                new UserCompetitionViewModel { CompetitionID = 2 }
            };

            _mockRepo.Setup(r => r.GetCompetitionsForUserAsync("user456"))
                     .ReturnsAsync(competitions);

            var result = await _controller.GetUserCompetitions() as OkObjectResult;

            Assert.IsNotNull(result);
            var data = result.Value as List<UserCompetitionViewModel>;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count, Is.EqualTo(2));
        }
    }
}
