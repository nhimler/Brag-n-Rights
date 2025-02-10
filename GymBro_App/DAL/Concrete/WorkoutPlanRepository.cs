using System.Linq.Expressions;
using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.DAL.Concrete
{
    public class WorkoutPlanRepository : Repository<WorkoutPlan>, IWorkoutPlanRepository
    {
        private readonly DbSet<WorkoutPlan> _workoutplans;
        private readonly GymBroDbContext _context;

        public WorkoutPlanRepository(GymBroDbContext context) : base(context)
        {
            _workoutplans = context.WorkoutPlans;
            _context = context;
        }

        public WorkoutPlan Find(Expression<Func<WorkoutPlan, bool>> predicate)
        {
            return _workoutplans.FirstOrDefault(predicate);
        }

        public void Add(WorkoutPlan workoutPlan)
        {
            _context.WorkoutPlans.Add(workoutPlan);
            _context.SaveChanges();
        }

        public void Update(WorkoutPlan workoutPlan)
        {
            var existingWorkoutPlan = _context.WorkoutPlans.FirstOrDefault(x => x.WorkoutPlanId == workoutPlan.WorkoutPlanId);
            if (existingWorkoutPlan != null)
            {
                existingWorkoutPlan.PlanName = workoutPlan.PlanName;
                _context.WorkoutPlans.Update(existingWorkoutPlan);
                _context.SaveChanges();
                return;
            }
    }
}
}