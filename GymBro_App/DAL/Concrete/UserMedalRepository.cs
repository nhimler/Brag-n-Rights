using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GymBro_App.Models.DTOs;


namespace GymBro_App.DAL.Concrete
{
    public class UserMedalRepository : IUserMedalRepository
    {
        private readonly GymBroDbContext _context;

        public UserMedalRepository(GymBroDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMedalDto>> GetAllUserMedalsAsync(string identityId)
        {
                var user = await _context.Users
                             .FirstOrDefaultAsync(u => u.IdentityUserId == identityId);
           if (user == null)
           {
               return Enumerable.Empty<UserMedalDto>();
           }

            // Convert to Pacific Time
            var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var todayPacific = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone).Date;

            return await _context.UserMedals
                .Where(um => um.UserId == user.UserId && um.EarnedDate < DateOnly.FromDateTime(todayPacific))
                .Include(um => um.Medal) // Include the Medal navigation property
                .OrderByDescending(um => um.EarnedDate)
                .Select(um => new UserMedalDto
                {
                    UserMedalId = um.UserMedalId,
                    EarnedDate = um.EarnedDate,
                    MedalId = um.MedalId,
                    MedalImage = um.Medal.Image, // Assuming ImageUrl is a property in Medal
                    MedalName = um.Medal.Name // Assuming Name is a property in Medal
                })
                .ToListAsync();
        }

        public async Task AddBatchUserMedalsAsync(List<UserMedal> userMedals)
        {
            if (userMedals == null || !userMedals.Any()) return;

            // Assuming _context is your database context (EF Core)
            await _context.UserMedals.AddRangeAsync(userMedals);  // Bulk insert medals
            await _context.SaveChangesAsync();  // Commit to the database
        }

        public async Task AddUserMedalAsync(UserMedal userMedal)
        {
            // Add the new UserMedal to the DbSet
            await _context.UserMedals.AddAsync(userMedal);

            // Save the changes to the database asynchronously
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserMedal>> GetUserMedalsEarnedTodayAsync(int userId)
        {
            // Convert to Pacific Time
            var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var todayPacific = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone).Date;

            // Convert DateTime to DateOnly
            var todayPacificDateOnly = DateOnly.FromDateTime(todayPacific);

            return await _context.UserMedals
                                .Where(um => um.UserId == userId && um.EarnedDate == todayPacificDateOnly)
                                .ToListAsync();
        }



        public UserMedal FindById(int id)
        {
            return _context.UserMedals.Find(id);
        }

        public bool Exists(int id)
        {
            return _context.UserMedals.Any(m => m.UserMedalId == id);
        }

        public IQueryable<UserMedal> GetAll()
        {
            return _context.UserMedals.AsQueryable();
        }

        public IQueryable<UserMedal> GetAll(Expression<Func<UserMedal, bool>> predicate)
        {
            return _context.UserMedals.Where(predicate).AsQueryable();
        }

        public IQueryable<UserMedal> GetAll(params Expression<Func<UserMedal, object>>[] includeProperties)
        {
            IQueryable<UserMedal> query = _context.UserMedals;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public UserMedal AddOrUpdate(UserMedal entity)
        {
            if (_context.UserMedals.Any(m => m.UserMedalId == entity.UserMedalId))
            {
                _context.UserMedals.Update(entity);
            }
            else
            {
                _context.UserMedals.Add(entity);
            }
            return entity;
        }

        public void Delete(UserMedal entity)
        {
            _context.UserMedals.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}