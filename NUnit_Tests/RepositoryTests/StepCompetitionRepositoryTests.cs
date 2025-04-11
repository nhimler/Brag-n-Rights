using GymBro_App.Models;
using GymBro_App.DAL.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

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

    [SetUp]
public void SetUp()
{
    _mockContext = new Mock<GymBroDbContext>();
    _repository = new StepCompetitionRepository(_mockContext.Object);



    // StepCompetition store
    _competitionStore = new List<StepCompetition>();
    var mockCompetitionSet = new Mock<DbSet<StepCompetition>>();
    mockCompetitionSet.Setup(m => m.AddAsync(It.IsAny<StepCompetition>(), It.IsAny<CancellationToken>()))
        .Callback<StepCompetition, CancellationToken>((comp, _) => _competitionStore.Add(comp))
        .Returns((StepCompetition comp, CancellationToken _) => ValueTask.FromResult((EntityEntry<StepCompetition>)null));
    _mockContext.Setup(c => c.StepCompetitions).Returns(mockCompetitionSet.Object);

    // StepCompetitionParticipants store
    var participantStore = new List<StepCompetitionParticipant>();
    var mockParticipantSet = new Mock<DbSet<StepCompetitionParticipant>>();
    mockParticipantSet.Setup(m => m.AddAsync(It.IsAny<StepCompetitionParticipant>(), It.IsAny<CancellationToken>()))
        .Callback<StepCompetitionParticipant, CancellationToken>((p, _) => participantStore.Add(p))
        .Returns((StepCompetitionParticipant p, CancellationToken _) => ValueTask.FromResult((EntityEntry<StepCompetitionParticipant>)null));
    _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(mockParticipantSet.Object);

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

    // Save changes
    _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
}


    [Test]
    public async Task SearchUsersWithTokenAsync_ReturnsMatchingUsers_ExcludesCurrentUser_AndOnlyWithToken()
    {
        // Arrange
        var repository = new StepCompetitionRepository(_mockContext.Object);

        // Act
        var result = await repository.SearchUsersWithTokenAsync("john", "currentUser");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result, Contains.Item("johnny"));
        Assert.That(result, Does.Not.Contain("john")); // current user
        Assert.That(result, Does.Not.Contain("jane")); // no token
    }

    [Test]
    public async Task SearchUsersWithTokenAsync_ReturnsEmpty_WhenNoMatches()
    {
        // Arrange
        var repository = new StepCompetitionRepository(_mockContext.Object);

        // Act
        var result = await repository.SearchUsersWithTokenAsync("nonexistent", "currentUser");

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task SearchUsersWithTokenAsync_IgnoresUsersWithNullUsername()
    {
        // Arrange
        _users.Add(new User { UserId = 4, IdentityUserId = "nullUser", Username = null });
        _tokens.Add(new TokenEntity { UserId = 4 });

        // Refresh mock with new data
        var userSet = GetMockDbSet(_users.AsQueryable());
        _mockContext.Setup(c => c.Users).Returns(userSet.Object);

        var repository = new StepCompetitionRepository(_mockContext.Object);

        // Act
        var result = await repository.SearchUsersWithTokenAsync("john", "currentUser");

        // Assert
        Assert.That(result, Does.Not.Contain(null));
        Assert.That(result, Does.Not.Contain(""));
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
    public async Task CreateCompetitionAsync_AddsToDatabaseAndCallsSaveChanges()
    {
        // Arrange
        var currentUserId = "user456";

        // Act
        var result = await _repository.CreateCompetitionAsync(currentUserId);

        // Assert
        _mockCompetitionSet.Verify(m => m.AddAsync(It.IsAny<StepCompetition>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
    public async Task InviteUsersToCompetitionAsync_AddsInvitedAndCurrentUserToParticipants()
    {
        // Arrange
        var competition = new StepCompetition { CompetitionID = 1 };
        var invitedUsernames = new List<string> { "johnny", "jane" }; // "jane" won't be added (not in Users list with token)
        
        // Act
        await _repository.InviteUsersToCompetitionAsync("currentUser", competition, invitedUsernames);

        // Assert
        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId == "anotherUser" && p.StepCompetitionId == 1), 
            It.IsAny<CancellationToken>()), Times.Once);

        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId == "currentUser" && p.StepCompetitionId == 1), 
            It.IsAny<CancellationToken>()), Times.Once);

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task InviteUsersToCompetitionAsync_DoesNotDuplicateCurrentUser()
    {
        // Arrange
        var competition = new StepCompetition { CompetitionID = 2 };
        var invitedUsernames = new List<string> { "john", "johnny" }; // "john" is the current user

        // Act
        await _repository.InviteUsersToCompetitionAsync("currentUser", competition, invitedUsernames);

        // Assert
        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId == "currentUser" && p.StepCompetitionId == 2), 
            It.IsAny<CancellationToken>()), Times.Once);

        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId == "anotherUser" && p.StepCompetitionId == 2), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task InviteUsersToCompetitionAsync_HandlesEmptyInvitedList_AddsOnlyCurrentUser()
    {
        // Arrange
        var competition = new StepCompetition { CompetitionID = 3 };
        var invitedUsernames = new List<string>(); // empty list

        // Act
        await _repository.InviteUsersToCompetitionAsync("currentUser", competition, invitedUsernames);

        // Assert
        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId == "currentUser" && p.StepCompetitionId == 3), 
            It.IsAny<CancellationToken>()), Times.Once);

        _mockContext.Verify(c => c.StepCompetitionParticipants.AddAsync(
            It.Is<StepCompetitionParticipant>(p => p.IdentityId != "currentUser"), 
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task GetCompetitionsForUserAsync_ReturnsCorrectCompetitionsWithActiveParticipants()
    {
        // Arrange
        var stepCompetition = new StepCompetition
        {
            CompetitionID = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            Participants = new List<StepCompetitionParticipant>()
        };

        var participant1 = new StepCompetitionParticipant
        {
            StepCompetitionId = 1,
            StepCompetition = stepCompetition,
            IdentityId = "user123",
            IsActive = true,
            Steps = 5000,
            User = new User { Username = "john" }
        };

        var participant2 = new StepCompetitionParticipant
        {
            StepCompetitionId = 1,
            StepCompetition = stepCompetition,
            IdentityId = "user456",
            IsActive = true,
            Steps = 3000,
            User = new User { Username = "jane" }
        };

        var currentUserParticipant = new StepCompetitionParticipant
        {
            StepCompetitionId = 1,
            StepCompetition = stepCompetition,
            IdentityId = "currentUser",
            IsActive = true,
            Steps = 7000,
            User = new User { Username = "me" }
        };

        stepCompetition.Participants = new List<StepCompetitionParticipant> { participant1, participant2, currentUserParticipant };

        _participantStore = new List<StepCompetitionParticipant> { currentUserParticipant };

        var queryableParticipants = _participantStore.AsQueryable();

        _mockParticipantSet = new Mock<DbSet<StepCompetitionParticipant>>();
        _mockParticipantSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Provider).Returns(queryableParticipants.Provider);
        _mockParticipantSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Expression).Returns(queryableParticipants.Expression);
        _mockParticipantSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.ElementType).Returns(queryableParticipants.ElementType);
        _mockParticipantSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.GetEnumerator()).Returns(() => queryableParticipants.GetEnumerator());

        _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(_mockParticipantSet.Object);

        // Act
        var result = await _repository.GetCompetitionsForUserAsync("currentUser");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        var comp = result.First();
        Assert.That(comp.CompetitionID, Is.EqualTo(1));
        Assert.That(comp.Participants.Count, Is.EqualTo(3)); // includes currentUser + 2 others
        Assert.That(comp.Participants.Any(p => p.Username == "me"), Is.True);
        Assert.That(comp.Participants.Any(p => p.Username == "john"), Is.True);
        Assert.That(comp.Participants.Any(p => p.Username == "jane"), Is.True);
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

        _participantStore = new List<StepCompetitionParticipant> { activeParticipant };
        var queryable = _participantStore.AsQueryable();

        var mockSet = new Mock<DbSet<StepCompetitionParticipant>>();
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Provider).Returns(queryable.Provider);
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


    [Test]
    public async Task GetCompetitionsForUserAsync_ReturnsMultipleCompetitionsForUser()
    {
        // Arrange
        var comp1 = new StepCompetition
        {
            CompetitionID = 10,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            Participants = new List<StepCompetitionParticipant>()
        };

        var comp2 = new StepCompetition
        {
            CompetitionID = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(5),
            IsActive = true,
            Participants = new List<StepCompetitionParticipant>()
        };

        var p1 = new StepCompetitionParticipant
        {
            StepCompetition = comp1,
            StepCompetitionId = 10,
            IdentityId = "currentUser",
            IsActive = true,
            Steps = 5000,
            User = new User { Username = "me" }
        };

        var p2 = new StepCompetitionParticipant
        {
            StepCompetition = comp2,
            StepCompetitionId = 20,
            IdentityId = "currentUser",
            IsActive = true,
            Steps = 3000,
            User = new User { Username = "me" }
        };

        comp1.Participants = new List<StepCompetitionParticipant> { p1 };
        comp2.Participants = new List<StepCompetitionParticipant> { p2 };

        _participantStore = new List<StepCompetitionParticipant> { p1, p2 };
        var queryable = _participantStore.AsQueryable();

        var mockSet = new Mock<DbSet<StepCompetitionParticipant>>();
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetCompetitionsForUserAsync("currentUser");

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Any(c => c.CompetitionID == 10), Is.True);
        Assert.That(result.Any(c => c.CompetitionID == 20), Is.True);
    }
    [Test]
    public async Task GetCompetitionsForUserAsync_ReturnsEmptyList_WhenUserHasNoCompetitions()
    {
        // Arrange
        _participantStore = new List<StepCompetitionParticipant>(); // No entries

        var queryable = _participantStore.AsQueryable();
        var mockSet = new Mock<DbSet<StepCompetitionParticipant>>();
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<StepCompetitionParticipant>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        _mockContext.Setup(c => c.StepCompetitionParticipants).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetCompetitionsForUserAsync("nonexistentUser");

        // Assert
        Assert.That(result, Is.Empty);
    }
}
