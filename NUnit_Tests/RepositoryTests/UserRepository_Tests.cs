using Moq;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace Repository_Tests;

public class UserRepository_Tests
{
    private Mock<GymBroDbContext> _mockContext;
    private List<WorkoutPlan> _workoutPlans;
    private List<User> _users;

    private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
        return mockSet;
    }

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<GymBroDbContext>();

        _workoutPlans = new List<WorkoutPlan>
        {
            new WorkoutPlan { UserId = 1, PlanName = "First Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 3, 1), Frequency = "3 times a week", Goal = "Build muscle", IsCompleted = 1, DifficultyLevel = "Beginner" },
            new WorkoutPlan { UserId = 1, PlanName = "Second Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 2, 1), Frequency = "4 times a week", Goal = "Build muscle", IsCompleted = 1, DifficultyLevel = "Intermediate" },
            new WorkoutPlan { UserId = 1, PlanName = "Third Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 1, 25), Frequency = "2 times a week", Goal = "Build muscle", IsCompleted = 0, DifficultyLevel = "Advanced" },
        };

        _users = new List<User>
        {
            new User { UserId = 1, IdentityUserId = "FakeIdentityId1", Username = "Test_User1", Email = "test@email.com", Password = "Don't store passwords here", Age = 42, Gender = "Male", FitnessLevel = "Advanced", Height = 6.0M, Weight = 200.0M, Fitnessgoals = "Build muscle", AccountCreationDate = new DateTime(2025, 1, 1), LastLogin = new DateTime(2025, 1, 1), PreferredWorkoutTime = "Morning", Location = "Test Location", WorkoutPlans = _workoutPlans },
            new User
            {
                UserId = 2,
                IdentityUserId = "ValidIdentityId",
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Age = 30,
                Gender = "Male",
                Weight = 75.5m,
                Height = 180.0m,
                FitnessLevel = "Intermediate",
                Fitnessgoals = "Build muscle",
                PreferredWorkoutTime = "Morning"
            }
        };

        // Mock the DbSet<User>
        var mockSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
    }

    [Test]
    public void GetUserByIdentityUserId_ShouldReturnUser()
    {
        // Arrange
        var mockSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        var repository = new UserRepository(_mockContext.Object);

        // Act
        var result = repository.GetUserByIdentityUserId("FakeIdentityId1");

        // Assert
        string expectedUsername = "Test_User1";
        Assert.That(expectedUsername, Is.EqualTo(result.Username));
    }

    [Test]
    public void GetWorkoutPlansByIdentityUserId_ShouldReturnWorkoutPlans()
    {
        // Arrange
        var mockSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        var repository = new UserRepository(_mockContext.Object);

        // Act
        var result = repository.GetWorkoutPlansByIdentityUserId("FakeIdentityId1");

        // Assert
        int expectedCount = 3;
        Assert.That(expectedCount, Is.EqualTo(result.Count));
    }

    [Test]
    public void GetWorkoutPlansByIdentityUserIdIntOverload_ShouldReturnCompletedWorkoutPlans()
    {
        // Arrange
        var mockSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        var repository = new UserRepository(_mockContext.Object);

        // Act
        var result = repository.GetWorkoutPlansByIdentityUserId("FakeIdentityId1", 1);

        // Assert
        int expectedCount = 2;
        Assert.That(expectedCount, Is.EqualTo(result.Count));
    }

    [Test]
    public void GetWorkoutPlansByIdentityUserIdIntOverload_ShouldReturnUncompletedWorkoutPlans()
    {
        // Arrange
        var mockSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        var repository = new UserRepository(_mockContext.Object);

        // Act
        var result = repository.GetWorkoutPlansByIdentityUserId("FakeIdentityId1", 0);

        // Assert
        int expectedCount = 1;
        Assert.That(expectedCount, Is.EqualTo(result.Count));
    }

    [Test]
    public void UpdateUser_ShouldUpdateUserWhenIdentityIdIsValid()
    {
        // Arrange
        var repository = new UserRepository(_mockContext.Object);
        var userInfo = new UserInfoModel
        {
            FirstName = "Jane",
            LastName = "Smith",
            Age = 28,
            Gender = "Female",
            Weight = 65.0m,
            Height = 170.0m,
            FitnessLevel = "Advanced",
            Fitnessgoals = "Lose weight",
            PreferredWorkoutTime = "Evening"
        };

        // Act
        repository.UpdateUser("ValidIdentityId", userInfo);

        // Assert
        var updatedUser = _users.FirstOrDefault(u => u.IdentityUserId == "ValidIdentityId");
        Assert.That(updatedUser?.FirstName, Is.EqualTo("Jane"));
        Assert.That(updatedUser?.LastName, Is.EqualTo("Smith"));
        Assert.That(updatedUser?.Age, Is.EqualTo(28));
        Assert.That(updatedUser?.Gender, Is.EqualTo("Female"));
        Assert.That(updatedUser?.Weight, Is.EqualTo(65.0m));
        Assert.That(updatedUser?.Height, Is.EqualTo(170.0m));
        Assert.That(updatedUser?.FitnessLevel, Is.EqualTo("Advanced"));
        Assert.That(updatedUser?.Fitnessgoals, Is.EqualTo("Lose weight"));
        Assert.That(updatedUser?.PreferredWorkoutTime, Is.EqualTo("Evening"));
    }

    [Test]
    public void UpdateUser_ShouldNotBeUpdatedWhenIdentityIdIsInvalid()
    {
        // Arrange
        var repository = new UserRepository(_mockContext.Object);
        var userInfo = new UserInfoModel
        {
            FirstName = "Jane",
            LastName = "Smith",
            Age = 28,
            Gender = "Female",
            Weight = 65.0m,
            Height = 170.0m,
            FitnessLevel = "Advanced",
            Fitnessgoals = "Lose weight",
            PreferredWorkoutTime = "Evening"
        };

        // Act
        repository.UpdateUser("InvalidIdentityId", userInfo);

        // Assert
        Assert.That(_users[1].FirstName, Is.Not.EqualTo(userInfo.FirstName));
        Assert.That(_users[1].LastName, Is.Not.EqualTo(userInfo.LastName));
        Assert.That(_users[1].Age, Is.Not.EqualTo(userInfo.Age));
        Assert.That(_users[1].Gender, Is.Not.EqualTo(userInfo.Gender));
        Assert.That(_users[1].Weight, Is.Not.EqualTo(userInfo.Weight));
        Assert.That(_users[1].Height, Is.Not.EqualTo(userInfo.Height));
        Assert.That(_users[1].FitnessLevel, Is.Not.EqualTo(userInfo.FitnessLevel));
        Assert.That(_users[1].Fitnessgoals, Is.Not.EqualTo(userInfo.Fitnessgoals));
        Assert.That(_users[1].PreferredWorkoutTime, Is.Not.EqualTo(userInfo.PreferredWorkoutTime));
    }
}