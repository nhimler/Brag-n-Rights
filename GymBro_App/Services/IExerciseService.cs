using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDTO>> GetExerciseAsync(string name);
        //Task<List<ExerciseDTO>> GetExercisesAsync();
        Task<List<ExerciseDTO>> GetExerciseByIdAsync(string id);
        Task<List<ExerciseDTO>> GetExerciseByBodyPartAsync(string bodyPart);
    }
}