using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public WorkoutsController(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
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
    }
}