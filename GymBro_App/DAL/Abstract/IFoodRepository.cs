
using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IFoodRepository : IRepository<Food>
{
    void DeleteInMeal(int mealId);
    void Add(Food f);
    void Update(Food f);
}
