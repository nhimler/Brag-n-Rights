
using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace GymBro_App.DAL.Abstract;
public interface IUserRepository : IRepository<User>
{
    int GetIdFromIdentityId(string identityId);
    User GetUserByIdentityUserId(string identityId);
    List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId);
    List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId, int isCompleted);
    void UpdateUser(string identityId, UserInfoModel userInfo);
    Task<List<string>> GetAllUserIdentityIDAsync();
}