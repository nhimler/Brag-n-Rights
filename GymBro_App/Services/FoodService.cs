using GymBro_App.DTO;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GymBro_App.Services;

//TODO: Conform these classes to the api's actual structure
class ExtFoodDTO
{
    public string food_id { get; set; } = "";
    public string food_name { get; set; } = "";
    public string food_description { get; set; } = "";
    public string brand_name { get; set; } = "";
    public ServingList servings { get; set; } = new ServingList();
}

public class Serving
{
    public string serving_description { get; set; } = "";
    public string calories { get; set; } = "";
    public string protein { get; set; } = "";
    public string carbs { get; set; } = "";
    public string fat { get; set; } = "";
}

public class ServingList
{
    public List<Serving> serving { get; set; } = new List<Serving>();
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

class AccessTokenResponse
{
    public string access_token { get; set; } = "";
}

public class FoodService : IFoodService
{
    readonly HttpClient _httpClient;
    readonly ILogger<FoodService> _logger;
    readonly string _clientID;
    readonly string _clientSecret;

    private string? _accessToken { set; get; } = null;
    public DateTime _accessTokenExpiration { private set; get; } = DateTime.MinValue;

    private const string ACCESS_TOKEN_REQUEST_URL = "https://oauth.fatsecret.com/connect/token";
    private const string FOOD_API_URL = "https://platform.fatsecret.com/rest/server.api";

    public FoodService(HttpClient httpClient, ILogger<FoodService> logger, string clientID, string clientSecret)
    {
        _httpClient = httpClient;
        _logger = logger;
        _clientID = clientID;
        _clientSecret = clientSecret;
    }

    public async Task GetNewAccessTokenAsync()
    {   
        string credentials = $"{_clientID}:{_clientSecret}";
        var byteArray = Encoding.ASCII.GetBytes(credentials);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var values = new Dictionary<string, string>
        {
        { "scope", "basic" },
        { "grant_type", "client_credentials" },
        };
        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync("https://oauth.fatsecret.com/connect/token", content);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        if (response.IsSuccessStatusCode)
        {
            var token = await JsonSerializer.DeserializeAsync<AccessTokenResponse>(await response.Content.ReadAsStreamAsync());
            if (token == null)
            {
                _logger.LogError("Failed to retrieve access token: token empty");
                return;
            }
            _accessTokenExpiration = DateTime.Now.AddHours(23);
            _accessToken = token.access_token;
        }else {
            _logger.LogError("Failed to retrieve access token: {0}", await response.Content.ReadAsStringAsync());
        }
    }

    public async Task<List<FoodDTO>> GetFoodsAsync(string query)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, FOOD_API_URL);
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string?, string?>("method", "foods.search"),
            new KeyValuePair<string?, string?>("search_expression", query),
            new KeyValuePair<string?, string?>("format", "json")
        });
        if(DateTime.Now > _accessTokenExpiration)
        {
            await GetNewAccessTokenAsync();
        }
        request.Headers.Add("Accept", "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        var response = await _httpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            // var responseStream = await response.Content.ReadAsStreamAsync();
            var responseStream = await response.Content.ReadAsStringAsync();
            var searchResults = await JsonSerializer.DeserializeAsync<FoodSearchResponse>(await response.Content.ReadAsStreamAsync(), options);
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

    public async Task<FoodDTO?> GetFoodAsync(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, FOOD_API_URL);
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string?, string?>("method", "food.get.v4"),
            new KeyValuePair<string?, string?>("food_id", id),
            new KeyValuePair<string?, string?>("format", "json")
        });
        if(DateTime.Now > _accessTokenExpiration)
        {
            await GetNewAccessTokenAsync();
        }
        request.Headers.Add("Accept", "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        var response = await _httpClient.SendAsync(request);

        _logger.LogDebug($"Response Status: {response.StatusCode}");
        if (response.IsSuccessStatusCode)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = await JsonSerializer.DeserializeAsync<GetFoodResponse>(await response.Content.ReadAsStreamAsync(), options);
            result = result ?? new GetFoodResponse();
            var food = result.food;
            var serving = result.food.servings.serving.FirstOrDefault() ?? new Serving();

            return new FoodDTO
            {
                FoodName = food.food_name,
                FoodId = food.food_id,
                BrandName = food.brand_name,
                FoodDescription = "Per " + serving.serving_description + " - Calories: " + serving.calories + "kcal | Fat: " + serving.fat 
                    + "g | Carbs: " + serving.carbs + "g | Protein: "+ serving.protein  + "g"
            };
        }
        return null;
    }
}