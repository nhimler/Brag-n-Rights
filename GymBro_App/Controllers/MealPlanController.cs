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

    private readonly IUserRepository _userRepository;

    public MealPlanController(ILogger<MealPlanController> logger, IFoodRepository foodRepository, IMealRepository mealRepository,
            IMealPlanRepository mealPlanRepository, UserManager<IdentityUser> userManager, IUserRepository userRepository)
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
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return View(null);
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return View(null);
        }
        int userId = _userRepository.GetIdFromIdentityId(user.Id);
        var mealPlan = _mealPlanRepository.GetFirstMealPlanForUser(userId);
        if (mealPlan == null)
        {   
            return View(null);
        }
        MealPlanView mealPlanView = new MealPlanView();
        foreach(Meal meal in mealPlan.Meals){
            mealPlanView.MealNames.Add(meal.MealName);
            List<long> foodIds = new List<long>();
            foreach(Food food in meal.Foods){
                foodIds.Add(food.ApiFoodId ?? -1);
            }
            mealPlanView.Foods.Add(foodIds);
        }
        return View(mealPlanView);
    }

    [HttpPost]
    public IActionResult Index(MealPlan? mp = null)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return View(null);
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return View(null);
        }
        var userId = _userRepository.GetIdFromIdentityId(user.Id);
        var mealPlan = _mealPlanRepository.GetFirstMealPlanForUser(userId);
        if (mealPlan == null)
        {
            _mealPlanRepository.Add(new MealPlan()
            {
                UserId = userId,
                PlanName = "New Meal Plan"
            });
            Debug.WriteLine("New meal plan added");
        }
        mealPlan = _mealPlanRepository.GetFirstMealPlanForUser(userId);
        if (!_mealPlanRepository.HasMeals(mealPlan.MealPlanId))
        {
            _mealRepository.Add(new Meal()
            {
                MealPlanId = mealPlan.MealPlanId,
                MealName = "New Meal"
            });
            Debug.WriteLine("New meal added");
        }
        return View(null);
    }

    [HttpGet]
    public IActionResult CreateMeal()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Index");
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Index");
        }
        var userId = _userRepository.GetIdFromIdentityId(user.Id);
        var mealPlan = _mealPlanRepository.GetFirstMealPlanForUser(userId);
        if (mealPlan == null)
        {   
            return RedirectToAction("Index");
        }
        var meal = _mealPlanRepository.FirstMeal(mealPlan.MealPlanId);
        if (meal == null)
        {
            return RedirectToAction("Index");
        }
        return View(null);
    }

    // TODO: Actually create user defined meals here
    // Currently only adds foods to the database
    [HttpPost]
    public IActionResult CreateMeal(MealView mv)
    {
        if (ModelState.IsValid && mv.Foods != null)
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
            {
                return RedirectToAction("Index");
            }
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var userId = _userRepository.GetIdFromIdentityId(user.Id);
            var mealPlan = _mealPlanRepository.GetFirstMealPlanForUser(userId);
            if (mealPlan == null)
            {   
                return RedirectToAction("Index");
            }
            var meal = _mealRepository.GetAll().Where(m => m.MealPlanId == mealPlan.MealPlanId).FirstOrDefault();
            if (meal == null)
            {
                return RedirectToAction("Index");
            }
            foreach (var food in mv.Foods)
            {
                _foodRepository.Add(new Food
                {
                    ApiFoodId = food,
                    MealId = meal.MealId
                });
            }
            return RedirectToAction("Index");
        }
        Debug.WriteLine("Food not added");
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
