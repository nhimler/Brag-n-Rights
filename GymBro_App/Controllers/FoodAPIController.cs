using GymBro_App.DTO;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FoodDTO>))]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string query)
    {
        var foods = await _foodService.GetFoodsAsync(query);
        return Ok(foods);
    }

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FoodDTO))]
    public async Task<IActionResult> GetById([FromQuery(Name = "id")] string id)
    {
        // TODO: add checking for null and non int queries
        var foods = await _foodService.GetFoodAsync(id);
        return Ok(foods);
    }
}


