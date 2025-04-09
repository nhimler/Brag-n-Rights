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
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        
        private readonly ILogger<WorkoutsController> _logger;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IUserRepository _userRepository;

        public WorkoutsController(IWorkoutPlanRepository workoutPlanRepository, ILogger<WorkoutsController> logger, UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var workoutPlans = _workoutPlanRepository.GetAll().ToList();
            return View(workoutPlans);
        }

        [HttpGet]
        public IActionResult WorkoutCreationPage()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index");
            }
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            int userId = _userRepository.GetIdFromIdentityId(user.Id);
            var workoutPlan = new WorkoutPlan
            {
                UserId = userId
            };
            
            return View(workoutPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkoutPlan workoutPlan)
        {
            try
            {
                if (!User.Identity?.IsAuthenticated ?? false)
                {
                    return RedirectToAction("Index");
                }
                
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                
                int userId = _userRepository.GetIdFromIdentityId(user.Id);
                _logger.LogInformation($"Setting workout plan UserId to {userId}");
                
                if (userId <= 0)
                {
                    _logger.LogError($"Invalid user ID {userId} for identity ID {user.Id}");
                    ModelState.AddModelError("", "Unable to find your user account. Please try logging in again.");
                    return View("WorkoutCreationPage", workoutPlan);
                }
                
                workoutPlan.UserId = userId;
                
                if (workoutPlan.IsCompleted == null)
                {
                    workoutPlan.IsCompleted = 0;
                }
                
                if (string.IsNullOrEmpty(workoutPlan.ApiId))
                {
                    workoutPlan.ApiId = $"local-{Guid.NewGuid()}";
                }
                
                _logger.LogInformation($"Saving WorkoutPlan: ID={workoutPlan.WorkoutPlanId}, UserId={workoutPlan.UserId}, " +
                                      $"PlanName={workoutPlan.PlanName}");
                
                _workoutPlanRepository.Add(workoutPlan);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving workout plan");
                ModelState.AddModelError("", "An error occurred while saving the workout plan: " + ex.Message);
                return View("WorkoutCreationPage", workoutPlan);
            }
        }

        [HttpGet]
        public IActionResult ExerciseSearch()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userRepository.GetIdFromIdentityId(User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                var userWorkoutPlans = _workoutPlanRepository.GetAll()
                    .Where(wp => wp.UserId == userId)
                    .Select(wp => new { wp.WorkoutPlanId, wp.PlanName })
                    .ToList();
                
                ViewBag.WorkoutPlans = userWorkoutPlans;
            }
            
            return View();
        }

        public IActionResult WorkoutCreationActionsPartial()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return PartialView("_WorkoutCreationActions");
            }
            return Content("");
        }

        }
    }
