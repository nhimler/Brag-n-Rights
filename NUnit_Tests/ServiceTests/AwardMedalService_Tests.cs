using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBro_App.Models;
using GymBro_App.Services;
using GymBro_App.DAL.Abstract;


[TestFixture]
public class AwardServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IMedalRepository> _medalRepositoryMock;
    private Mock<IUserMedalRepository> _userMedalRepositoryMock;
    private Mock<IBiometricDatumRepository> _biometricDatumRepositoryMock;
    private Mock<IOAuthService> _oAuthServiceMock;
    private AwardMedalService _awardService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _medalRepositoryMock = new Mock<IMedalRepository>();
        _userMedalRepositoryMock = new Mock<IUserMedalRepository>();
        _biometricDatumRepositoryMock = new Mock<IBiometricDatumRepository>();
        _oAuthServiceMock = new Mock<IOAuthService>();

        _awardService = new AwardMedalService(
            _userRepositoryMock.Object,
            _medalRepositoryMock.Object,
            _userMedalRepositoryMock.Object,
            _biometricDatumRepositoryMock.Object,
            _oAuthServiceMock.Object
        );
    }

    [Test]
    public async Task AwardUserdMedalsAsync_UserEarnsNewMedal_ReturnsAwardedMedals()
    {
        // Arrange
        string identityId = "test-identity";
        int userId = 1;
        int steps = 10000;

        // Mock user lookup
        _userRepositoryMock
            .Setup(repo => repo.GetIdFromIdentityId(identityId))
            .Returns(userId);

        // Mock OAuthService to return specific step count
        _oAuthServiceMock
            .Setup(service => service.GetAccessToken(identityId))
            .ReturnsAsync("mock-access-token");

        _oAuthServiceMock
            .Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(steps);

        // Mock medals
        var medals = new List<Medal>
        {
            new Medal
            {
                MedalId = 1,
                Name = "10K Steps",
                StepThreshold = 10000,
                Description = "Awarded for walking 10,000 steps",
                Image = "image.png"
            }
        };

        _medalRepositoryMock
            .Setup(repo => repo.GetAllMedalsAsync())
            .ReturnsAsync(medals);

        // Mock earned medals (none earned yet)
        _userMedalRepositoryMock
            .Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId))
            .ReturnsAsync(new List<UserMedal>());

        // Mock biometric data repo
        _biometricDatumRepositoryMock
            .Setup(repo => repo.GetUserStepsAsync(userId))
            .ReturnsAsync(steps);

        // Act
        var result = await _awardService.AwardUserdMedalsAsync(identityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
        Assert.AreEqual(1, result.AwardedMedals.Count);
        Assert.AreEqual("10K Steps", result.AwardedMedals[0].MedalName);

        // Verify new medal was saved
        _userMedalRepositoryMock.Verify(
            repo => repo.AddBatchUserMedalsAsync(It.Is<List<UserMedal>>(medals =>
                medals.Count == 1 && medals[0].MedalId == 1)),
            Times.Once
        );
    }

    [Test]
    public async Task AwardUserdMedalsAsync_UserAlreadyEarnedMedal_NoDuplicateAward()
    {
        // Arrange
        string identityId = "test-identity";
        int userId = 1;
        int steps = 10000;

        _userRepositoryMock
            .Setup(repo => repo.GetIdFromIdentityId(identityId))
            .Returns(userId);

        _oAuthServiceMock
            .Setup(service => service.GetAccessToken(identityId))
            .ReturnsAsync("mock-access-token");

        _oAuthServiceMock
            .Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(steps);

        var medals = new List<Medal>
        {
            new Medal
            {
                MedalId = 1,
                Name = "10K Steps",
                StepThreshold = 10000,
                Description = "Awarded for walking 10,000 steps",
                Image = "image.png"
            }
        };

        _medalRepositoryMock
            .Setup(repo => repo.GetAllMedalsAsync())
            .ReturnsAsync(medals);

        // Mock that user has already earned the medal today
        _userMedalRepositoryMock
            .Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId))
            .ReturnsAsync(new List<UserMedal>
            {
                new UserMedal { MedalId = 1, UserId = userId }
            });

        // Act
        var result = await _awardService.AwardUserdMedalsAsync(identityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
        Assert.AreEqual(1, result.AwardedMedals.Count);
        Assert.IsTrue(result.AwardedMedals[0].Locked);

        // Make sure NO new medal was added
        _userMedalRepositoryMock.Verify(
            repo => repo.AddBatchUserMedalsAsync(It.IsAny<List<UserMedal>>()),
            Times.Never
        );
    }

    [Test]
    public async Task AwardUserdMedalsAsync_UserHasNoSteps_NoMedalAwarded()
    {
        // Arrange
        string identityId = "test-identity";
        int userId = 1;

        _userRepositoryMock
            .Setup(repo => repo.GetIdFromIdentityId(identityId))
            .Returns(userId);

        _oAuthServiceMock
            .Setup(service => service.GetAccessToken(identityId))
            .ReturnsAsync("mock-access-token");

        _oAuthServiceMock
            .Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(0); // No steps

        _medalRepositoryMock
            .Setup(repo => repo.GetAllMedalsAsync())
            .ReturnsAsync(new List<Medal>());

        _userMedalRepositoryMock
            .Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId))
            .ReturnsAsync(new List<UserMedal>());

        // Act
        var result = await _awardService.AwardUserdMedalsAsync(identityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
        Assert.IsEmpty(result.AwardedMedals);
    }
        [Test]
    public async Task AwardUserdMedalsAsync_NoMedalsDefined_ReturnsEmptyList()
    {
        // Arrange
        string identityId = "test-identity";
        int userId = 1;

        _userRepositoryMock.Setup(repo => repo.GetIdFromIdentityId(identityId)).Returns(userId);
        _medalRepositoryMock.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(new List<Medal>());
        _userMedalRepositoryMock.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId)).ReturnsAsync(new List<UserMedal>());
        _oAuthServiceMock.Setup(service => service.GetAccessToken(identityId)).ReturnsAsync("mock-access-token");
        _oAuthServiceMock.Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(10000);

        // Act
        var result = await _awardService.AwardUserdMedalsAsync(identityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result.AwardedMedals);
    }

