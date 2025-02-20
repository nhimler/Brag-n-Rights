using Moq;
using GymBro_App.DAL.Abstract;
using GymBro_App.Controllers;
using GymBro_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Controller_Tests;

[TestFixture]
    public class WorkoutsController_Tests
    {
        private Mock<IWorkoutPlanRepository> _mockRepo;
        private WorkoutsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IWorkoutPlanRepository>();
            _controller = new WorkoutsController(_mockRepo.Object);
        }

        [Test]
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
        }

        [Test]
        public void WorkoutCreationPage_ReturnsViewResult()
        {
            // Act
            var result = _controller.WorkoutCreationPage();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var workoutPlan = new WorkoutPlan();
            _mockRepo.Setup(repo => repo.Add(It.IsAny<WorkoutPlan>()));

            // Act
            var result = _controller.Create(workoutPlan);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void Create_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var workoutPlan = new WorkoutPlan();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Create(workoutPlan);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.EqualTo(workoutPlan));
        }


        [TearDown]
        public void TearDown()
        {
            _mockRepo = null;
            _controller.Dispose();
            _controller = null;
        }
}