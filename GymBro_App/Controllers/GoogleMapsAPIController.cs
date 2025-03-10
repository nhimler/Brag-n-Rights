using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/maps")]
public class GoogleMapsAPIController : ControllerBase
{
    private readonly IEmbedMapService _embedMapService;
    private readonly INearbySearchMapService _nearbySearchMapService;
    private readonly ILogger<GoogleMapsAPIController> _logger;

    public GoogleMapsAPIController(IEmbedMapService embedMapService, ILogger<GoogleMapsAPIController> logger, INearbySearchMapService nearbySearchMapService)
    {
        _embedMapService = embedMapService;
        _nearbySearchMapService = nearbySearchMapService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetGoogleMapsApiKey()
    {
        var apiKey = await _embedMapService.GetGoogleMapsApiKey();
        return Ok(new { apiKey });
    }

    // TODO: Call this method in a better way (ex: "api/maps/nearby?latitude=lat&longitude=long")
    [HttpGet("nearby/{latitude}/{longitude}")]
    public async Task<IActionResult> GetNearbyPlaces(double latitude, double longitude)
    {
        var nearbyPlaces = await _nearbySearchMapService.FindNearbyGyms(latitude, longitude);
        return Ok(nearbyPlaces);
    }
}
