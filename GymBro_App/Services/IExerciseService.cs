using GymBro_App.Models;
namespace GymBro_App.Services
{
    public interface IExerciseService
    {
        Task<List<ApiExercise>> GetExercisesAsync(string query);
        Task<ApiExercise> GetExerciseAsync(string id);
    }
}