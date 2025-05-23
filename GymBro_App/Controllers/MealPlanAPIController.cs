using GymBro_App.DTO;
using GymBro_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GymBro_App.Controllers;

[Route("api/mealplans")]
public class MealPlanAPIController : ControllerBase
{
    private readonly ILogger<MealPlanAPIController> _logger;
    private readonly IMealPlanRepository _mealPlanRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;

    public MealPlanAPIController(ILogger<MealPlanAPIController> logger, IMealPlanRepository mealPlanRepository, 
    UserManager<IdentityUser> userManager, IUserRepository userRepository)
    {
        _logger = logger;
        _mealPlanRepository = mealPlanRepository;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    [HttpGet("schedule/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MealPlanScheduleDTO>))]
    public async Task<IActionResult> GetSchedules(int id)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return new UnauthorizedResult();
        }
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return new NotFoundResult();
        }
        int userId = _userRepository.GetIdFromIdentityId(user.Id);
        var mealPlans = _mealPlanRepository.GetMealPlansForUser(userId);

        mealPlans = mealPlans?.OrderBy(mp => mp.StartDate).ToList();
        if (mealPlans == null)
        {
            return new NotFoundResult();
        }

        IEnumerable<MealPlanScheduleDTO> mealPlanSchedules = mealPlans.Select(mp => new MealPlanScheduleDTO
        {
            title = mp.PlanName != null ? mp.PlanName : "No Name",
            start = mp.StartDate,
            end = mp.EndDate
        });

        foreach (var mealPlan in mealPlans)
        {
            var meals = mealPlan.Meals;
            if (meals != null)
            {
                foreach (var meal in meals)
                {
                    if (meal == null)
                    {
                        continue;
                    }
                    var mealSchedule = new MealPlanScheduleDTO
                    {
                        title = meal.MealName != null ? meal.MealName : "No Name",
                        start = meal.Date,
                        end = meal.Date
                    };
                    mealPlanSchedules = mealPlanSchedules.Append(mealSchedule);
                }
            }
        }
        return Ok(mealPlanSchedules);
    }

}


