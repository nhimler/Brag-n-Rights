using GymBro_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface IAwardMedalService
    {
        Task<AwardMedal> AwardUserdMedalsAsync(string identityId); // Method name matches the implementation
    }
}
