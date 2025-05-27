using System.ComponentModel.DataAnnotations;
using GymBro_App.Models;

namespace GymBro_App.ViewModels
{
    public class MealPlanView : IValidatableObject
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

        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate == null || EndDate == null || string.IsNullOrEmpty(Frequency))
                yield break;

            int planLength = EndDate.Value.DayNumber - StartDate.Value.DayNumber + 1;

            if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase) && planLength >= 7)
            {
                yield return new ValidationResult("A weekly plan must be shorter than 7 days.", new[] { nameof(EndDate) });
            }

            if (Frequency.Equals("Monthly", StringComparison.OrdinalIgnoreCase) && planLength >= 30)
            {
                yield return new ValidationResult("A monthly plan must be shorter than 30 days.", new[] { nameof(EndDate) });
            }
        }
    }
}
