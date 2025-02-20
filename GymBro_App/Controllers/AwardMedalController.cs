using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  // For [Authorize]
using System.Security.Claims;  // Add this namespace for ClaimsPrincipal and ClaimTypes
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using System.Threading.Tasks;

namespace GymBro_App.Controllers
{
     // Ensure that the controller requires authentication
    public class AwardMedalController : Controller
    {
        private readonly IAwardMedalService _awardMedalService;

        public AwardMedalController(IAwardMedalService awardMedalService)
        {
            _awardMedalService = awardMedalService;
        }

        // Action to award medals for a specific user
        [Authorize] 
        public async Task<IActionResult> AwardMedals()
        {
            // Get the identityId from the logged-in user's claims
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Using NameIdentifier for the Identity Id (replace if needed)

            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized();  // Return 401 if the user is not logged in or has no identity
            }

            try
            {
                var awardMedalsResult = await _awardMedalService.AwardUserdMedalsAsync(identityId);

                if (awardMedalsResult.AwardedMedals.Count == 0)
                {
                    return View("NoMedals");  // View for when no medals were awarded today
                }

                return View("AwardedMedals", awardMedalsResult);  // Pass AwardMedal to the view
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and show an error message
                return View("Error", new { message = ex.Message });
            }
        }
    }
}
