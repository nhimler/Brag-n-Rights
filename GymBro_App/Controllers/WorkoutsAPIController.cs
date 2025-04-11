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

        public WorkoutsAPIController(IWorkoutPlanRepository workoutPlanRepository, ILogger<WorkoutsAPIController> logger, UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
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
    }
}
