using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;

namespace GymBro_App.Controllers;

public class UserPageController : Controller
{
    private readonly ILogger<UserPageController> _logger;

    public UserPageController(ILogger<UserPageController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
