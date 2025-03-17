
using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IUserRepository : IRepository<User>
{
    int GetIdFromIdentityId(string identityId);
    User GetUserByIdentityUserId(string identityId);
    List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId);
    List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId, int isCompleted);

    Task<List<string>> GetAllUserIdentityIDAsync();
}