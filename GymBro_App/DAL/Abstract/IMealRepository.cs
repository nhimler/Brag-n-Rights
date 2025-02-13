
using GymBro_App.Models;
using System.Linq.Expressions;

namespace GymBro_App.DAL.Abstract;
public interface IMealRepository : IRepository<Meal>
{
    Meal Find(Expression<Func<Meal, bool>> predicate);
    void Add(Meal m);
}
