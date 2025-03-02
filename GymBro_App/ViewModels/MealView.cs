namespace GymBro_App.ViewModels
{
    public class MealView
    {
        public List<long>? Foods { get; set; }

        public int MealId { get; set; }

        public string MealName { get; set; } = "";
        
        public string MealType { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
