namespace GymBro_App.ViewModels
{
    public class MealPlanView
    {
        public List<string>? MealNames { get; set; } = new List<string>();

        public List<List<long>>? Foods { get; set; } = new List<List<long>>();
    }
}
