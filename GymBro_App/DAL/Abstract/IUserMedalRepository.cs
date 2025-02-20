using GymBro_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GymBro_App.DAL.Abstract
{
    public interface IUserMedalRepository
    {
        Task<IEnumerable<UserMedal>> GetAllUserMedalsAsync(int userId);

        Task AddUserMedalAsync(UserMedal userMedal);

        Task<List<UserMedal>> GetUserMedalsEarnedTodayAsync(int userId);

        Task AddBatchUserMedalsAsync(List<UserMedal> userMedals);
    }
    
}