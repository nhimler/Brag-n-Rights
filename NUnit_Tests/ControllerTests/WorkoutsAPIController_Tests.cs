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
using GymBro_App.Models.DTOs;
using System;
using System.Collections.Generic;
using GymBro_App.Services;

namespace Controller_Tests
{
    [TestFixture]
    public class WorkoutsAPIController_Tests
    {
        private Mock<IWorkoutPlanRepository> _workoutPlanRepoMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILogger<WorkoutsAPIController>> _loggerMock;
        private Mock<IExerciseService> _exerciseServiceMock;
        private UserManager<IdentityUser> _userManager;
        private WorkoutsAPIController _controller;

        [SetUp]
        public void Setup()
        {
            _workoutPlanRepoMock = new Mock<IWorkoutPlanRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<WorkoutsAPIController>>();
            _exerciseServiceMock = new Mock<IExerciseService>(); 

            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManager = new UserManager<IdentityUser>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new WorkoutsAPIController(
                _workoutPlanRepoMock.Object,
                _loggerMock.Object,
                _userManager,
                _userRepositoryMock.Object,
                _exerciseServiceMock.Object 
            );

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "identity_user_id") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Test]
        public void AddExercisesToWorkout_WorkoutPlanNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new AddExercisesToWorkoutDto
            {
                WorkoutPlanId = 1,
                ExerciseApiIds = new List<string> { "0001", "0002" }
            };

            _userRepositoryMock
                .Setup(x => x.GetIdFromIdentityId("identity_user_id"))
                .Returns(0);
            _workoutPlanRepoMock
                .Setup(x => x.FindById(dto.WorkoutPlanId))
                .Returns<WorkoutPlan>(null);

