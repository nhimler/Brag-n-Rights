using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.Models;


namespace GymBro_App.DAL.Concrete;

public class UserRepository : Repository<User>, IUserRepository
{
    private DbSet<Models.User> _users;

    public UserRepository(GymBroDbContext context) : base(context)
    {
        _users = context.Users;
    }

    public User GetUserByIdentityUserId(string identityId)
    {
        return _users.FirstOrDefault(u => u.IdentityUserId == identityId) ?? new User();
    }

    public List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId)
    {
        return _users.FirstOrDefault(u => u.IdentityUserId == identityId)?.WorkoutPlans
                     .ToList() ?? new List<WorkoutPlan>();
    }

    // Bool types are not supported by SQL. Treat isCompleted as a boolean value (0 or 1) to see only workouts that are complete/incomplete.
    public List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId, int isCompleted)
    {
        return _users.FirstOrDefault(u => u.IdentityUserId == identityId)?.WorkoutPlans
                     .Where(wp => wp.IsCompleted == isCompleted).ToList() ?? new List<WorkoutPlan>();
    }
}