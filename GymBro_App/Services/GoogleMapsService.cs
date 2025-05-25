using GymBro_App.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;

namespace GymBro_App.Services
{
    public class GeocodeResult(double latitude, double longitude)
    {
        public double Latitude { get; set; } = latitude;
        public double Longitude { get; set; } = longitude;
    }

    public class GoogleMapsService : IGoogleMapsService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<GoogleMapsService> _logger;

        public GoogleMapsService(HttpClient httpClient, ILogger<GoogleMapsService> logger)
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

        public async Task<PlaceDTO> GetPlaceDetails(string placeId)
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            string url = $"https://places.googleapis.com/v1/places/{placeId}?fields=formattedAddress,displayName,regularOpeningHours,websiteUri,name&key={apiKey}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                PlaceDTO myDeserializedClass = JsonSerializer.Deserialize<PlaceDTO>(responseContent, options) ?? new PlaceDTO();
                return myDeserializedClass;
            }
            else
            {
                _logger.LogError($"Error in GetPlaceDetails Service: {response.StatusCode} - {response.ReasonPhrase}");
                return new PlaceDTO();
            }
        }

        public async Task<string> ReverseGeocode(double latitude, double longitude)
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            string url = $"api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                GeocodeDTO myDeserializedClass = JsonSerializer.Deserialize<GeocodeDTO>(responseContent) ?? new GeocodeDTO();
                return myDeserializedClass.Results[0].FormattedAddress;
            }
            else
            {
                return "";
            }
        }

        public async Task<GeocodeResult> GeocodePostalCode(string postalCode)
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            string url = $"api/geocode/json?components=postal_code:{postalCode}&key={apiKey}";
             var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                GeocodeDTO myDeserializedClass = JsonSerializer.Deserialize<GeocodeDTO>(responseContent) ?? new GeocodeDTO();
                if (myDeserializedClass.Results.Count > 0)
                {
                    double lat = myDeserializedClass.Results[0].Geometry.Location.Lat;
                    double lng = myDeserializedClass.Results[0].Geometry.Location.Lng;
                    return new GeocodeResult(lat, lng);
                }
                else
                {
                    _logger.LogError($"No results found for postal code: {postalCode}");
                    return new GeocodeResult(0, 0);
                }
            }
            else
            {
                _logger.LogError($"Error in GeocodePostalCode Service: {response.StatusCode} - {response.ReasonPhrase}");
                return new GeocodeResult(0, 0);
            }
        }

        
    }
}