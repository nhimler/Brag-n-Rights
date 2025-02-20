using GymBro_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GymBro_App.DAL.Abstract;
public interface IBiometricDatumRepository : IRepository<BiometricDatum>
{
    Task<int?> GetUserStepsAsync(int userId);

    Task<IEnumerable<BiometricDatum>> GetUserBiometricDataAsync(int userId);
}