using GymBro_App.DTO;

namespace GymBro_App.Services;

public interface IFoodService
{
    Task<List<FoodDTO>> GetFoodsAsync(string query);
    Task<FoodDTO?> GetFoodAsync(string id);
}
