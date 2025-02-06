using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;

namespace GymBro_App.Controllers;

public class WorkoutsController : Controller
{
    private readonly ILogger<WorkoutsController> _logger;

    public WorkoutsController(ILogger<WorkoutsController> logger)
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