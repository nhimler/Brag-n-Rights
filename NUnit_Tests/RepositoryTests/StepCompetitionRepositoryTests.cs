using GymBro_App.Models;
using GymBro_App.ViewModels;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Linq.Expressions;


using TimeZoneConverter;

namespace Repository_Tests;

[TestFixture]
public class StepCompetitionRepository_Tests
{
    private Mock<GymBroDbContext> _mockContext;
    private List<User> _users;
    private List<TokenEntity> _tokens;
    private Mock<DbSet<StepCompetition>> _mockCompetitionSet;
    private List<StepCompetition> _competitionStore;
    private List<StepCompetitionParticipant> _participantStore;
    private Mock<DbSet<StepCompetitionParticipant>> _mockParticipantSet;
    private StepCompetitionRepository _repository;

    private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());

    public T Current => _inner.Current;
}

public interface IAsyncQueryProvider : IQueryProvider
{
    IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression);
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
}

public class TestAsyncQueryProvider<T> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<T>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) =>
        new TestAsyncEnumerable<TResult>(expression);

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
        Task.FromResult(Execute<TResult>(expression));
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

    [SetUp]
public void SetUp()
{
    _mockContext = new Mock<GymBroDbContext>();
    _repository = new StepCompetitionRepository(_mockContext.Object);

    // StepCompetition store
    _competitionStore = new List<StepCompetition>();
    _mockCompetitionSet = new Mock<DbSet<StepCompetition>>();
    _mockCompetitionSet
        .Setup(m => m.AddAsync(It.IsAny<StepCompetition>(), It.IsAny<CancellationToken>()))
        .Callback<StepCompetition, CancellationToken>((comp, _) => _competitionStore.Add(comp))
        .Returns((StepCompetition comp, CancellationToken _) => ValueTask.FromResult((EntityEntry<StepCompetition>)null));
    _mockContext.Setup(c => c.StepCompetitions).Returns(_mockCompetitionSet.Object);

    // StepCompetitionParticipants store
    _participantStore = new List<StepCompetitionParticipant>();
    _mockParticipantSet = new Mock<DbSet<StepCompetitionParticipant>>();
    _mockParticipantSet
        .Setup(m => m.AddAsync(It.IsAny<StepCompetitionParticipant>(), It.IsAny<CancellationToken>()))
        .Callback<StepCompetitionParticipant, CancellationToken>((p, _) => _participantStore.Add(p))
        .Returns((StepCompetitionParticipant p, CancellationToken _) => ValueTask.FromResult((EntityEntry<StepCompetitionParticipant>)null));
    _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(_mockParticipantSet.Object);

    // Users
    _users = new List<User>
    {
        new User { UserId = 1, IdentityUserId = "currentUser", Username = "john" },
        new User { UserId = 2, IdentityUserId = "anotherUser", Username = "johnny" },
        new User { UserId = 3, IdentityUserId = "noTokenUser", Username = "jane" }
    };

    var userSet = GetMockDbSet(_users.AsQueryable());
    _mockContext.Setup(c => c.Users).Returns(userSet.Object);

    // Tokens
    _tokens = new List<TokenEntity>
    {
        new TokenEntity { UserId = 1 },
        new TokenEntity { UserId = 2 }
    };

    var tokenSet = GetMockDbSet(_tokens.AsQueryable());
    _mockContext.Setup(c => c.Tokens).Returns(tokenSet.Object);

    // SaveChanges
    _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
}








        [Test]
    public async Task CreateCompetitionAsync_CreatesCompetitionWithCorrectDatesAndIsActive()
    {
        // Arrange
        var currentUserId = "user123";

        // Act
        var result = await _repository.CreateCompetitionAsync(currentUserId);

        // Assert
        Assert.That(result.CreatorIdentityId, Is.EqualTo(currentUserId));
        Assert.That(result.IsActive, Is.True);
        Assert.That(result.EndDate, Is.EqualTo(result.StartDate.AddDays(7)).Within(TimeSpan.FromSeconds(1)));
        Assert.That(_competitionStore.Count, Is.EqualTo(1));
    }



    [Test]
    public async Task CreateCompetitionAsync_SetsPacificTimeDates()
    {
        // Arrange
        var currentUserId = "user789";
        var pacificZone = TZConvert.GetTimeZoneInfo("Pacific Standard Time");
        var pacificNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone);

        // Act
        var result = await _repository.CreateCompetitionAsync(currentUserId);

        // Assert
        Assert.That(result.StartDate.Date, Is.EqualTo(pacificNow.Date));
        Assert.That(result.EndDate.Date, Is.EqualTo(pacificNow.AddDays(7).Date));
    }
 

    [Test]
    public async Task GetCompetitionsForUserAsync_ExcludesInactiveParticipants()
    {
        // Arrange
        var stepCompetition = new StepCompetition
        {
            CompetitionID = 2,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            Participants = new List<StepCompetitionParticipant>()
        };

        var activeParticipant = new StepCompetitionParticipant
        {
            StepCompetitionId = 2,
            StepCompetition = stepCompetition,
            IdentityId = "currentUser",
            IsActive = true,
            Steps = 1000,
            User = new User { Username = "me" }
        };

        var inactiveParticipant = new StepCompetitionParticipant
        {
            StepCompetitionId = 2,
            StepCompetition = stepCompetition,
            IdentityId = "user123",
            IsActive = false,
            Steps = 3000,
            User = new User { Username = "inactiveUser" }
        };

        stepCompetition.Participants = new List<StepCompetitionParticipant> { activeParticipant, inactiveParticipant };

        _participantStore = new List<StepCompetitionParticipant> { activeParticipant, inactiveParticipant };
        
        var queryable = _participantStore.AsQueryable();
        var mockSet = new Mock<DbSet<StepCompetitionParticipant>>();
        
        mockSet.As<IAsyncEnumerable<StepCompetitionParticipant>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken ct) => new TestAsyncEnumerator<StepCompetitionParticipant>(_participantStore.GetEnumerator()));
        
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<StepCompetitionParticipant>(queryable.Provider));
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetCompetitionsForUserAsync("currentUser");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().Participants.Count, Is.EqualTo(1));
        Assert.That(result.First().Participants.Any(p => p.Username == "inactiveUser"), Is.False);
    }
    
}
