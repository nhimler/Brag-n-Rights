using Moq;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace Repository_Tests;

public class GymUserRepository_Tests
{
   private Mock<DbSet<GymUser>> _mockDbSet;
        private Mock<GymBroDbContext> _mockContext;
        private List<GymUser> _gymUsers;

        [SetUp]
        public void SetUp()
        {
            // Sample data for GymUser
            _gymUsers = new List<GymUser>
            {
                new GymUser { GymUserId = 1, UserId = 1, ApiGymId = "Gym A" },
                new GymUser { GymUserId = 2, UserId = 1, ApiGymId = "Gym B" },
                new GymUser { GymUserId = 3, UserId = 2, ApiGymId = "Gym C" }
            };

            // Mock DbSet
            _mockDbSet = GetMockDbSet(_gymUsers.AsQueryable());

            // Mock DbContext
            _mockContext = new Mock<GymBroDbContext>();
            _mockContext.Setup(c => c.GymUsers).Returns(_mockDbSet.Object);
        }

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        [Test]
        public void GetAllGymUsersByUserId_ShouldReturnGymUsersForGivenUserId()
        {
            // Arrange
            var repository = new GymUserRepository(_mockContext.Object);
            int userId = 1;

            // Act
            var result = repository.GetAllGymUsersByUserId(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(u => u.ApiGymId == "Gym A"), Is.True);
            Assert.That(result.Any(u => u.ApiGymId == "Gym B"), Is.True);
        }

        [Test]
        public void GetAllGymUsersByUserId_ShouldReturnEmptyListForNonExistentUserId()
        {
            // Arrange
            var repository = new GymUserRepository(_mockContext.Object);
            int userId = 99; // Non-existent userId

            // Act
            var result = repository.GetAllGymUsersByUserId(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
}