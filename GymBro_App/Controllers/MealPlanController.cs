using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

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
        var mealPlans = _mealPlanRepository.GetMealPlansForUser(userId);
        if (mealPlans == null)
        {   
            return View(null);
        }
        MealPlanHomeView mealPlanView = new MealPlanHomeView();
        foreach(MealPlan mealPlan in mealPlans){
            List<string> mealPlanMeals = new List<string>();
            List<List<long>> mealPlanFoods = new List<List<long>>();
            mealPlanView.PlanNames.Add(mealPlan.PlanName ?? "");
            foreach(Meal meal in mealPlan.Meals){
                if(mealPlanView.MealNames != null && meal.MealName != null && mealPlanView.Foods != null){
                mealPlanMeals.Add(meal.MealName);
                List<long> foodIds = new List<long>();
                foreach(Food food in meal.Foods){
                    foodIds.Add(food.ApiFoodId ?? -1);
                }
                mealPlanFoods.Add(foodIds);
                }
            }
            mealPlanView.MealNames.Add(mealPlanMeals);
            mealPlanView.Foods.Add(mealPlanFoods);
        }
        return View(mealPlanView);
    }

    [HttpGet ("CreateMealPlan/{id}")]
    public IActionResult CreateMealPlan(string id)
    {
        if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index");
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Index");
        }
        if(id == "new"){
            return View("CreateMealPlan", null);
        }

        if (!int.TryParse(id, out int mealPlanId))
        {
            return RedirectToAction("Index");
        }

        var mealPlan = _mealPlanRepository.FindById(mealPlanId);
        if (mealPlan == null)
        {
            return RedirectToAction("Index");
        }
        return View("CreateMealPlan", new MealPlanView(mealPlan));
    }

    [HttpPost]
    public IActionResult CreateMealPlan(MealPlanView mv)
    {
        if (ModelState.IsValid)
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
            _mealPlanRepository.Add(new MealPlan()
            {
                UserId = userId,
                PlanName = mv.PlanName,
                StartDate = mv.StartDate,
                EndDate = mv.EndDate,
                Frequency = mv.Frequency,
                TargetCalories = mv.TargetCalories,
                TargetProtein = mv.TargetProtein,
                TargetCarbs = mv.TargetCarbs,
                TargetFats = mv.TargetFats
            });
            return RedirectToAction("Index");
        }
        Debug.WriteLine("Food not added");
        return RedirectToAction("Index");
    }


    [HttpGet ("CreateMeal/{id}")]
    public IActionResult CreateMeal(string id)
    {
        if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index");
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Index");
        }
        var userId = _userRepository.GetIdFromIdentityId(user.Id);
        if(id == "new"){
            var mealPlans = _mealPlanRepository.GetMealPlansForUser(userId);
            MealView mv = new MealView();
            if(mealPlans.IsNullOrEmpty()){
                return RedirectToAction("Index");
            }
            foreach(MealPlan mp in mealPlans){
                mv.PlanIds.Add(mp.MealPlanId);
                mv.PlanNames.Add(mp.PlanName ?? "");
            }
            return View("CreateMeal", mv);
        }

        if (!int.TryParse(id, out int mealId))
        {
            return RedirectToAction("Index");
        }

        var meal = _mealRepository.FindById(mealId);
        if (meal == null)
        {
            return RedirectToAction("Index");
        }
        var mealPlan = _mealPlanRepository.FindById(meal.MealPlanId ?? -1);
        if (mealPlan == null || mealPlan.UserId != userId)
        {
            return RedirectToAction("Index");
        }
        return View("CreateMeal", new MealView()
        {
            MealId = meal.MealId,
            MealName = meal.MealName ?? "",
            MealType = meal.MealType ?? "",
            Description = "",//meal.Description ?? "",
            Foods = meal.Foods.Select(f => f.ApiFoodId ?? -1).ToList()
        });
    }

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
            var mealPlans = _mealPlanRepository.GetMealPlansForUser(userId);
            if (mealPlans.IsNullOrEmpty() || !mealPlans.Select(mp => mp.MealPlanId).Contains(mv.MealPlanId))
            {   
                return RedirectToAction("Index");
            }
            var meal = new Meal()
            {
                MealPlanId = mv.MealPlanId,
                MealName = mv.MealName,
                MealType = mv.MealType,
                Description = mv.Description
            };
            _mealRepository.Add(meal);
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
