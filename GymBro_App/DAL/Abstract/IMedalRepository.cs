using GymBro_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GymBro_App.DAL.Abstract;
public interface IMedalRepository : IRepository<Medal>
{
    // Get all medals from the database
    Task<IEnumerable<Medal>> GetAllMedalsAsync();
}