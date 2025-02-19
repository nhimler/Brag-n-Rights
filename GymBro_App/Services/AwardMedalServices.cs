using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBro_App.Models;
using GymBro_App.Models.DTOs;
using GymBro_App.DAL.Abstract; 

namespace GymBro_App.Services
{
    public class AwardMedalService : IAwardMedalService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMedalRepository _medalRepository;
        private readonly IUserMedalRepository _userMedalRepository;
        private readonly IBiometricDatumRepository _biometricDatumRepository;  // Declare the field

        public AwardMedalService(IUserRepository userRepository, IMedalRepository medalRepository, 
            IUserMedalRepository userMedalRepository, IBiometricDatumRepository biometricDatumRepository)
        {
            _userRepository = userRepository;
            _medalRepository = medalRepository;
            _userMedalRepository = userMedalRepository;
            _biometricDatumRepository = biometricDatumRepository;  // Assign to field
        }

        public async Task<AwardMedal> AwardUserdMedalsAsync(string identityId)
        {
            var userId = _userRepository.GetIdFromIdentityId(identityId);

            var medals = await _medalRepository.GetAllMedalsAsync();// get all medals
            var userMedalsToday = await _userMedalRepository.GetUserMedalsEarnedTodayAsync(userId);// get all medals earned by the user today
            var today = DateOnly.FromDateTime(DateTime.Now);

            var userSteps = await _biometricDatumRepository.GetUserStepsAsync(userId) ?? 0;  // get total user steps for today
            var awardedMedals = new List<AwardMedalDetails>();
            var newUserMedals = new List<UserMedal>();

            foreach (var medal in medals)
            {
                bool alreadyEarnedToday = userMedalsToday.Any(um => um.MedalId == medal.MedalId);
                int stepsRemaining = medal.StepThreshold - userSteps;

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



    }
}
