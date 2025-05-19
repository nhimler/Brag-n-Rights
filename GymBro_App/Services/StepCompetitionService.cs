using GymBro_App.DAL.Abstract;
using GymBro_App.Models;


namespace GymBro_App.Services
{
    public class StepCompetitionService : IStepCompetitionService
    {

        private readonly IAwardMedalService _awardMedalService;
        private readonly IUserRepository _userRepository;

        private readonly IStepCompetitionRepository _stepCompetitionRepository;
        
        private readonly IBiometricDatumRepository _biometricDatumRepository;
        public StepCompetitionService(IAwardMedalService awardMedalService, IUserRepository userRepository, IBiometricDatumRepository biometricDatumRepository 
        ,IStepCompetitionRepository stepCompetitionRepository)
        {
            _awardMedalService = awardMedalService;
            _userRepository = userRepository;
            _biometricDatumRepository = biometricDatumRepository;
            _stepCompetitionRepository = stepCompetitionRepository;

        }

        public async Task UpdateCompetitionParticipantStepCountAsync(StepCompetitionParticipant participant, DateTime startDate, DateTime endDate, int competitionID)
        {

            await _awardMedalService.SaveActivityData(participant.IdentityId);
            var user = _userRepository.GetUserByIdentityUserId(participant.IdentityId);

            //steps on the start date before start time
            var subtractSteps = await _biometricDatumRepository.LatestStepsBeforeCompAsync(user.UserId, startDate);



            var totalSteps = await _biometricDatumRepository.GetUserTotalStepsByDayAsync(user.UserId, startDate, endDate);

            if (subtractSteps > 0)
            {
                totalSteps -= subtractSteps;
            }

            if (totalSteps != null)
            {
                 await _stepCompetitionRepository.updateParticipantStepCount(participant.IdentityId, totalSteps , competitionID);
            }
            return;
        }
    }
}