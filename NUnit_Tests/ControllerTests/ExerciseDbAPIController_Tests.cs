using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymBro_App.Controllers;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Controller_Tests
{
    public class ExerciseDbAPIController_Tests
    {
        private Mock<IExerciseService> _mockService;
        private Mock<ILogger<ExerciseDbAPIController>> _mockLogger;
        private ExerciseDbAPIController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<IExerciseService>();
            _mockLogger = new Mock<ILogger<ExerciseDbAPIController>>();
            _controller = new ExerciseDbAPIController(_mockService.Object, _mockLogger.Object);
        }

        /*[Test]
        public async Task GetExercisesAsync_ReturnsListOfExercises()
        {
            // Arrange
            var exercises = new List<ExerciseDTO>
            {
                new ExerciseDTO { Id = "0001", Name = "sit-up" },
                new ExerciseDTO { Id = "0002", Name = "sit-up" }
            };
            _mockService.Setup(service => service.GetExercisesAsync()).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExercises();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(exercises));
        }

        [Test]
        public async Task GetExercisesAsync_ReturnsNotFound_WhenNoExercises()
        {
            // Arrange
            _mockService.Setup(service => service.GetExercisesAsync()).ReturnsAsync(new List<ExerciseDTO>());

            // Act
            var result = await _controller.GetExercises();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }*/

        [Test]
        public async Task GetExerciseAsync_ReturnsExercise()
        {
            // Arrange
            var exercises = new List<ExerciseDTO>
            {
                new ExerciseDTO { Id = "0001", Name = "sit-up" }
            };
            _mockService.Setup(service => service.GetExerciseAsync("sit-up")).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExercise("sit-up");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(exercises));
        }

        [Test]
        public async Task GetExerciseAsync_ReturnsNotFound_WhenNoExercise()
        {
            // Arrange
            _mockService.Setup(service => service.GetExerciseAsync("NonExistentExercise")).ReturnsAsync(new List<ExerciseDTO>());

            // Act
            var result = await _controller.GetExercise("NonExistentExercise");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.Value, Is.EqualTo("No exercises found for 'NonExistentExercise'."));
        }

        [Test]
        public async Task GetExerciseById_ReturnsExercise()
        {
            // Arrange
            var exercises = new List<ExerciseDTO>
            {
                new ExerciseDTO { Id = "0001", Name = "sit-up" }
            };
            _mockService.Setup(service => service.GetExerciseByIdAsync("0001")).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExerciseById("0001");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(exercises));
        }

        [Test]
        public async Task GetExerciseById_ReturnsNotFound_WhenNoExercise()
        {
            // Arrange
            _mockService.Setup(service => service.GetExerciseByIdAsync("NonExistentId")).ReturnsAsync(new List<ExerciseDTO>());

            // Act
            var result = await _controller.GetExerciseById("NonExistentId");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.Value, Is.EqualTo("No exercise found with ID 'NonExistentId'. Valid IDs are in the format 0001."));
        }

        [Test]
        public async Task GetExerciseByBodyPart_ReturnsExercise()
        {
            // Arrange
            var exercises = new List<ExerciseDTO>
            {
                new ExerciseDTO { Id = "0001", Name = "sit-up", BodyPart = "waist" }
            };
            _mockService.Setup(service => service.GetExerciseByBodyPartAsync("waist")).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExerciseByBodyPart("waist");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(exercises));
        }

        [Test]
        public async Task GetExerciseByBodyPart_ReturnsNotFound_WhenBodyPartDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetExerciseByBodyPartAsync("NonExistentBodyPart")).ReturnsAsync(new List<ExerciseDTO>());

            // Act
            var result = await _controller.GetExerciseByBodyPart("NonExistentBodyPart");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult.Value, Is.EqualTo("No exercises found for body part 'NonExistentBodyPart'. Valid body parts are: back, cardio, chest, lower arms, lower legs, neck, shoulders, upper arms, upper legs, waist."));
        }

        [TearDown]
        public void TearDown()
        {
            _mockService = null;
            _mockLogger = null;
            _controller = null;
        }
    }
}