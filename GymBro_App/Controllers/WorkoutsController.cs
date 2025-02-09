using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.Controllers
{
    public class WorkoutPlanController : Controller
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public WorkoutPlanController(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
        }

        public IActionResult Index()
        {
            var workoutPlans = _workoutPlanRepository.GetAll().ToList();
            return View(workoutPlans);
        }

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