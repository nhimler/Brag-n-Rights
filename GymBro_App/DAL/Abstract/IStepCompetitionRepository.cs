using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace GymBro_App.DAL.Abstract;
public interface IStepCompetitionRepository : IRepository<StepCompetition>
{

    Task AddAsync(StepCompetition competition);
    Task<List<string>> SearchUsersWithTokenAsync(string query, string currentUserIdentityId);
    Task<StepCompetition> CreateCompetitionAsync(string currentUserIdentityId);
    Task InviteUsersToCompetitionAsync(string currentUserIdentityId, StepCompetition competition, List<string> invitedUsernames);

    Task<List<UserCompetitionViewModel>> GetCompetitionsForUserAsync(string identityId);

    Task<List<UserCompetitionViewModel>> GetPastCompetitionsForUserAsync(string identityId, int daysToSkip);

    Task<List<UserCompetitionViewModel>> GetRecentlyEndedCompetitionsForUserAsync(string identityId);

    Task<bool> LeaveCompetitionAsync(string identityId, int competitionID);
    
    Task<List<StepCompetition>> GetActiveCompetitionsAsync();

    Task UpdateAsync(StepCompetition competition);

    Task SetIsActiveToFalseForAllParticipantsAsync(int competitionID);

    Task updateParticipantStepCount(string identityId, int totalSteps, int competitionID);


}