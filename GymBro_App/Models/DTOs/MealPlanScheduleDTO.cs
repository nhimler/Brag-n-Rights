namespace GymBro_App.DTO
{
    public class MealPlanScheduleDTO
    {
        public string title { get; set; } = "";
        public DateOnly? start { get; set; }
        public DateOnly? end { get; set; } 
    }
}