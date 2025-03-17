using GymBro_App.Models;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract;
using Azure.Core;

namespace GymBro_App.Services
{
    public class AwardMedalService : IAwardMedalService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMedalRepository _medalRepository;
        private readonly IUserMedalRepository _userMedalRepository;
        private readonly IBiometricDatumRepository _biometricDatumRepository;  // Declare the field
        private readonly IOAuthService _oAuthService;

        public AwardMedalService(IUserRepository userRepository, IMedalRepository medalRepository, 
            IUserMedalRepository userMedalRepository, IBiometricDatumRepository biometricDatumRepository
            , IOAuthService oAuthService)
        {
            _userRepository = userRepository;
            _medalRepository = medalRepository;
            _userMedalRepository = userMedalRepository;
            _biometricDatumRepository = biometricDatumRepository;  // Assign to field
            _oAuthService = oAuthService;
        }

        public async Task<AwardMedal> AwardUserdMedalsAsync(string identityId)
        {
            await SaveActivityData(identityId);
            var userId = _userRepository.GetIdFromIdentityId(identityId);

            var medals = await _medalRepository.GetAllMedalsAsync();// get all medals
            var userMedalsToday = await _userMedalRepository.GetUserMedalsEarnedTodayAsync(userId);// get all medals earned by the user today

            var pacificTime = GetPacificTime();  // Use the helper method to get Pacific Time
            var today = DateOnly.FromDateTime(pacificTime.Date);  // Extract date from Pacific Time

            var userSteps = await _biometricDatumRepository.GetUserStepsAsync(userId) ?? 0;  // get total user steps for today
            var awardedMedals = new List<AwardMedalDetails>();
            var newUserMedals = new List<UserMedal>();

            foreach (var medal in medals)
            {
                bool alreadyEarnedToday = userMedalsToday.Any(um => um.MedalId == medal.MedalId);
                int stepsRemaining = Math.Max(0, medal.StepThreshold - userSteps);

                awardedMedals.Add(new AwardMedalDetails
                {
                    MedalId = medal.MedalId,
                    MedalName = medal.Name,
                    MedalImage = medal.Image,
                    MedalDescription = medal.Description ?? "No description available",
                    StepThreshold = medal.StepThreshold,
                    Locked = userSteps < medal.StepThreshold,
                    StepsRemaining = stepsRemaining
                });

                if (userSteps >= medal.StepThreshold && !alreadyEarnedToday)
                {
                    newUserMedals.Add(new UserMedal
                    {
                        UserId = userId,
                        MedalId = medal.MedalId,
                        EarnedDate = today
                    });
                }
            }

            // Batch insert newly earned medals
            if (newUserMedals.Any())
            {
                await _userMedalRepository.AddBatchUserMedalsAsync(newUserMedals);
            }

            return new AwardMedal
            {
                UserId = userId,
                AwardedMedals = awardedMedals
            };
        } 
        public async Task SaveActivityData(string identityId)
        {
            var pacificTime = GetPacificTime();  // Use the helper method to get Pacific Time
            var today = DateOnly.FromDateTime(pacificTime.Date); 

            var Token = await _oAuthService.GetAccessToken(identityId);
            var steps = await _oAuthService.GetUserSteps(Token, today.ToString("yyyy-MM-dd"));

            try
            {
                if (steps != null)
                {
                    var userId = _userRepository.GetIdFromIdentityId(identityId);

                    if (userId == null)
                    {
                        Console.WriteLine("Error: User ID not found.");
                        return;
                    }

                    var biometricData = new BiometricDatum
                    {
                        UserId = userId,
                        Steps = steps,
                       LastUpdated = GetPacificTime() 
                    };

                    await _biometricDatumRepository.AddAsync(biometricData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving activity data: {ex.Message}");
            }

        }

        private DateTime GetPacificTime()
        {
            var pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, pacificTimeZone);
        }


    }
}
