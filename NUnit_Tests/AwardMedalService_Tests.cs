using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using GymBro_App.Services;
using GymBro_App.Models;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract;

public class AwardMedalServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMedalRepository> _mockMedalRepo;
    private readonly Mock<IUserMedalRepository> _mockUserMedalRepo;
    private readonly Mock<IBiometricDatumRepository> _mockBiometricDatumRepo;
    private readonly AwardMedalService _service;

    public AwardMedalServiceTests()
    {
        // Create mocks for the dependencies
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMedalRepo = new Mock<IMedalRepository>();
        _mockUserMedalRepo = new Mock<IUserMedalRepository>();
        _mockBiometricDatumRepo = new Mock<IBiometricDatumRepository>();

        // Initialize the service with mocked dependencies
        _service = new AwardMedalService(
            _mockUserRepo.Object,
            _mockMedalRepo.Object,
            _mockUserMedalRepo.Object,
            _mockBiometricDatumRepo.Object
        );
    }

[Fact]
public async Task AwardUserdMedalsAsync_ShouldAwardNewMedals_WhenUserStepsMeetThreshold()
    {
        // Arrange
        var identityId = "user123";
        var userId = 1;
        
        var medals = new List<Medal>
        {
            new Medal { MedalId = 1, Name = "Bronze", StepThreshold = 100, Image = "bronze.png", Description = "Bronze medal" },
            new Medal { MedalId = 2, Name = "Silver", StepThreshold = 200, Image = "silver.png", Description = "Silver medal" }
        };
        
        var userMedalsToday = new List<UserMedal>(); // Assume the user hasn't earned any medals today
        var userSteps = 150; // User has taken 150 steps today

        // Setup the mock repository methods
        _mockUserRepo.Setup(repo => repo.GetIdFromIdentityId(It.IsAny<string>())).Returns(userId);
        _mockMedalRepo.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
        _mockUserMedalRepo.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(It.IsAny<int>())).ReturnsAsync(userMedalsToday);
        _mockBiometricDatumRepo.Setup(repo => repo.GetUserStepsAsync(It.IsAny<int>())).ReturnsAsync(userSteps);

        // Act
        var result = await _service.AwardUserdMedalsAsync(identityId);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(userId, result.UserId);
        Xunit.Assert.Equal(2, result.AwardedMedals.Count); // Should return 2 medals (even if user only earns 1, both will be in the list)

        // Assert the first medal (Bronze) is unlocked
        Xunit.Assert.True(result.AwardedMedals[0].Locked == false);
        Xunit.Assert.Equal(0, result.AwardedMedals[0].StepsRemaining); // 100 threshold - 150 steps = 0 (not negative)

        // Assert that the second medal (Silver) remains locked
        Xunit.Assert.True(result.AwardedMedals[1].Locked == true);
        Xunit.Assert.Equal(50, result.AwardedMedals[1].StepsRemaining); // 200 threshold - 150 steps = 50 (locked is true)

        // Verify that new medals are added to the user's medal list
        _mockUserMedalRepo.Verify(repo => repo.AddBatchUserMedalsAsync(It.IsAny<List<UserMedal>>()), Times.Once);
    }

    [Fact]
    public async Task AwardUserdMedalsAsync_ShouldNotAwardMedals_IfAlreadyEarnedToday()
    {
        // Arrange
        var identityId = "user123";
        var userId = 1;
        
        var medals = new List<Medal>
        {
            new Medal { MedalId = 1, Name = "Bronze", StepThreshold = 100, Image = "bronze.png", Description = "Bronze medal" }
        };
        
        var userMedalsToday = new List<UserMedal> // Assume the user already earned the "Bronze" medal today
        {
            new UserMedal { MedalId = 1, UserId = userId, EarnedDate = DateOnly.FromDateTime(DateTime.Now) }
        };
        
        var userSteps = 150;

        // Setup the mock repository methods
        _mockUserRepo.Setup(repo => repo.GetIdFromIdentityId(It.IsAny<string>())).Returns(userId);
        _mockMedalRepo.Setup(repo => repo.GetAllMedalsAsync()).ReturnsAsync(medals);
        _mockUserMedalRepo.Setup(repo => repo.GetUserMedalsEarnedTodayAsync(It.IsAny<int>())).ReturnsAsync(userMedalsToday);
        _mockBiometricDatumRepo.Setup(repo => repo.GetUserStepsAsync(It.IsAny<int>())).ReturnsAsync(userSteps);

        // Act
        var result = await _service.AwardUserdMedalsAsync(identityId);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Single(result.AwardedMedals); // Only 1 medal, because the "Bronze" medal was already earned today

        // Verify that no new medals are awarded
        _mockUserMedalRepo.Verify(repo => repo.AddBatchUserMedalsAsync(It.IsAny<List<UserMedal>>()), Times.Never);
    }
}
