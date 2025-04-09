namespace GymBro_App.Models.DTOs
{
    public class AddExercisesToWorkoutDto
    {
        public int WorkoutPlanId { get; set; }
        public List<string> ExerciseApiIds { get; set; } = new List<string>();
    }
}