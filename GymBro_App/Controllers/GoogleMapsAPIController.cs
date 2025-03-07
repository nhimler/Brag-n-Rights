using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/maps")]
public class GoogleMapsAPIController : ControllerBase
{
    private readonly IEmbedMapService _embedMapService;
    private readonly ILogger<GoogleMapsAPIController> _logger;

    public GoogleMapsAPIController(IEmbedMapService embedMapService, ILogger<GoogleMapsAPIController> logger)
    {
        _embedMapService = embedMapService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetGoogleMapsApiKey()
    {
        var apiKey = await _embedMapService.GetGoogleMapsApiKey();
        return Ok(new { apiKey });
    }
}
