
using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IMealRepository : IRepository<Meal>
{
    void Add(Meal m);
}
