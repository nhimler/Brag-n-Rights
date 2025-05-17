using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IBiometricDatumRepository : IRepository<BiometricDatum>
{
    Task<int?> GetUserStepsAsync(int userId);

    Task<IEnumerable<BiometricDatum>> GetUserBiometricDataAsync(int userId);

    Task<BiometricDatum> AddAsync(BiometricDatum entity);

    Task<BiometricDatum> GetLatestBiometricDataAsync(int userId);

    Task<int> GetUserTotalStepsByDayAsync(int userId, DateTime startDate, DateTime endDate);   

    Task<int> LatestStepsBeforeCompAsync(int userId, DateTime startDate);
    
}