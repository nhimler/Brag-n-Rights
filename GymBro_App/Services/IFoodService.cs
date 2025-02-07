using GymBro_App.Models;

namespace GymBro_App.Services;

public interface IFoodService
{
    Task<List<Food>> GetFoodsAsync();
    Task<Food> GetFoodAsync(int id);
}
