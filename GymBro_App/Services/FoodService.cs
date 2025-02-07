using GymBro_App.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GymBro_App.Services;

class ApiFood
{

}

class FoodSearch
{

}

public class FoodService
{
    readonly HttpClient _httpClient;
    readonly ILogger<FoodService> _logger;

    public FoodService(HttpClient httpClient, ILogger<FoodService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Food>> GetFoodsAsync()
    {
        var response = await _httpClient.GetAsync("api/foods");
        if (response.IsSuccessStatusCode)
        {
            var foods = await JsonSerializer.DeserializeAsync<List<ApiFood>>(await response.Content.ReadAsStreamAsync());
            foods = foods ?? new List<ApiFood>();
            return foods.Select(f => new Food
            {
            }).ToList();
        }
        return null;
    }

    public async Task<Food> GetFoodAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/foods/{id}");
        if (response.IsSuccessStatusCode)
        {
            var food = await JsonSerializer.DeserializeAsync<ApiFood>(await response.Content.ReadAsStreamAsync());
            return new Food
            {
            };
        }
        return null;
    }
}
