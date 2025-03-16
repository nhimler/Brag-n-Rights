using GymBro_App.Models;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/maps")]
public class GoogleMapsAPIController : ControllerBase
{
    private readonly IGoogleMapsService _googleMapsService;
    private readonly INearbySearchMapService _nearbySearchMapService;
    private readonly ILogger<GoogleMapsAPIController> _logger;

    public GoogleMapsAPIController(IGoogleMapsService googleMapsService, ILogger<GoogleMapsAPIController> logger, INearbySearchMapService nearbySearchMapService)
    {
        _googleMapsService = googleMapsService;
        _nearbySearchMapService = nearbySearchMapService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetGoogleMapsApiKey()
    {
        var apiKey = await _googleMapsService.GetGoogleMapsApiKey();
        return Ok(new { apiKey });
    }

    // TODO: Call this method in a better way (ex: "api/maps/nearby?latitude=lat&longitude=long"). We'll also need to 
    // update this method to use a postal code and/or city and state instead.
    [HttpGet("nearby/{latitude}/{longitude}")]
    public async Task<IActionResult> GetNearbyPlaces(double latitude, double longitude)
    {
        var nearbyPlaces = await _nearbySearchMapService.FindNearbyGyms(latitude, longitude);
        return Ok(nearbyPlaces);
    }

    [HttpGet("reversegeocode/{latitude}/{longitude}")]
    public async Task<IActionResult> ReverseGeocode(double latitude, double longitude)
    {
        var address = await _googleMapsService.ReverseGeocode(latitude, longitude);
        return Ok(new { address });
    }
}
