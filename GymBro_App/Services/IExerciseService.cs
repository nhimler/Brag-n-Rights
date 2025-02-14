using GymBro_App.Models;
namespace GymBro_App.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseRespone>> GetExercisesAsync(string query);
        Task<ExerciseRespone> GetExerciseAsync(string id);
    }
}