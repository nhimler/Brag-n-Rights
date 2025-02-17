using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GymBro_App.DAL.Concrete
{
    public class UserMedalRepository : IUserMedalRepository
    {
        private readonly GymBroDbContext _context;

        public UserMedalRepository(GymBroDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMedal>> GetAllUserMedalsAsync(int userId)
        {
            return await _context.UserMedals
                                .Where(um => um.UserId == userId)
                                .ToListAsync();
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
            var today = DateOnly.FromDateTime(DateTime.Now);  // Get today's date
            return await _context.UserMedals
                                .Where(um => um.UserId == userId && um.EarnedDate == today)
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