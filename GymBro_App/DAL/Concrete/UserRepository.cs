using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace GymBro_App.DAL.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository

    {
        private readonly DbSet<User> _user;
        private readonly GymBroDbContext _context;

        public UserRepository(GymBroDbContext context) : base(context)
        {
            _user = context.Users;
            _context = context;
        }

        public int GetIdFromIdentityId(string identityId)
        {
            User? user = GetAll().Where(u => u.IdentityUserId == identityId).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            return user.UserId;
        }

        public User GetUserByIdentityUserId(string identityId)
        {
            return _user.FirstOrDefault(u => u.IdentityUserId == identityId) ?? new User();
        }

        public List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId)
        {
            return _user.FirstOrDefault(u => u.IdentityUserId == identityId)?.WorkoutPlans
                         .ToList() ?? new List<WorkoutPlan>();
        }

        // Bool types are not supported by SQL. Treat isCompleted as a boolean value (0 or 1) to see only workouts that are complete/incomplete.
        public List<WorkoutPlan> GetWorkoutPlansByIdentityUserId(string identityId, int isCompleted)
        {
            return _user.FirstOrDefault(u => u.IdentityUserId == identityId)?.WorkoutPlans
                         .Where(wp => wp.IsCompleted == isCompleted).ToList() ?? new List<WorkoutPlan>();
        }
        public async Task<List<string>> GetAllUserIdentityIDAsync()
        {
            return await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.IdentityUserId)) // Ensure IdentityUserId is not null or empty
                .Select(u => u.IdentityUserId!)
                .ToListAsync();
        }

        public void UpdateUser(string identityId, UserInfoModel userInfo)
        {
            User? user = _user.FirstOrDefault(u => u.IdentityUserId == identityId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }
            else if (userInfo.Username != user.Username && _user.Any(u => u.Username == userInfo.Username))
            {
                Console.WriteLine("Username already taken.");
                return;
            }
            else if (userInfo.Email != user.Email && _user.Any(u => u.Email == userInfo.Email))
            {
                Console.WriteLine("Email already taken.");
                return;
            }
            else
            {
                user.FirstName = userInfo.FirstName;
                user.LastName = userInfo.LastName;
                user.Username = userInfo.Username;
                user.Age = userInfo.Age;
                user.Gender = userInfo.Gender;
                user.Weight = userInfo.Weight;
                user.Height = userInfo.Height;
                user.FitnessLevel = userInfo.FitnessLevel;
                user.Fitnessgoals = userInfo.Fitnessgoals;
                user.PreferredWorkoutTime = userInfo.PreferredWorkoutTime;
                user.Email = userInfo.Email;
                _context.Users.Update(user);
                _context.SaveChanges();
                Console.WriteLine("User updated successfully.");
                Console.WriteLine(user.FirstName);
            }
        }
    }
}
