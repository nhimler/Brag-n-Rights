using GymBro_App.Models;
using System.Linq.Expressions;

namespace GymBro_App.DAL.Abstract
{
    public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
    {
        WorkoutPlan Find(Expression<Func<WorkoutPlan, bool>> predicate);
        void Add(WorkoutPlan workoutPlan);
        void Update(WorkoutPlan workoutPlan);
    }
}
