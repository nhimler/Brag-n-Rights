using GymBro_App.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;

namespace GymBro_App.Services
{
    public class MapService : IMapService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<MapService> _logger;

        public MapService(HttpClient httpClient, ILogger<MapService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Get the Google Maps API Key from the HttpClient. This is the best way I could think of to hide the API Key.
        public string GetGoogleMapsApiKey()
        {
            string apiKey = _httpClient.DefaultRequestHeaders.GetValues("X-goog-api-key").FirstOrDefault() ?? "";
            return apiKey;
        }

        
    }   

}