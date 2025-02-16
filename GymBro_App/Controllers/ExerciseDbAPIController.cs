using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/exercises")]
public class ExerciseDbAPIController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly ILogger<ExerciseDbAPIController> _logger;

    public ExerciseDbAPIController(IExerciseService exerciseService, ILogger<ExerciseDbAPIController> logger)
    {
        _exerciseService = exerciseService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetExercises()
    {
        var exercises = await _exerciseService.GetExercisesAsync();
    
        if (exercises == null || exercises.Count == 0)
        {
            return NotFound("No exercises found for the given query.");
        }

        return Ok(exercises);
}

    [HttpGet("{name}")]
    public async Task<IActionResult> GetExercise(string name)
    {
        var exercises = await _exerciseService.GetExerciseAsync(name);
    
        if (exercises == null || exercises.Count == 0)
        {
            return NotFound($"No exercises found for '{name}'.");
        }

        return Ok(exercises);
}
}
