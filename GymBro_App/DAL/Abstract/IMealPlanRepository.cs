using GymBro_App.Models;

namespace GymBro_App.DAL.Abstract;
public interface IMealPlanRepository : IRepository<MealPlan>
{
    void Add(MealPlan mp);
    MealPlan? GetFirstMealPlanForUser(int userId);
    bool HasMeals(int mealPlanId);
    Meal? FirstMeal(int mealPlanId);
}
