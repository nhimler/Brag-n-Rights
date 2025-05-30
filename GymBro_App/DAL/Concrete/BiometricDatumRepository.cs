using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymBro_App.DAL.Concrete
{
    public class BiometricDatumRepository : IBiometricDatumRepository, IRepository<BiometricDatum>
    {
        private readonly GymBroDbContext _context;

        public BiometricDatumRepository(GymBroDbContext context)
        {
            _context = context;
        }

        public async Task<BiometricDatum> AddAsync(BiometricDatum entity)
        {
            await _context.BiometricData.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Fetch the latest step count for a given user
        public async Task<int?> GetUserStepsAsync(int userId)
        {
            // Get today's date in Pacific Time Zone (ignoring time)
            var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var todayPacific = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pacificZone).Date; // Get date part only

            // Ensure we are considering only steps from today
            var startOfToday = todayPacific;  // Start of today at 00:00:00 AM in Pacific Time
            var endOfToday = todayPacific.AddDays(1).AddTicks(-1);  // End of today at 23:59:59.9999999

            // Fetch the latest step data for today (if any exists)
            var latestStep = await _context.BiometricData
                .Where(b => b.UserId == userId && b.LastUpdated.HasValue &&
                            b.LastUpdated.Value >= startOfToday && b.LastUpdated.Value <= endOfToday)  // Only steps from today
                .OrderByDescending(b => b.LastUpdated)  // Order by the latest LastUpdated date
                .Select(b => b.Steps)
                .FirstOrDefaultAsync();

            return latestStep;
        }

        // Find a BiometricDatum by ID
        public BiometricDatum FindById(int id)
        {
            return _context.BiometricData.Find(id);
        }

        // Check if a BiometricDatum exists by ID
        public bool Exists(int id)
        {
            return _context.BiometricData.Any(b => b.BiometricId == id); // Corrected to BiometricId
        }

        // Get all biometric data
        public IQueryable<BiometricDatum> GetAll()
        {
            return _context.BiometricData.AsQueryable();
        }

        // Get all biometric data by a specific condition
        public IQueryable<BiometricDatum> GetAll(Expression<Func<BiometricDatum, bool>> predicate)
        {
            return _context.BiometricData.Where(predicate).AsQueryable();
        }

        // Get all biometric data including related entities
        public IQueryable<BiometricDatum> GetAll(params Expression<Func<BiometricDatum, object>>[] includeProperties)
        {
            IQueryable<BiometricDatum> query = _context.BiometricData;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        // Delete a BiometricDatum by entity
        public void Delete(BiometricDatum entity)
        {
            _context.BiometricData.Remove(entity);  // Use the correct entity and property name (BiometricId)
        }

        // Delete a BiometricDatum by ID
        public void DeleteById(int id)
        {
            var entity = _context.BiometricData.Find(id); // Use BiometricId here
            if (entity != null)
            {
                _context.BiometricData.Remove(entity);  // Remove the entity by its BiometricId
            }
        }

        // Add or update a BiometricDatum
        public BiometricDatum AddOrUpdate(BiometricDatum entity)
        {
            if (_context.BiometricData.Any(b => b.BiometricId == entity.BiometricId))
            {
                _context.BiometricData.Update(entity);
            }
            else
            {
                _context.BiometricData.Add(entity);
            }

            _context.SaveChanges(); // Persist the changes to the database
            return entity;
        }
        public async Task<BiometricDatum> AddOrUpdateAsync(BiometricDatum entity)
        {
            if (await _context.BiometricData.AnyAsync(b => b.BiometricId == entity.BiometricId))
            {
                _context.BiometricData.Update(entity);
            }
            else
            {
                await _context.BiometricData.AddAsync(entity);
            }

            await _context.SaveChangesAsync(); // Persist the changes to the database
            return entity;
        }

        // Get all biometric data for a user
        public async Task<IEnumerable<BiometricDatum>> GetUserBiometricDataAsync(int userId)
        {
            return await _context.BiometricData
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        // Get the latest biometric data for a user
        public async Task<BiometricDatum> GetLatestBiometricDataAsync(int userId)
        {
            return await _context.BiometricData
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.LastUpdated)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException("No biometric data found for the user.");
        }

        public async Task<int> GetUserTotalStepsByDayAsync(
            int userId,
            DateTime startDate,
            DateTime endDate)
        {
            var dailyMaxSteps = _context.BiometricData
                .Where(b =>
                    b.UserId == userId &&
                    b.LastUpdated >= startDate &&   // Filters both date and time
                    b.LastUpdated <= endDate)       // Filters both date and time
                .GroupBy(b => b.LastUpdated!.Value.Date)  // Group by date, ignoring the time
                .Select(g => g.Max(b => b.Steps));  // Get the maximum steps for each day

            // Sum the total steps
            return await dailyMaxSteps.SumAsync() ?? 0;
        }

        public async Task<int> LatestStepsBeforeCompAsync(int userId, DateTime startDate)
        {
            var latestSteps = await _context.BiometricData
                .Where(b => b.UserId == userId && b.LastUpdated < startDate) // Filter by user and before startDate
                .OrderByDescending(b => b.LastUpdated) // Order by LastUpdated descending to get the latest
                .Select(b => b.Steps) // Select the Steps value
                .FirstOrDefaultAsync(); // Get the latest (most recent) Steps

            // If no records found, return 0
            return latestSteps ?? 0;
        }
    }
}
