namespace GymBro_App.ViewModels
{
    public class MealView
    {
        public List<long>? Foods { get; set; }

        public List<string> PlanNames { get; set; } = new List<string>();

        public List<int> PlanIds { get; set; } = new List<int>();

        public int MealId { get; set; }

        public int MealPlanId { get; set; }

        public string MealName { get; set; } = "";
        
        public string MealType { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
