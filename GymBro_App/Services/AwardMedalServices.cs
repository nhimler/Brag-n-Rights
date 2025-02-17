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
            var medals = await _medalRepository.GetAllMedalsAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);

            // 🟢 Get all medals user has already earned today
            var userMedalsToday = await _userMedalRepository.GetUserMedalsEarnedTodayAsync(userId);
            var userSteps = await _biometricDatumRepository.GetUserStepsAsync(userId);

            var awardedMedals = new List<AwardMedalDetails>();

            foreach (var medal in medals)
            {
                // Check if the user already earned this medal today
                var existingMedal = userMedalsToday.FirstOrDefault(um => um.MedalId == medal.MedalId);
                
                if (userSteps >= medal.StepThreshold)
                {
                    if (existingMedal == null)
                    {
                        // 🟢 User has not earned this medal today, so we award it now
                        var userMedal = new UserMedal
                        {
                            UserId = userId,
                            MedalId = medal.MedalId,
                            EarnedDate = today
                        };

                        await _userMedalRepository.AddUserMedalAsync(userMedal);
                        
                        // Add to today's medals list
                        awardedMedals.Add(new AwardMedalDetails
                        {
                            MedalId = medal.MedalId,
                            MedalName = medal.Name,
                            MedalImage = medal.Image,
                            AwardedDate = DateTime.Now,
                            StepThreshold = medal.StepThreshold,
                            ProgressPercentage = (double)userSteps / medal.StepThreshold * 100
                        });
                    }
                    else
                    {
                        // 🟢 User already earned this medal today, but still needs to see it
                        awardedMedals.Add(new AwardMedalDetails
                        {
                            MedalId = medal.MedalId,
                            MedalName = medal.Name,
                            MedalImage = medal.Image,
                            AwardedDate = existingMedal.EarnedDate.ToDateTime(TimeOnly.MinValue),
                            StepThreshold = medal.StepThreshold,
                            ProgressPercentage = (double)userSteps / medal.StepThreshold * 100
                        });
                    }
                }
            }

            return new AwardMedal
            {
                UserId = userId,
                AwardedMedals = awardedMedals // Always return medals earned today
            };
        }
    }
}
