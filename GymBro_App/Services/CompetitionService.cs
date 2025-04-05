using GymBro_App.Models;
using TimeZoneConverter;
using GymBro_App.DAL.Abstract;


namespace GymBro_App.Services
{
    public class CompetitionService : ICompetitionService{

        private readonly GymBroDbContext _context;
        private readonly IStepCompetitionRepository _competitionRepository;

        public CompetitionService(GymBroDbContext context, IStepCompetitionRepository competitionRepository)
        {
            _competitionRepository = competitionRepository;
            _context = context;
        }
        public async Task<StepCompetitionEntity> CreateCompetitionAsync(string creatorId)
        {
            // Get Pacific Time zone
            TimeZoneInfo pacificZone = TZConvert.GetTimeZoneInfo("Pacific Standard Time");

            // Get current time in Pacific Time
            DateTime pacificNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone);

            // Create the competition
            var competition = new StepCompetitionEntity
            {
                CreatorIdentityId = creatorId,
                StartDate = pacificNow,
                EndDate = pacificNow.AddDays(7),
                IsActive = true
            };

            // Add the competition to the database
            await _competitionRepository.AddAsync(competition);

            return competition;
        }
    }
}