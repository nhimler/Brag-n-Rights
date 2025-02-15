using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;

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
            if(user == null)
            {
                return -1;
            }
            return user.UserId;
        }
    }
}