            // Act
            var result = _controller.AddExercisesToWorkout(dto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("Workout plan not found."));
        }

        [Test]
        public void AddExercisesToWorkout_Success_ReturnsOkWithExerciseIDs()
        {
            // Arrange
            var dto = new AddExercisesToWorkoutDto
            {
                WorkoutPlanId = 3,
                ExerciseApiIds = new List<string> { "0001", "0002", "0003" }
            };

            var workoutPlan = new WorkoutPlan
            {
                WorkoutPlanId = dto.WorkoutPlanId,
                UserId = 1,
                WorkoutPlanExercises = new List<WorkoutPlanExercise>()
            };

            _userRepositoryMock
                .Setup(x => x.GetIdFromIdentityId("identity_user_id"))
                .Returns(1);
            _workoutPlanRepoMock
                .Setup(x => x.FindById(dto.WorkoutPlanId))
                .Returns(workoutPlan);

            // Act
            var result = _controller.AddExercisesToWorkout(dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var resultType = okResult.Value.GetType();
            var planIdProperty = resultType.GetProperty("workoutPlanId");
            var exercisesCountProperty = resultType.GetProperty("addedExercisesCount");

            Assert.IsNotNull(planIdProperty);
            Assert.IsNotNull(exercisesCountProperty);

            var returnedPlanId = (int)planIdProperty.GetValue(okResult.Value, null);
            var returnedExercisesCount = (int)exercisesCountProperty.GetValue(okResult.Value, null);

            Assert.That(returnedPlanId, Is.EqualTo(dto.WorkoutPlanId));
            Assert.That(returnedExercisesCount, Is.EqualTo(dto.ExerciseApiIds.Count));
            Assert.That(workoutPlan.WorkoutPlanExercises.Count, Is.EqualTo(dto.ExerciseApiIds.Count));
            _workoutPlanRepoMock.Verify(x => x.Update(workoutPlan), Times.Once);
        }

        [Test]
        public void AddExercisesToWorkout_UpdateThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var dto = new AddExercisesToWorkoutDto
            {
                WorkoutPlanId = 4,
                ExerciseApiIds = new List<string> { "0001" }
            };

            var workoutPlan = new WorkoutPlan
            {
                WorkoutPlanId = dto.WorkoutPlanId,
                UserId = 1,
                WorkoutPlanExercises = new List<WorkoutPlanExercise>()
            };

            _userRepositoryMock
                .Setup(x => x.GetIdFromIdentityId("identity_user_id"))
                .Returns(1);
            _workoutPlanRepoMock
                .Setup(x => x.FindById(dto.WorkoutPlanId))
                .Returns(workoutPlan);
            _workoutPlanRepoMock
                .Setup(x => x.Update(workoutPlan))
                .Throws(new Exception("DB error"));

            // Act
            var result = _controller.AddExercisesToWorkout(dto);

            // Assert
            var statusResult = result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.That(statusResult.StatusCode, Is.EqualTo(500));
            StringAssert.Contains(
                "Error updating workout plan: DB error",
                statusResult.Value.ToString()
            );
        }

        [Test]
        public async Task GetExercisesForPlan_PlanNotFound_ReturnsNotFound()
        {
            // Arrange
            _workoutPlanRepoMock.Setup(x => x.FindById(It.IsAny<int>())).Returns<WorkoutPlan>(null);

            // Act
            var result = await _controller.GetExercisesForPlan(5);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFound = result as NotFoundObjectResult;
            Assert.That(notFound.Value, Is.EqualTo("Plan 5 not found."));
        }

        [Test]
        public async Task GetExercisesForPlan_NoExercises_ReturnsEmptyList()
        {
            // Arrange
            var plan = new WorkoutPlan
            {
                WorkoutPlanId = 1,
                WorkoutPlanExercises = new List<WorkoutPlanExercise>()
            };
            _workoutPlanRepoMock.Setup(x => x.FindById(1)).Returns(plan);

            // Act
            var result = await _controller.GetExercisesForPlan(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var exercises = result.Value as List<ExerciseDTO>;
            Assert.IsNotNull(exercises);
            Assert.That(exercises.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetExercisesForPlan_WithExercises_ReturnsExerciseList()
        {
            // Arrange
            var wpeList = new List<WorkoutPlanExercise>
            {
                new WorkoutPlanExercise { ApiId = "id1" },
                new WorkoutPlanExercise { ApiId = "id2" }
            };
            var plan = new WorkoutPlan
            {
                WorkoutPlanId = 2,
                WorkoutPlanExercises = wpeList
            };
            _workoutPlanRepoMock.Setup(x => x.FindById(2)).Returns(plan);

            var dto1 = new ExerciseDTO { Id = "id1", Name = "Ex1" };
            var dto2 = new ExerciseDTO { Id = "id2", Name = "Ex2" };
            _exerciseServiceMock
                .Setup(s => s.GetExerciseByIdAsync("id1"))
                .ReturnsAsync(new List<ExerciseDTO> { dto1 });
            _exerciseServiceMock
                .Setup(s => s.GetExerciseByIdAsync("id2"))
                .ReturnsAsync(new List<ExerciseDTO> { dto2 });

            // Act
            var result = await _controller.GetExercisesForPlan(2) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var exercises = result.Value as List<ExerciseDTO>;
            Assert.IsNotNull(exercises);
            Assert.That(exercises.Count, Is.EqualTo(2));
            Assert.That(exercises[0].Id, Is.EqualTo("id1"));
            Assert.That(exercises[1].Id, Is.EqualTo("id2"));

            _exerciseServiceMock.Verify(s => s.GetExerciseByIdAsync("id1"), Times.Once);
            _exerciseServiceMock.Verify(s => s.GetExerciseByIdAsync("id2"), Times.Once);
        }

        /*public async Task UpdateSetsAndReps_SuccessOnValidInput()
        {
            //Arrange
            //Act
            //Assert
        }*/

        [TearDown]
        public void TearDown()
        {
            _workoutPlanRepoMock = null;
            _userRepositoryMock = null;
            _loggerMock = null;
            _exerciseServiceMock = null;
            _userManager?.Dispose();
            _userManager = null;
            _controller = null;
        }
    }
}