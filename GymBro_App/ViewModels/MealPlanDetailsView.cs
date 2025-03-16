

using GymBro_App.Models;

namespace GymBro_App.ViewModels
{
    public class MealPlanDetailsView
    {
        public MealPlanView MealPlan { get; set; } = new MealPlanView();
        public List<MealView> Meals { get; set; } = new List<MealView>();
    }
}