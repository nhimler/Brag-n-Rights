
using GymBro_App.Models;
using GymBro_App.ViewModels;

namespace GymBro_App.DAL.Abstract;
public interface IGymUserRepository : IRepository<GymUser>
{
    List<GymUser> GetAllGymUsersByUserId(int userId);
    bool IsGymBookmarked(string gymPlaceId, int userId);
    GymUser? GetGymUserByGymIdAndUserId(string gymPlaceId, int userId);
}