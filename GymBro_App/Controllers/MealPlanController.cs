using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GymBro_App.Controllers;

public class MealPlanController : Controller
{
    private readonly ILogger<MealPlanController> _logger;

    private readonly IFoodRepository _foodRepository;

    private readonly IMealRepository _mealRepository;

    private readonly IMealPlanRepository _mealPlanRepository;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly IRepository<User> _userRepository;

    public MealPlanController(ILogger<MealPlanController> logger, IFoodRepository foodRepository, IMealRepository mealRepository,
            IMealPlanRepository mealPlanRepository, UserManager<IdentityUser> userManager, IRepository<User> userRepository)
    {
        _logger = logger;
        _foodRepository = foodRepository;
        _mealRepository = mealRepository;
        _mealPlanRepository = mealPlanRepository;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(MealPlan? mp = null)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return View();
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return View();
        }
        var userId = _userRepository.GetAll().Where(u => u.IdentityUserId == user.Id).FirstOrDefault()?.UserId;
        var mealPlan = _mealPlanRepository.GetAll().Where(m => m.UserId == userId).FirstOrDefault();
        if (mealPlan == null)
        {
            _mealPlanRepository.Add(new MealPlan()
            {
                UserId = userId,
                PlanName = "New Meal Plan"
            });
            Console.WriteLine("New meal plan added");
        }
        mealPlan = _mealPlanRepository.GetAll().Where(m => m.UserId == userId).FirstOrDefault();
        if (!_mealRepository.GetAll().Where(m => m.MealPlanId == mealPlan.MealPlanId).Any())
        {
            _mealRepository.Add(new Meal()
            {
                MealPlanId = mealPlan.MealPlanId,
                MealName = "New Meal"
            });
            Console.WriteLine("New meal added");
        }
        return View();
    }

    [HttpGet]
    public IActionResult CreateMeal()
    {
        return View(null);
    }

    // TODO: Actually create user defined meals here
    // Currently only adds foods to the database
    [HttpPost]
    public IActionResult CreateMeal(MealView m)
    {
        if (ModelState.IsValid && m.Foods != null)
        {
            foreach (var food in m.Foods)
            {
                _foodRepository.Add(new Food
                {
                    ApiFoodId = food,
                    MealId = null
                });
            }
            return RedirectToAction("Index");
        }
        Debug.WriteLine("Food not added");
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
