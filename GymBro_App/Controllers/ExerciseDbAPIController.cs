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

    /*[HttpGet]
    public async Task<IActionResult> GetExercises()
    {
        var exercises = await _exerciseService.GetExercisesAsync();
    
        if (exercises == null || exercises.Count == 0)
        {
            return NotFound("No exercises found for the given query.");
        }

        return Ok(exercises);
} */

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

    //Id parameter is in the format of a string starting with "0001" and incrementing ex : "0002", "0003", etc.
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetExerciseById(string id)
    {
        var exercises = await _exerciseService.GetExerciseByIdAsync(id);
    
        if (exercises == null || exercises.Count == 0)
        {
            return NotFound($"No exercise found with ID '{id}'. Valid IDs are in the format 0001.");
        }

        return Ok(exercises);
    }

    //Bodypart parameter must be one of the following: back, cardio, chest, lower arms, lower legs, neck, shoulders, upper arms, upper legs, waist
    [HttpGet("bodyPart/{bodyPart}")]
    public async Task<IActionResult> GetExerciseByBodyPart(string bodyPart)
    {
        var exercises = await _exerciseService.GetExerciseByBodyPartAsync(bodyPart);
    
        if (exercises == null || exercises.Count == 0)
        {
            return NotFound($"No exercises found for body part '{bodyPart}'. Valid body parts are: back, cardio, chest, lower arms, lower legs, neck, shoulders, upper arms, upper legs, waist.");
        }

        return Ok(exercises);
    }
}
