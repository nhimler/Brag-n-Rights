
using GymBro_App.Models;
using System.Linq.Expressions;

namespace GymBro_App.DAL.Abstract;
public interface IFoodRepository : IRepository<Food>
{
    Food Find(Expression<Func<Food, bool>> predicate);
    void Add(Food f);
    void Update(Food f);
}
