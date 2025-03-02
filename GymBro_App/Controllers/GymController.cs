using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;

namespace GymBro_App.Controllers;

public class GymController : Controller
{
    private readonly ILogger<GymController> _logger;

    public GymController(ILogger<GymController> logger)
    {
        _logger = logger;
    }

    public IActionResult FindNearbyGyms()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
