using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;

public interface IUserRepository : IRepository<User>
{
    User GetUserByUserId(int id);

    List<WorkoutPlan> GetWorkoutPlansByUserId(int id);
}