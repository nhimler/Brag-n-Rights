using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GymBro_App.ViewModels;

namespace GymBro_App.Controllers;

public class UserPageController : Controller
{
    private readonly ILogger<UserPageController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public UserPageController(ILogger<UserPageController> logger, IUserRepository userRepository, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userRepository = userRepository;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index(UserInfoModel userInfoModel)
    {
        string identityId = _userManager.GetUserId(User) ?? "";
        Models.User gymBroUser = _userRepository.GetUserByIdentityUserId(identityId);
        userInfoModel.WorkoutPlans = _userRepository.GetWorkoutPlansByIdentityUserId(identityId, 1);
        userInfoModel.SetInfoFromUserModel(gymBroUser);
        return View("Index", userInfoModel);
    }

    [Authorize]
    [HttpPost]
    public IActionResult UpdateUserInfo(UserInfoModel userInfoModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ChangeInfo", userInfoModel);
        }

        string identityId = _userManager.GetUserId(User) ?? "";
        _userRepository.UpdateUser(identityId, userInfoModel);

        return RedirectToAction("Index", userInfoModel);
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult ChangeInfo(UserInfoModel userInfoModel)
    {
        string identityId = _userManager.GetUserId(User) ?? "";
        Models.User gymBroUser = _userRepository.GetUserByIdentityUserId(identityId);
        userInfoModel.SetInfoFromUserModel(gymBroUser);
        
        return View("ChangeInfo", userInfoModel);
    }

    [Authorize]
    [HttpGet]
    public IActionResult BookmarkedGyms()
    {
        return View("BookmarkedGyms");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
