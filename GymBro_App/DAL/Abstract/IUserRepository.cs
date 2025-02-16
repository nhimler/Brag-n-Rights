
using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IUserRepository : IRepository<User>
{
    int GetIdFromIdentityId(string identityId);
}