[Test]
public async Task AwardUserdMedalsAsync_UserEarnsMultipleMedalsAtOnce_ReturnsAllMedals()
{
    // Arrange
    string identityId = "test-identity";
    int userId = 1;
    int steps = 15000; // ✅ High enough to meet multiple medal thresholds

    var medals = new List<Medal>
    {
        new Medal { MedalId = 1, StepThreshold = 5000, Name = "5K Medal", Image = "5k.png" },
        new Medal { MedalId = 2, StepThreshold = 10000, Name = "10K Medal", Image = "10k.png" },
        new Medal { MedalId = 3, StepThreshold = 15000, Name = "15K Medal", Image = "15k.png" }
    };

    _userRepositoryMock.Setup(repo => repo.GetIdFromIdentityId(identityId)).Returns(userId);
    _medalRepositoryMock.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
    _userMedalRepositoryMock.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId)).ReturnsAsync(new List<UserMedal>()); // ✅ No medals earned yet
    _oAuthServiceMock.Setup(service => service.GetAccessToken(identityId)).ReturnsAsync("mock-access-token");
    _oAuthServiceMock.Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(steps);
    _biometricDatumRepositoryMock.Setup(repo => repo.GetUserStepsAsync(userId)).ReturnsAsync(steps);

    // Act
    var result = await _awardService.AwardUserdMedalsAsync(identityId);

    // Assert
    Assert.AreEqual(3, result.AwardedMedals.Count); 
    Assert.IsTrue(result.AwardedMedals.Any(m => m.MedalId == 1 && !m.Locked)); // Medal 1 should be unlocked
    Assert.IsTrue(result.AwardedMedals.Any(m => m.MedalId == 2 && !m.Locked)); // Medal 2 should be unlocked
    Assert.IsTrue(result.AwardedMedals.Any(m => m.MedalId == 3 && !m.Locked)); // Medal 3 should be unlocked

    _userMedalRepositoryMock.Verify(repo => repo.AddBatchUserMedalsAsync(It.Is<List<UserMedal>>(list => list.Count == 3)), Times.Once); // ✅ Ensure all 3 medals are saved
}


    [Test]
    public async Task AwardUserdMedalsAsync_ApiReturnsZeroSteps_NoMedalAwarded()
    {
        // Arrange
        string identityId = "test-identity";
        int userId = 1;

        _userRepositoryMock.Setup(repo => repo.GetIdFromIdentityId(identityId)).Returns(userId);
        _medalRepositoryMock.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(new List<Medal>());
        _oAuthServiceMock.Setup(service => service.GetAccessToken(identityId)).ReturnsAsync("mock-access-token");
        _oAuthServiceMock.Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);

        // Act
        var result = await _awardService.AwardUserdMedalsAsync(identityId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result.AwardedMedals);
    }

[Test]
public async Task AwardUserdMedalsAsync_StepsBelowThreshold_MedalLocked()
{
    // Arrange
    string identityId = "test-identity";
    int userId = 1;
    int steps = 4000;

    var medals = new List<Medal>
    {
        new Medal { MedalId = 1, StepThreshold = 10000, Name = "10K Medal", Image = "10k.png" }
    };

    _userRepositoryMock.Setup(repo => repo.GetIdFromIdentityId(identityId)).Returns(userId);
    _medalRepositoryMock.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
    _userMedalRepositoryMock.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(userId)).ReturnsAsync(new List<UserMedal>()); // ✅ Fix: Return an empty list instead of null
    _oAuthServiceMock.Setup(service => service.GetAccessToken(identityId)).ReturnsAsync("mock-access-token");
    _oAuthServiceMock.Setup(service => service.GetUserSteps(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(steps);

    // Act
    var result = await _awardService.AwardUserdMedalsAsync(identityId);

    // Assert
    Assert.AreEqual(1, result.AwardedMedals.Count);
    Assert.IsTrue(result.AwardedMedals[0].Locked); // Medal should be locked since steps are below the threshold
}

}
