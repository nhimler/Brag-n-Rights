using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;
using GymBro_App.Models;

namespace GymBro_App.DAL.Concrete
{
    public class GymUserRepository : Repository<GymUser>, IGymUserRepository
    {
        private readonly DbSet<GymUser> _gymUser;

        public GymUserRepository(GymBroDbContext context) : base(context)
        {
            _gymUser = context.GymUsers;
        }

        public List<GymUser> GetAllGymUsersByUserId(int userId)
        {
            return _gymUser.Where(u => u.UserId == userId).ToList();
        }

        public bool IsGymBookmarked(string gymUserId, int userId)
        {
            return _gymUser.Any(u => u.ApiGymId == gymUserId && u.UserId == userId);
        }

        public GymUser? GetGymUserByGymIdAndUserId(string gymPlaceId, int userId)
        {
            return _gymUser.FirstOrDefault(u => u.ApiGymId == gymPlaceId && u.UserId == userId);
        }
    }
}