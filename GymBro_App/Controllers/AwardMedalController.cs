using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using System.Threading.Tasks;
using System.Security.Claims;

public class AwardMedalController : Controller
{
    private readonly IAwardMedalService _awardMedalService;

    public AwardMedalController(IAwardMedalService awardMedalService)
    {
        _awardMedalService = awardMedalService;
    }

    public async Task<IActionResult> AwardMedals()
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user’s Identity ID

        if (string.IsNullOrEmpty(identityId))
        {
            return Unauthorized("User not logged in.");  // Return 401 if not authenticated
        }

        try
        {
            var awardMedalsResult = await _awardMedalService.AwardUserdMedalsAsync(identityId);

            if (awardMedalsResult.AwardedMedals.Count == 0)
            {
                return View("NoMedals");  // No medals awarded
            }

            return View("AwardedMedals", awardMedalsResult);  // Show awarded medals
        }
        catch (Exception ex)
        {
            return View("Error", new { message = ex.Message });
        }
    }
}
