using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GymBro_App.Controllers;

[Route("api/food")]
public class FoodAPIController : ControllerBase
{

    private readonly IFoodService _foodService;
    private readonly ILogger<FoodAPIController> _logger;

    public FoodAPIController(IFoodService foodService, ILogger<FoodAPIController> logger)
    {
        _foodService = foodService;
        _logger = logger;
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApiFood>))]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string query)
    {
        // TODO: add checking for null and non int queries
        var foods = await _foodService.GetFoodAsync(query);
        return Ok(foods);
    }
}


