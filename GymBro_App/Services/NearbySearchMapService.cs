using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public class NearbySearchMapService : INearbySearchMapService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<NearbySearchMapService> _logger;

        public class Root
        {
            [JsonPropertyName("places")]
            public List<PlaceDTO> Places { get; set; } = [];
        }

        public class Place
        {
            [JsonPropertyName("displayName")]
            public DisplayName DisplayName { get; set; } = new DisplayName();
            [JsonPropertyName("formattedAddress")]
            public string FormattedAddress { get; set; } = "";
        }

        public class DisplayName
        {
            [JsonPropertyName("text")]
            public string Text { get; set; } = "";
        }

        public NearbySearchMapService(HttpClient httpClient, ILogger<NearbySearchMapService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<PlaceDTO>> FindNearbyGyms(double latitude, double longitude)
        {
            Console.WriteLine("Got to GetNearbyPlaces");
            string url = "https://places.googleapis.com/v1/places:searchNearby";

            var requestBody = new
            {
                includedTypes = new[] { "gym" },
                maxResultCount = 10,
                locationRestriction = new
                {
                    circle = new
                    {
                        center = new
                        {
                            latitude,
                            longitude
                        },
                        radius = 30000.0
                    }
                }
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Root root = await JsonSerializer.DeserializeAsync<Root>(responseStream, options) ?? new Root();

                var places = root.Places.Select(p => new PlaceDTO
                {
                    DisplayName = p.DisplayName,
                    FormattedAddress = p.FormattedAddress
                }).ToList();
                return places;
            }
            else
            {
                _logger.LogError("Failed to get nearby places. Status Code: {0}", response.StatusCode);
                return [];
            }
        }
    }


}