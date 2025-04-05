using GymBro_App.Models;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract;
using Azure.Core;

namespace GymBro_App.Services
{
    public class CompetitionService : ICompetitionService{

        private readonly GymBroDbContext _context;

        public CompetitionService(GymBroDbContext context)
        {
            _context = context;
        }   
        
        public async Task<StepCompetitionEntity> CreateCompetitionAsync(string creatorIdentityId)
        {
            var competition = new StepCompetitionEntity
            {
                CreatorIdentityId = creatorIdentityId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            _context.StepCompetitions.Add(competition);
            await _context.SaveChangesAsync();

            return competition;
        }
    }
}