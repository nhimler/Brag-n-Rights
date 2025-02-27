using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/maps")]
public class GoogleMapsAPIController : ControllerBase
{
    private readonly IMapService _mapService;
    private readonly ILogger<GoogleMapsAPIController> _logger;

    public GoogleMapsAPIController(IMapService mapService, ILogger<GoogleMapsAPIController> logger)
    {
        _mapService = mapService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetGoogleMapsApiKey()
    {
        var apiKey = await _mapService.GetGoogleMapsApiKey();
        _logger.LogInformation("Google Maps API Key: {apiKey}", apiKey);
        return Ok(apiKey);
    }
}
