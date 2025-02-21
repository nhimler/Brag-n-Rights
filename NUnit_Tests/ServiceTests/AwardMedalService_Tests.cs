using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using GymBro_App.Services;
using GymBro_App.Models;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract;

namespace Service_Tests;

[TestFixture]
public class AwardMedalServiceTests
{
    private Mock<IUserRepository> _mockUserRepo;
    private Mock<IMedalRepository> _mockMedalRepo;
    private Mock<IUserMedalRepository> _mockUserMedalRepo;
    private Mock<IBiometricDatumRepository> _mockBiometricDatumRepo;
    private AwardMedalService _service;

    [SetUp]
    public void Setup()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMedalRepo = new Mock<IMedalRepository>();
        _mockUserMedalRepo = new Mock<IUserMedalRepository>();
        _mockBiometricDatumRepo = new Mock<IBiometricDatumRepository>();

        _service = new AwardMedalService(
            _mockUserRepo.Object,
            _mockMedalRepo.Object,
            _mockUserMedalRepo.Object,
            _mockBiometricDatumRepo.Object
        );
    }

    [Test]
    public async Task AwardUserdMedalsAsync_ShouldAwardNewMedals_WhenUserStepsMeetThreshold()
    {
        var identityId = "user123";
        var userId = 1;
        
        var medals = new List<Medal>
        {
            new Medal { MedalId = 1, Name = "Bronze", StepThreshold = 100, Image = "bronze.png", Description = "Bronze medal" },
            new Medal { MedalId = 2, Name = "Silver", StepThreshold = 200, Image = "silver.png", Description = "Silver medal" }
        };
        
        var userMedalsToday = new List<UserMedal>();
        var userSteps = 150;

        _mockUserRepo.Setup(repo => repo.GetIdFromIdentityId(It.IsAny<string>())).Returns(userId);
        _mockMedalRepo.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
        _mockUserMedalRepo.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(It.IsAny<int>())).ReturnsAsync(userMedalsToday);
        _mockBiometricDatumRepo.Setup(repo => repo.GetUserStepsAsync(It.IsAny<int>())).ReturnsAsync(userSteps);

        var result = await _service.AwardUserdMedalsAsync(identityId);

        Assert.NotNull(result);
        Assert.AreEqual(userId, result.UserId);
        Assert.AreEqual(2, result.AwardedMedals.Count);

        Assert.False(result.AwardedMedals[0].Locked);
        Assert.AreEqual(0, result.AwardedMedals[0].StepsRemaining);

        Assert.True(result.AwardedMedals[1].Locked);
        Assert.AreEqual(50, result.AwardedMedals[1].StepsRemaining);

        _mockUserMedalRepo.Verify(repo => repo.AddBatchUserMedalsAsync(It.IsAny<List<UserMedal>>()), Times.Once);
    }

    [Test]
    public async Task AwardUserdMedalsAsync_ShouldNotAwardMedals_IfAlreadyEarnedToday()
    {
        var identityId = "user123";
        var userId = 1;
        
        var medals = new List<Medal>
        {
            new Medal { MedalId = 1, Name = "Bronze", StepThreshold = 100, Image = "bronze.png", Description = "Bronze medal" }
        };
        
        var userMedalsToday = new List<UserMedal>
        {
            new UserMedal { MedalId = 1, UserId = userId, EarnedDate = DateOnly.FromDateTime(DateTime.Now) }
        };
        
        var userSteps = 150;

        _mockUserRepo.Setup(repo => repo.GetIdFromIdentityId(It.IsAny<string>())).Returns(userId);
        _mockMedalRepo.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
        _mockUserMedalRepo.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(It.IsAny<int>())).ReturnsAsync(userMedalsToday);
        _mockBiometricDatumRepo.Setup(repo => repo.GetUserStepsAsync(It.IsAny<int>())).ReturnsAsync(userSteps);

        var result = await _service.AwardUserdMedalsAsync(identityId);

        Assert.NotNull(result);
        Assert.AreEqual(1, result.AwardedMedals.Count);

        _mockUserMedalRepo.Verify(repo => repo.AddBatchUserMedalsAsync(It.IsAny<List<UserMedal>>()), Times.Never);
    }
}