using GymBro_App.Models;

namespace GymBro_App.Services;

public interface IFoodService
{
    Task<List<ApiFood>> GetFoodsAsync();
    Task<ApiFood> GetFoodAsync(string id);
}
