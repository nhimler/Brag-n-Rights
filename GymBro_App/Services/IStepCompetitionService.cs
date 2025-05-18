using GymBro_App.Models;

namespace GymBro_App.Services
{
    public interface IStepCompetitionService
    {
        // Method to update competition participants step count
        Task UpdateCompetitionParticipantStepCountAsync(StepCompetitionParticipant participant, DateTime startDate, DateTime endDate, int competitionID);
    }
}