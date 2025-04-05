using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDTO>> GetExerciseAsync(string name);
        Task<List<ExerciseDTO>> GetExercisesAsync();
        Task<ExerciseDTO> GetExerciseByIdAsync(int id);
    }
}