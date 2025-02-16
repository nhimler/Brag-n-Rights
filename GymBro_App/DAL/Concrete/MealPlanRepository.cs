using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.DAL.Concrete
{
    public class MealPlanRepository : Repository<MealPlan>, IMealPlanRepository

    {
        private readonly DbSet<MealPlan> _mealPlans;
        private readonly GymBroDbContext _context;

        public MealPlanRepository(GymBroDbContext context) : base(context)
        {
            _mealPlans = context.MealPlans;
            _context = context;
        }

        public void Add(MealPlan mp)
        {
            _context.MealPlans.Add(mp);
            _context.SaveChanges();
        }

        public MealPlan? GetFirstMealPlanForUser(int userId)
        {
            return GetAll().Where(mp => mp.UserId == userId).FirstOrDefault();
        }

        public bool HasMeals(int mealPlanId)
        {
            return FindById(mealPlanId).Meals.Any();
        }
        
        public Meal? FirstMeal(int mealPlanId)
        {
            return FindById(mealPlanId).Meals.FirstOrDefault();
        }

    }
}
