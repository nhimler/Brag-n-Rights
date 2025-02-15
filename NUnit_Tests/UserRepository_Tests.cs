using Moq;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;

public class UserRepository_Tests
{
    private Mock<GymBroDbContext> _mockContext;
    private Mock<DbSet<User>> _mockSet;
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

        _users = new List<User>
        {
            new User { UserId = 1, IdentityUserId = "FakeIdentityId1", Username = "Test_User1", Email = "test@email.com", Password = "Don't store passwords here", Age = 42, Gender = "Male", FitnessLevel = "Advanced", Height = 6.0M, Weight = 200.0M, Fitnessgoals = "Build muscle", AccountCreationDate = new DateTime(2025, 1, 1), LastLogin = new DateTime(2025, 1, 1), PreferredWorkoutTime = "Morning", Location = "Test Location"},
        };

        _workoutPlans = new List<WorkoutPlan>
        {
            new WorkoutPlan { UserId = 1, PlanName = "First Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 3, 1), Frequency = "3 times a week", Goal = "Build muscle", IsCompleted = 1, DifficultyLevel = "Beginner" },
            new WorkoutPlan { UserId = 1, PlanName = "Second Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 2, 1), Frequency = "4 times a week", Goal = "Build muscle", IsCompleted = 1, DifficultyLevel = "Intermediate" },
            new WorkoutPlan { UserId = 1, PlanName = "Third Plan", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 1, 25), Frequency = "2 times a week", Goal = "Build muscle", IsCompleted = 0, DifficultyLevel = "Advanced" },
        };


        
    }

    [Test]
    public void GetUserByIdentityUserId_ShouldReturnUser()
    {
        return;
    }
}