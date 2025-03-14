using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.Services;
using Microsoft.AspNetCore.Identity;

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
            return View();
        }

        [HttpPost]
        public IActionResult Create(WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                _workoutPlanRepository.Add(workoutPlan);
                return RedirectToAction("Index");
            }
            return View(workoutPlan);
        }

        [HttpGet]
        public IActionResult ExerciseSearch()
        {
            return View();
        }

        public IActionResult WorkoutCreationActionsPartial()
        {
            return PartialView("_WorkoutCreationActions");
        }
    }
}