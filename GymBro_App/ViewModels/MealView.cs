using System.ComponentModel.DataAnnotations;
using GymBro_App.Models;

namespace GymBro_App.ViewModels
{
    public class MealView
    {
        public MealView()
        {
        }

        public MealView(Meal meal)
        {
            MealId = meal.MealId;
            MealPlanId = meal.MealPlanId ?? -1;
            MealName = meal.MealName ?? "";
            MealType = meal.MealType ?? "";
            Description = meal.Description ?? "";
            Date = meal.Date;
        }
        public List<long> Foods { get; set; } = new List<long>();

        public List<string> PlanNames { get; set; } = new List<string>();

        public List<int> PlanIds { get; set; } = new List<int>();

        public DateOnly? Date { get; set; }

        public int MealId { get; set; }

        public int MealPlanId { get; set; }

        public string MealName { get; set; } = "";

        public string MealType { get; set; } = "";

        public string Description { get; set; } = "";

        public List<DateOnly> StartDates { get; set; } = new List<DateOnly>();
        public List<DateOnly> EndDates { get; set; } = new List<DateOnly>();

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (Date == null || MealPlan == null || MealPlan.StartDate == null || MealPlan.EndDate == null)
        //         yield break;

        //     if (Date < MealPlan.StartDate || Date > MealPlan.EndDate)
        //     {
        //         yield return new ValidationResult(
        //             "Meal date must be within the meal plan's start and end dates.",
        //             new[] { nameof(Date) });
        //     }
        // }
    }
}
