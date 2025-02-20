using GymBro_App.DTO;
using System.Text.Json;

namespace GymBro_App.Services;

//TODO: Conform these classes to the api's actual structure
class ExtFoodDTO
{
    public string food_id { get; set; } = "";
    public string food_name { get; set; } = "";
    public string food_description { get; set; } = "";
    public string brand_name { get; set; } = "";
}

class GetFoodResponse
{
    public ExtFoodDTO food { get; set; } = new ExtFoodDTO();
}

class FoodSearch
{
    public List<ExtFoodDTO> food { get; set; } = new List<ExtFoodDTO>();
}

class FoodSearchResponse
{
    public FoodSearch foods { get; set; } = new FoodSearch();
}

public class FoodService : IFoodService
{
    readonly HttpClient _httpClient;
    readonly ILogger<FoodService> _logger;

    public FoodService(HttpClient httpClient, ILogger<FoodService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<FoodDTO>> GetFoodsAsync(string query)
    {
        var response = await _httpClient.PostAsync("", new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string?, string?>("method", "foods.search"),
            new KeyValuePair<string?, string?>("search_expression", query),
            new KeyValuePair<string?, string?>("format", "json")
        }));
        if (response.IsSuccessStatusCode)
        {
            var searchResults = await JsonSerializer.DeserializeAsync<FoodSearchResponse>(await response.Content.ReadAsStreamAsync());
            searchResults = searchResults ?? new FoodSearchResponse();
            return searchResults.foods.food.Select(f => new FoodDTO
            {
                FoodId = f.food_id,
                FoodName = f.food_name,
                FoodDescription = f.food_description
            }).ToList();
        }
        return new List<FoodDTO>();
    }

    public async Task<FoodDTO> GetFoodAsync(string id)
    {
        var response = await _httpClient.PostAsync("", new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string?, string?>("method", "food.get.v4"),
            new KeyValuePair<string?, string?>("food_id", id),
            new KeyValuePair<string?, string?>("format", "json")
        }));
        var rawResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Status: {response.StatusCode}");
        // Console.WriteLine($"Raw Response: {rawResponse}");
        if (response.IsSuccessStatusCode)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var foods = await JsonSerializer.DeserializeAsync<GetFoodResponse>(await response.Content.ReadAsStreamAsync(), options);
            foods = foods ?? new GetFoodResponse();
            var food = foods.food;

            return new FoodDTO
            {
                FoodName = food.food_name,
                FoodId = food.food_id,
                BrandName = food.brand_name
            };
        }
        return new FoodDTO();
    }
}