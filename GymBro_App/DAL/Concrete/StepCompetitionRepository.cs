using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeZoneConverter;
using GymBro_App.ViewModels;

namespace GymBro_App.DAL.Concrete
{
    public class StepCompetitionRepository : IStepCompetitionRepository
    {
        private readonly GymBroDbContext _context;

        public StepCompetitionRepository(GymBroDbContext context)
        {
            _context = context;
        }

            public async Task AddAsync(StepCompetition competition)
            {
                await _context.StepCompetitions.AddAsync(competition);
                await _context.SaveChangesAsync();
            }
        public StepCompetition AddOrUpdate(StepCompetition entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(StepCompetition entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public StepCompetition FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetition> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetition> GetAll(Expression<Func<StepCompetition, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StepCompetition> GetAll(params Expression<Func<StepCompetition, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> SearchUsersWithTokenAsync(string query, string currentUserIdentityId)
        {
            return await _context.Users
                .Where(u => u.Username != null &&
                            u.Username.Contains(query) &&
                            u.IdentityUserId != currentUserIdentityId && // 👈 EXCLUDE current user
                            _context.Tokens.Any(t => t.UserId == u.UserId))
                .Select(u => u.Username!)
                .ToListAsync();
        }
        public async Task<StepCompetition> CreateCompetitionAsync(string currentUserIdentityId)
        {
            TimeZoneInfo pacificZone = TZConvert.GetTimeZoneInfo("Pacific Standard Time");

            // Get current time in Pacific Time
            DateTime pacificNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone);
            // Create a new StepCompetition object
            var competition = new StepCompetition
            {
                CreatorIdentityId = currentUserIdentityId,
                StartDate = pacificNow,
                EndDate = pacificNow.AddDays(7),
                IsActive = true
            };

            // Add the competition to the database
            await _context.StepCompetitions.AddAsync(competition);
            await _context.SaveChangesAsync();
            return competition;
        }

        public async Task InviteUsersToCompetitionAsync(string currentUserIdentityId, StepCompetition competition, List<string> invitedUsernames)
        {
            // Get the users from the database based on the invited usernames, excluding the current user
            var users = await _context.Users
                .Where(u => invitedUsernames.Contains(u.Username) && u.IdentityUserId != currentUserIdentityId)
                .ToListAsync();

            // Create StepCompetitionParticipants for each user and add them to the competition
            foreach (var user in users)
            {
                var participant = new StepCompetitionParticipant
                {
                    StepCompetitionId = competition.CompetitionID,
                    IdentityId = user.IdentityUserId,
                };
               await _context.StepCompetitionParticipants.AddAsync(participant);
            }

            // Add the current user as a participant
            var currentUserParticipant = new StepCompetitionParticipant
            {
                StepCompetitionId = competition.CompetitionID,
                IdentityId = currentUserIdentityId,  // Add the current user's identity ID
            };
            await _context.StepCompetitionParticipants.AddAsync(currentUserParticipant);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserCompetitionViewModel>> GetCompetitionsForUserAsync(string identityId)
        {
            return await _context.StepCompetitionParticipants
                .Where(p => p.IdentityId == identityId && p.IsActive)
                .Include(p => p.StepCompetition)
                    .ThenInclude(sc => sc.Participants)
                        .ThenInclude(part => part.User)
                .Select(p => new UserCompetitionViewModel
                {
                    CompetitionID = p.StepCompetition.CompetitionID,
                    StartDate = p.StepCompetition.StartDate,
                    EndDate = p.StepCompetition.EndDate,
                    IsActive = p.StepCompetition.IsActive,
                    Participants = p.StepCompetition.Participants
                        .Where(part => part.IsActive)
                        .OrderByDescending(part => part.Steps)
                        .Select(part => new ParticipantViewModel
                        {
                            Username = part.User.Username,
                            Steps = part.Steps
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<UserCompetitionViewModel>> GetPastCompetitionsForUserAsync(string identityId)
        {
            return await _context.StepCompetitionParticipants
                .Where(p => p.IdentityId == identityId && !p.StepCompetition.IsActive)
                .Include(p => p.StepCompetition)
                    .ThenInclude(sc => sc.Participants)
                        .ThenInclude(part => part.User)
                .Select(p => new UserCompetitionViewModel
                {
                    CompetitionID = p.StepCompetition.CompetitionID,
                    StartDate     = p.StepCompetition.StartDate,
                    EndDate       = p.StepCompetition.EndDate,
                    IsActive      = p.StepCompetition.IsActive,
                    Participants  = p.StepCompetition.Participants
                        .Where(part => !part.IsActive)
                        .OrderByDescending(part => part.Steps)
                        .Select(part => new ParticipantViewModel
                        {
                            Username = part.User.Username,
                            Steps    = part.Steps
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        public async Task<bool> LeaveCompetitionAsync(string identityId,int competitionID)
        {
            var participant = await _context.StepCompetitionParticipants
                .FirstOrDefaultAsync(p => p.IdentityId == identityId && p.StepCompetitionId == competitionID);

            if (participant != null)
            {
                participant.IsActive = false; // Mark as inactive instead of deleting
                await _context.SaveChangesAsync();

                return true; // Successfully left the competition
            }
            return false; // Participant not found
        }
    }
}