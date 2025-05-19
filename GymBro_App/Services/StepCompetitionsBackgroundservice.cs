using GymBro_App.DAL.Abstract;

namespace GymBro_App.Services
{
    public class StepCompetitionsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StepCompetitionsBackgroundService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);

        public StepCompetitionsBackgroundService(IServiceProvider serviceProvider, ILogger<StepCompetitionsBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var stepCompetitionRepository = scope.ServiceProvider.GetRequiredService<IStepCompetitionRepository>();
                        var activeCompetitions = await stepCompetitionRepository.GetActiveCompetitionsAsync();

                        var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                        var pacificNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone);

                        foreach (var competition in activeCompetitions)
                        {

                            if (competition.EndDate <= pacificNow)
                            {
                                // Update the competition status to ended
                                competition.IsActive = false;
                                await  stepCompetitionRepository.UpdateAsync(competition);
                                //set all participants to inactive
                                await  stepCompetitionRepository.SetIsActiveToFalseForAllParticipantsAsync(competition.CompetitionID); 

                            }else
                            {
                                foreach (var user in competition.Participants)
                                {
                                    if (user.IsActive == true)
                                    {                                        

                                        // Logic to update the user's step count
                                        var stepCompetitionService = scope.ServiceProvider.GetRequiredService<IStepCompetitionService>();
                                        await stepCompetitionService.UpdateCompetitionParticipantStepCountAsync(user, competition.StartDate, competition.EndDate,competition.CompetitionID ); 
                                    }
                                }
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ExecuteAsync method of StepCompetitionsBackgroundService");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
