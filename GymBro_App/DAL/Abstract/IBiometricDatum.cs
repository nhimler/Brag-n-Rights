using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IBiometricDatumRepository : IRepository<BiometricDatum>
{
    Task<int?> GetUserStepsAsync(int userId);

    Task<IEnumerable<BiometricDatum>> GetUserBiometricDataAsync(int userId);

    Task<BiometricDatum> AddAsync(BiometricDatum entity);
}