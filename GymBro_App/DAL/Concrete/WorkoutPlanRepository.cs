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

        public WorkoutPlan FindById(int id)
        {
            return _workoutplans
                    .Include(wp => wp.WorkoutPlanExercises)
                    .FirstOrDefault(wp => wp.WorkoutPlanId == id);
        }

        public void Add(WorkoutPlan workoutPlan)
        {
            try
            {
                if (workoutPlan == null)
                {
                    throw new ArgumentNullException(nameof(workoutPlan), "WorkoutPlan cannot be null");
                }

                if (workoutPlan.UserId == null || workoutPlan.UserId <= 0)
                {
                    throw new InvalidOperationException($"Invalid UserId: {workoutPlan.UserId}. User ID must be a positive number.");
                }

                var userExists = _context.Users.Any(u => u.UserId == workoutPlan.UserId);
                if (!userExists)
                {
                    throw new InvalidOperationException($"No user found with ID: {workoutPlan.UserId}");
                }

                _context.WorkoutPlans.Add(workoutPlan);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerEx = ex.InnerException;
                throw new Exception($"Failed to save workout plan. UserId: {workoutPlan.UserId}, Error: {innerEx?.Message}", ex);
            }
        }

        public void Update(WorkoutPlan workoutPlan)
        {
            foreach (var exercise in workoutPlan.WorkoutPlanExercises)
            {
                if (exercise.WorkoutPlanExerciseId == 0)
                {
                    _context.Entry(exercise).State = EntityState.Added;
                }
            }

            _context.WorkoutPlans.Update(workoutPlan);
            _context.SaveChanges();
        }

        public IEnumerable<WorkoutPlanTemplate> GetAllPreMadeWorkoutPlans()
        {
            // Only pulls PlanName, DifficultyLevel, and the list of ApiID strings
            return _context.WorkoutPlanTemplates
                        .Select(t => new WorkoutPlanTemplate
                        {
                            WorkoutPlanTemplateID = t.WorkoutPlanTemplateID,
                            PlanName = t.PlanName,
                            DifficultyLevel = t.DifficultyLevel,
                            Exercises = t.Exercises
                                                    .Select(e => new WorkoutPlanTemplateExercise
                                                    {
                                                        ApiID = e.ApiID
                                                    }).ToList()
                        })
                        .AsNoTracking()
                        .ToList();
        }

        public void SavePremadeWorkoutPlan(ApplyTemplateDto dto, int userId)
        {
            // 1) Build the new plan
            var plan = new WorkoutPlan
            {
                UserId          = userId,
                PlanName        = dto.PlanName,
                DifficultyLevel = dto.Difficulty,
                ApiId           = Guid.NewGuid().ToString(),
                // optional: set defaults or leave null
                StartDate       = null,
                EndDate         = null,
                IsCompleted     = 0,
                ArchivedWorkout = false
            };

            // 2) Add exercises with default reps/sets
            const int defaultReps = 10;
            const int defaultSets = 3;

            foreach (var apiId in dto.ExerciseApiIds)
            {
                plan.WorkoutPlanExercises.Add(new WorkoutPlanExercise
                {
                    ApiId          = apiId,
                    Reps           = defaultReps,
                    Sets           = defaultSets,
                    // WorkoutPlanId will be set automatically when plan is saved
                });
            }

            // 3) Persist in one SaveChanges call (EF will insert the plan,
            //    get its PK, then insert the child exercises with that FK)
            _context.WorkoutPlans.Add(plan);
            _context.SaveChanges();
        }

    }
}