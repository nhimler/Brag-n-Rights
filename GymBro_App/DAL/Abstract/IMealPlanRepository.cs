using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IMealPlanRepository : IRepository<MealPlan>
{
    void Add(MealPlan mp);
}
