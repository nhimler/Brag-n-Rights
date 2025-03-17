using GymBro_App.Models;

namespace GymBro_App.ViewModels
{
    public class MealPlanView
    {
        public MealPlanView()
        {
        }

        public MealPlanView(MealPlan mp)
        {
            MealPlanId = mp.MealPlanId;
            UserId = mp.UserId;
            PlanName = mp.PlanName;
            StartDate = mp.StartDate;
            EndDate = mp.EndDate;
            Frequency = mp.Frequency;
            TargetCalories = mp.TargetCalories;
            TargetProtein = mp.TargetProtein;
            TargetCarbs = mp.TargetCarbs;
            TargetFats = mp.TargetFats;
        }

        public int? MealPlanId { get; set; }

        public int? UserId { get; set; }

        public string? PlanName { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string? Frequency { get; set; }

        public int? TargetCalories { get; set; }

        public int? TargetProtein { get; set; }

        public int? TargetCarbs { get; set; }

        public int? TargetFats { get; set; }
    }
}
