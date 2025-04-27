using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;
using GymBro_App.Models.DTOs;

namespace Repository_Tests
{
    [TestFixture]
    public class UserMedalRepositoryTests
    {
        private GymBroDbContext _context;
        private UserMedalRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<GymBroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GymBroDbContext(options);

            // Seed a fully valid User (including required fields)
            _context.Users.Add(new User
            {
                UserId               = 1,
                IdentityUserId       = "test-identity",
                FirstName            = "Test",
                LastName             = "User",
                Email                = "test@example.com",   // required
                Password             = "P@ssw0rd!",          // required
                AccountCreationDate  = DateTime.UtcNow
            });

            // Seed one Medal
            _context.Medals.Add(new Medal
            {
                MedalId       = 1,
                Name          = "Test Medal",
                Image         = "test.png",
                StepThreshold = 1000
            });

            _context.SaveChanges();

            _repository = new UserMedalRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllUserMedalsAsync_ReturnsEmpty_When_UserNotFound()
        {
            var result = await _repository.GetAllUserMedalsAsync("non-existent");

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllUserMedalsAsync_ReturnsMedals_When_UserHasMedals()
        {
            // Arrange: add a UserMedal earned yesterday
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

            _context.UserMedals.Add(new UserMedal
            {
                UserMedalId = 1,
                UserId      = 1,
                MedalId     = 1,
                EarnedDate  = yesterday
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUserMedalsAsync("test-identity");

            // Assert
            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Test Medal", list[0].MedalName);
            Assert.AreEqual("test.png", list[0].MedalImage);
        }
    }
}
