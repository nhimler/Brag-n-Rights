using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.DAL.Concrete
{
    public class MealRepository : Repository<Meal>, IMealRepository

    {
        private readonly DbSet<Meal> _meals;
        private readonly GymBroDbContext _context;

        public MealRepository(GymBroDbContext context) : base(context)
        {
            _meals = context.Meals;
            _context = context;
        }

        public void Add(Meal m)
        {
            _context.Meals.Add(m);
            _context.SaveChanges();
        }

    }
}
