using NUnit.Framework;
using Moq;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GymBro_App.DAL.Tests
{
    [TestFixture]
    public class StepCompetitionRepositoryTests
    {
        private Mock<GymBroDbContext>         _mockCtx;
        private Mock<DbSet<StepCompetition>> _mockSet;
        private List<StepCompetition>         _data;
        private StepCompetitionRepository    _repo;

        [SetUp]
        public void Setup()
        {
            _data = new List<StepCompetition>();

            _mockSet = TestHelpers.CreateMockSet(_data);

            _mockCtx = new Mock<GymBroDbContext>( /* options if your ctor needs them */ );
            _mockCtx.Setup(c => c.StepCompetitions).Returns(_mockSet.Object);
            _mockCtx
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

            _repo = new StepCompetitionRepository(_mockCtx.Object);
        }

        [Test]
        public async Task AddAsync_ShouldAddCompetition()
        {
            // Arrange
            var competition = new StepCompetition
            {
                CompetitionID = 1,
                CreatorIdentityId = "324",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7)
            };

            // Act
            await _repo.AddAsync(competition);

            // Assert
            _mockSet.Verify(m => m.AddAsync(It.IsAny<StepCompetition>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockCtx.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [TearDown]
        public void Teardown()
        {
            // Clean up the mock context and set
            _mockSet = null;
            _mockCtx = null;
            _data = null;
            _repo = null;          
        }
    }
}
