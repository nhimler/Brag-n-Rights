using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;

public interface IUserRepository : IRepository<User>
{
    User GetUserByIdentityUserId(string identityId);

    List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId);
}