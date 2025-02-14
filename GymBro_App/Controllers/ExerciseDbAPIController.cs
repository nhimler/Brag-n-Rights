using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

// [Route("api/exercise")]

public class ExerciseDbAPIController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly ILogger<ExerciseDbAPIController> _logger;

    public ExerciseDbAPIController(IExerciseService exerciseService, ILogger<ExerciseDbAPIController> logger)
    {
        _exerciseService = exerciseService;
        _logger = logger;
    }
}