using GymBro_App.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;

namespace GymBro_App.Services
{
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

        public async Task<string> ReverseGeocode(double latitude, double longitude)
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            string url = $"api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ReverseGeocodeDTO myDeserializedClass = JsonSerializer.Deserialize<ReverseGeocodeDTO>(responseContent) ?? new ReverseGeocodeDTO();
                return myDeserializedClass.Results[0].FormattedAddress;
            }
            else
            {
                return "";
            }
        }

        
    }
}