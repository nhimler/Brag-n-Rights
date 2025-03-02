namespace GymBro_App.ViewModels
{
    public class MealPlanHomeView
    {
        public List<string>? PlanNames { get; set; } = new List<string>();
        public List<List<string>>? MealNames { get; set; } = new List<List<string>>();

        public List<List<List<long>>>? Foods { get; set; } = new List<List<List<long>>>();
    }
}
