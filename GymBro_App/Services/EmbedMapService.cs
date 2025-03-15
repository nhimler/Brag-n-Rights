using GymBro_App.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;

namespace GymBro_App.Services
{

    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; } = "";

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = "";

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = [];
    }

    public class Bounds
    {
        [JsonPropertyName("northeast")]
        public Northeast Northeast { get; set; } = new Northeast();

        [JsonPropertyName("southwest")]
        public Southwest Southwest { get; set; } = new Southwest();
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();

        [JsonPropertyName("location_type")]
        public string LocationType { get; set; } = "";

        [JsonPropertyName("viewport")]
        public Viewport Viewport { get; set; } = new Viewport();

        [JsonPropertyName("bounds")]
        public Bounds Bounds { get; set; } = new Bounds();
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public class NavigationPoint
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();
    }

    public class Northeast
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class PlusCode
    {
        [JsonPropertyName("compound_code")]
        public string CompoundCode { get; set; } = "";

        [JsonPropertyName("global_code")]
        public string GlobalCode { get; set; } = "";
    }

    public class Result
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; } = [];

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = "";

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = new Geometry();

        [JsonPropertyName("navigation_points")]
        public List<NavigationPoint> NavigationPoints { get; set; } = [];

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = "";

        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = new PlusCode();

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = [];
    }

    public class Root
    {
        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = new PlusCode();

        [JsonPropertyName("results")]
        public List<Result> Results { get; set; } = [];

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }

    public class Southwest
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonPropertyName("northeast")]
        public Northeast Northeast { get; set; } = new Northeast();

        [JsonPropertyName("southwest")]
        public Southwest Southwest { get; set; } = new Southwest();
    }

    public class EmbedMapService : IEmbedMapService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<EmbedMapService> _logger;

        public EmbedMapService(HttpClient httpClient, ILogger<EmbedMapService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Get the Google Maps API Key from the HttpClient. This is the best way I could think of to hide the API Key.
        public async Task<string> GetGoogleMapsApiKey()
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            return apiKey;
        }

        public async Task<string> ReverseGeocode(double latitude, double longitude)
        {
            _logger.LogInformation("Reverse Geocoding location: {0}, {1}", latitude, longitude);
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";
            string response = await _httpClient.GetStringAsync(url);
            if (response != null)
            {
                Root myDeserializedClass = JsonSerializer.Deserialize<Root>(response) ?? new Root();
                return myDeserializedClass.Results[0].FormattedAddress;
            }
            else
            {
                return "";
            }
        }
    }
}