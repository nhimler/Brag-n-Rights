using GymBro_App.Models;


namespace GymBro_App.Services
{
    public class StepCompetitionService : IStepCompetitionService
    {

        private readonly IAwardMedalService _awardMedalService;

        public StepCompetitionService(IAwardMedalService awardMedalService)
        {
            _awardMedalService = awardMedalService;
        }

        public async Task UpdateCompetitionParticipantStepCountAsync(StepCompetitionParticipant participant)
        {
            await _awardMedalService.SaveActivityData(participant.IdentityId);

            // Logic to update the participant's step count
            
        }
    }
}