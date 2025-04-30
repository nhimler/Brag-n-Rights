using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;
using GymBro_App.ViewModels;  // Ensure your view models (UserCompetitionViewModel, ParticipantViewModel) are available

namespace Repository_Tests
{
    [TestFixture]
    public class StepCompetitionRepositoryTests
    {
        private GymBroDbContext _context;
        private StepCompetitionRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GymBroDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new GymBroDbContext(options);
            _repo = new StepCompetitionRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetCompetitionsForUserAsync_ReturnsActiveCompetitions()
        {
            // Arrange
            string identityId = "active-user";
            var user = new User
            {
                UserId = 1,
                IdentityUserId = identityId,
                Username = "ActiveUser",
                Email = "invited@example.com",
                Password = "Invite123!"
            };
            _context.Users.Add(user);
            var competition = new StepCompetition
            {
                CompetitionID = 1,
                CreatorIdentityId = identityId,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(6),
                IsActive = true
            };
            _context.StepCompetitions.Add(competition);

            // Add an active participant
            var participant = new StepCompetitionParticipant
            {
                StepCompetitionId = competition.CompetitionID,
                IdentityId = identityId,
                IsActive = true,
                Steps = 1500,
                StepCompetition = competition,
                User = user
            };
            _context.StepCompetitionParticipants.Add(participant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetCompetitionsForUserAsync(identityId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            var comp = result.First();
            Assert.AreEqual(competition.CompetitionID, comp.CompetitionID);
            Assert.IsTrue(comp.IsActive);
            Assert.AreEqual(1, comp.Participants.Count);
            Assert.AreEqual(user.Username, comp.Participants.First().Username);
        }

        [Test]
        public async Task GetPastCompetitionsForUserAsync_ReturnsInactiveCompetitions()
        {
            // Arrange
            string identityId = "past-user";
            var user = new User
            {
                UserId = 2,
                IdentityUserId = identityId,
                Username = "PastUser",
                Email = "invited@example.com",
                Password = "Invite123!"
            };
            _context.Users.Add(user);
            var competition = new StepCompetition
            {
                CompetitionID = 2,
                CreatorIdentityId = identityId,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(-3),
                IsActive = false
            };
            _context.StepCompetitions.Add(competition);

            // Add an inactive participant for that competition
            var participant = new StepCompetitionParticipant
            {
                StepCompetitionId = competition.CompetitionID,
                IdentityId = identityId,
                IsActive = false,
                Steps = 2000,
                StepCompetition = competition,
                User = user
            };
            _context.StepCompetitionParticipants.Add(participant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetPastCompetitionsForUserAsync(identityId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            var comp = result.First();
            Assert.AreEqual(competition.CompetitionID, comp.CompetitionID);
            Assert.IsFalse(comp.IsActive);
            Assert.AreEqual(1, comp.Participants.Count);
            Assert.AreEqual(user.Username, comp.Participants.First().Username);
        }

        [Test]
        public async Task CreateCompetitionAsync_CreatesActiveCompetition()
        {
            // Arrange
            string identityId = "creator-user";

            // Act
            var competition = await _repo.CreateCompetitionAsync(identityId);

            // Assert
            Assert.IsNotNull(competition);
            Assert.AreEqual(identityId, competition.CreatorIdentityId);
            Assert.IsTrue(competition.IsActive);
            // Expect 7 day duration (comparing dates)
            Assert.AreEqual(competition.StartDate.AddDays(7).Date, competition.EndDate.Date);

            // Check it was added in the database
            var competitionFromDb = await _context.StepCompetitions.FindAsync(competition.CompetitionID);
            Assert.IsNotNull(competitionFromDb);
        }

        [Test]
        public async Task InviteUsersToCompetitionAsync_AddsParticipantsIncludingCurrentUser()
        {
            // Arrange
            string currentUserIdentity = "current-user";
            var currentUser = new User { 
                UserId = 10, 
                IdentityUserId = currentUserIdentity, 
                Username = "CurrentUser",
                Email = "invited@example.com",
                Password = "Invite123!" };
            var invitedUser = new User { 
                UserId = 11, 
                IdentityUserId = "invited-user", 
                Username = "InvitedUser",
                Email = "invited@example.com",
                Password = "Invite123!"
                 };
            _context.Users.AddRange(currentUser, invitedUser);
            await _context.SaveChangesAsync();

            var competition = new StepCompetition
            {
                CompetitionID = 100,
                CreatorIdentityId = currentUserIdentity,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };
            _context.StepCompetitions.Add(competition);
            await _context.SaveChangesAsync();

            List<string> invitedNames = new List<string> { "InvitedUser" };

            // Act
            await _repo.InviteUsersToCompetitionAsync(currentUserIdentity, competition, invitedNames);

            // Assert: Expect the current user and the invited user to be added as participants.
            var participants = _context.StepCompetitionParticipants
                .Where(p => p.StepCompetitionId == competition.CompetitionID)
                .ToList();
            Assert.AreEqual(2, participants.Count);
            Assert.IsTrue(participants.Any(p => p.IdentityId == currentUserIdentity));
            Assert.IsTrue(participants.Any(p => p.IdentityId == "invited-user"));
        }

        [Test]
        public async Task LeaveCompetitionAsync_MarksParticipantAsInactive()
        {
            // Arrange
            string identityId = "leave-user";
            var user = new User { 
                UserId = 20, 
                IdentityUserId = identityId, 
                Username = "LeaveUser",
                Email = "invited@example.com",
                Password = "Invite123!" 
                };
            _context.Users.Add(user);
            var competition = new StepCompetition
            {
                CompetitionID = 200,
                CreatorIdentityId = identityId,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(6),
                IsActive = true
            };
            _context.StepCompetitions.Add(competition);
            var participant = new StepCompetitionParticipant
            {
                StepCompetitionId = competition.CompetitionID,
                IdentityId = identityId,
                IsActive = true,
                Steps = 3000,
                StepCompetition = competition,
                User = user
            };
            _context.StepCompetitionParticipants.Add(participant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.LeaveCompetitionAsync(identityId, competition.CompetitionID);

            // Assert
            Assert.IsTrue(result);
            var updatedParticipant = await _context.StepCompetitionParticipants
                .FirstOrDefaultAsync(p => p.IdentityId == identityId && p.StepCompetitionId == competition.CompetitionID);
            Assert.IsNotNull(updatedParticipant);
            Assert.IsFalse(updatedParticipant.IsActive);
        }
    }
}