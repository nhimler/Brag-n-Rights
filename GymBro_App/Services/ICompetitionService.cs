using GymBro_App.Models;

namespace GymBro_App.Services
{
    public interface ICompetitionService
    {
            Task<StepCompetitionEntity> CreateCompetitionAsync(string creatorIdentityId);

    }
}