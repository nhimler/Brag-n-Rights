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
    var userMedalsToday = await _userMedalRepository.GetUserMedalsEarnedTodayAsync(userId);
    var today = DateOnly.FromDateTime(DateTime.Now);

    var userSteps = await _biometricDatumRepository.GetUserStepsAsync(userId) ?? 0;  // Handle nullable case

    var awardedMedals = new List<AwardMedalDetails>();

    foreach (var medal in medals)
    {
        bool alreadyEarnedToday = userMedalsToday
            .Any(um => um.MedalId == medal.MedalId && um.EarnedDate == today);

        int stepsRemaining = medal.StepThreshold - userSteps;

        // Create an AwardMedalDetails object
        var awardMedalDetails = new AwardMedalDetails
        {
            MedalId = medal.MedalId,
            MedalName = medal.Name,
            MedalImage = medal.Image,
            AwardedDate = userSteps >= medal.StepThreshold ? DateTime.Now : (DateTime?)null, // Set awarded date if steps threshold met
            StepThreshold = medal.StepThreshold,
            ProgressPercentage = userSteps >= medal.StepThreshold ? 100 : (double)userSteps / medal.StepThreshold * 100,
            Locked = userSteps < medal.StepThreshold,  // If steps are less than the threshold, it's locked
            StepsRemaining = stepsRemaining  // Calculate steps remaining
        };

        // If the user hasn't earned this medal yet today and meets the step threshold
        if (userSteps >= medal.StepThreshold && !alreadyEarnedToday)
        {
            var userMedal = new UserMedal
            {
                UserId = userId,
                MedalId = medal.MedalId,
                EarnedDate = today  // Record the date the medal was awarded
            };

            // Save to the database
            await _userMedalRepository.AddUserMedalAsync(userMedal);
        }

        awardedMedals.Add(awardMedalDetails);
    }

    return new AwardMedal
    {
        UserId = userId,
        AwardedMedals = awardedMedals
    };
}


    }
}
