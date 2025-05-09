namespace GymBro_App.Models.DTOs
{
    public class UpdateExerciseDTO
    {
        public int PlanId { get; set; }
        public string ApiId { get; set; } = "";
        public int Sets { get; set; }
        public int Reps { get; set; }
    }
}