using GymBro_App.Models;
using GymBro_App.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymBro_App.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GymBro_App.Services
{
    public class Exercises
    {
        [JsonPropertyName("bodyPart")]
        public string BodyPart { get; set; }

        [JsonPropertyName("equipment")]
        public string Equipment { get; set; }

        [JsonPropertyName("gifUrl")]
        public string GifUrl { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("target")]
        public string Target { get; set; }

        [JsonPropertyName("secondaryMuscles")]
        public List<string> SecondaryMuscles { get; set; }

        [JsonPropertyName("instructions")]
        public List<string> Instructions { get; set; }
    }
    public class ExerciseService : IExerciseService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<ExerciseService> _logger;

        public ExerciseService(HttpClient httpClient, ILogger<ExerciseService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /*public async Task<List<ExerciseDTO>> GetExercisesAsync()
        {
            string endpoint = $"/exercises?limit=10&offset=0";
            var response = await _httpClient.GetAsync(endpoint);
            _logger.LogInformation($"Response status code: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Step 1");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Step 2");
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Console.WriteLine("Step 3");
                var results = JsonSerializer.Deserialize<List<ExerciseDTO>>(responseBody, options);
                Console.WriteLine("Step 4");
                return results?? new List<ExerciseDTO>();
                
            }
            return new List<ExerciseDTO>();
        }*/
        public async Task<List<ExerciseDTO>> GetExerciseAsync(string name)
        {
            string endpoint = $"/exercises/name/{name}";
            var response = await _httpClient.GetAsync(endpoint);
            _logger.LogInformation($"Response status code: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var results = JsonSerializer.Deserialize<List<ExerciseDTO>>(responseBody, options);
                return results ?? new List<ExerciseDTO>();
            }
            return new List<ExerciseDTO>();
        }
        public async Task<List<ExerciseDTO>> GetExerciseByIdAsync(string id)
        {
            string endpoint = $"/exercises/exercise/{id}";
            var response = await _httpClient.GetAsync(endpoint);
            _logger.LogInformation($"Response status code: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response body: {responseBody}");
                
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                try {
                    var results = JsonSerializer.Deserialize<List<ExerciseDTO>>(responseBody, options);
                    return results ?? new List<ExerciseDTO>();
                }
                catch {
                    try {
                        var singleResult = JsonSerializer.Deserialize<ExerciseDTO>(responseBody, options);
                        if (singleResult != null) {
                            return new List<ExerciseDTO> { singleResult };
                        }
                    }
                    catch (Exception ex) {
                        _logger.LogError($"Error deserializing response: {ex.Message}");
                    }
                }
            }
            
            return new List<ExerciseDTO>();
        }
    }
}

