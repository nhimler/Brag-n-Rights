namespace GymBro_App.ViewModels
{
    public class MealPlanHomeView
    {
        public List<HomeMealPlan> MealPlans { get; set; } = new List<HomeMealPlan>();
    }

    public class HomeMealPlan
    {
        public string PlanName { get; set; } = "";
        public DateOnly StartDate { get; set; }
        public int Id { get; set; }
        public List<HomeMeal> Meals { get; set; } = new List<HomeMeal>();
    }

    public class HomeMeal
    {
        public string MealName { get; set; } = "";
        public List<long> Foods { get; set; }
    }
}
