using GymBro_App.Models;
using System.Text.Json;

namespace GymBro_App.Services;





public class ExerciseService : IExerciseService
{
    readonly HttpClient _httpClient;
    readonly ILogger<ExerciseService> _logger;

    public ExerciseService(HttpClient httpClient, ILogger<ExerciseService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<ApiExercise> GetExerciseAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ApiExercise>> GetExercisesAsync(string query)
    {
        throw new NotImplementedException();
    }
}

public class ApiExercise
{
}