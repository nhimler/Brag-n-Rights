using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.DAL.Concrete
{
    public class FoodRepository : Repository<Food>, IFoodRepository

    {
        private readonly DbSet<Food> _food;
        private readonly GymBroDbContext _context;

        public FoodRepository(GymBroDbContext context) : base(context)
        {
            _food = context.Foods;
            _context = context;
        }

        public void Add(Food f)
        {
            _context.Foods.Add(f);
            _context.SaveChanges();
        }

        public void Update(Food f)
        {
            Food? oldFood = _context.Foods.FirstOrDefault(x => x.FoodId == f.FoodId);
            if (oldFood != null)
            {
                oldFood.ApiFoodId = f.ApiFoodId;
                oldFood.Amount = 1;
                _context.Foods.Update(oldFood);
                _context.SaveChanges();
                return;
            }
        }
    }
}
