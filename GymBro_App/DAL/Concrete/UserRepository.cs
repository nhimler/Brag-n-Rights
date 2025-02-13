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
        return _users.FirstOrDefault(u => u.IdentityUserId == identityId);
    }

    public List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId)
    {
        return _users.FirstOrDefault(u => u.IdentityUserId == identityId)?.WorkoutPlans.ToList();
    }
}