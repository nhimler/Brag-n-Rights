using GymBro_App.Models;
using GymBro_App.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    public class ExerciseRespone
    {
        [JsonPropertyName("exercises")]
        public List<Exercises> Exercises { get; set; }
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

        public Task<ExerciseRespone> GetExerciseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExerciseRespone>> GetExercisesAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}

