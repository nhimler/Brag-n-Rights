using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GymBro_App.Models.DTOs;
using System.Security.Claims;

namespace GymBro_App.Controllers
{
    [Route("api/Workouts")]
    [ApiController]
    [Authorize]
    public class WorkoutsAPIController : ControllerBase
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly ILogger<WorkoutsAPIController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseService _exerciseService;

        public WorkoutsAPIController(IWorkoutPlanRepository workoutPlanRepository, ILogger<WorkoutsAPIController> logger, UserManager<IdentityUser> userManager, IUserRepository userRepository, IExerciseService exerciseService)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
            _exerciseService = exerciseService;
        }

        [HttpPost("AddExercises")]
        [Authorize]
        public IActionResult AddExercisesToWorkout([FromBody] AddExercisesToWorkoutDto dto)
        {
            var currentUserId = _userRepository.GetIdFromIdentityId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var workoutPlan = _workoutPlanRepository.FindById(dto.WorkoutPlanId);
            if (workoutPlan == null)
            {
                return NotFound("Workout plan not found.");
            }
            if (workoutPlan.UserId != currentUserId)
            {
                return Forbid();
            }

            foreach (var exerciseApiId in dto.ExerciseApiIds)
            {
                workoutPlan.WorkoutPlanExercises.Add(new WorkoutPlanExercise
                {
                    WorkoutPlanId = workoutPlan.WorkoutPlanId,
                    ApiId = exerciseApiId,
                });
            }

            try
            {
                _workoutPlanRepository.Update(workoutPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workout plan with added exercises");
                return StatusCode(500, "Error updating workout plan: " + ex.Message);
            }

            return Ok(new
            {
                workoutPlanId = workoutPlan.WorkoutPlanId,
                addedExercisesCount = dto.ExerciseApiIds.Count
            });

        }

        [HttpGet("{planId}/Exercises")]
        public async Task<IActionResult> GetExercisesForPlan(int planId)
        {
            var plan = _workoutPlanRepository.FindById(planId);
            if (plan == null) return NotFound($"Plan {planId} not found.");

            var result = new List<object>();
            foreach (var wpe in plan.WorkoutPlanExercises)
            {
                var exs = await _exerciseService.GetExerciseByIdAsync(wpe.ApiId);
                var ex = exs?.FirstOrDefault();
                if (ex == null) continue;
                result.Add(new
                {
                    ex.Id,
                    ex.Name,
                    ex.GifUrl,
                    BodyPart = ex.BodyPart,
                    Equipment = ex.Equipment,
                    Target = ex.Target,
                    SecondaryMuscles = ex.SecondaryMuscles,
                    Instructions = ex.Instructions,
                    Sets = wpe.Sets ?? 0,
                    Reps = wpe.Reps ?? 0,
                    Weight = wpe.Weight ?? 0
                });
            }
            return Ok(result);
        }

        [HttpPut("Exercise")]
        public IActionResult UpdateSetsAndReps([FromBody] UpdateExerciseDTO dto)
        {
            var plan = _workoutPlanRepository.FindById(dto.PlanId);
            if (plan == null) return NotFound();
            var wpe = plan.WorkoutPlanExercises.FirstOrDefault(e => e.ApiId == dto.ApiId);
            if (wpe == null) return NotFound();
            wpe.Sets = dto.Sets;
            wpe.Reps = dto.Reps;
            wpe.Weight = dto.Weight;
            _workoutPlanRepository.Update(plan);
            return NoContent();
        }
    }
}
