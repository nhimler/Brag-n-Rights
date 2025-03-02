using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  // For [Authorize]
using System.Security.Claims;  // Add this namespace for ClaimsPrincipal and ClaimTypes
using GymBro_App.Services;

namespace GymBro_App.Controllers
{
     // Ensure that the controller requires authentication
    public class AwardMedalController : Controller
    {
        private readonly IAwardMedalService _awardMedalService;
        private readonly IOAuthService _oauthService;

        public AwardMedalController(IAwardMedalService awardMedalService, IOAuthService oAuthService)
        {
            _awardMedalService = awardMedalService;
            _oauthService = oAuthService;
        }

        // Action to award medals for a specific user
        [Authorize]
        public async Task<IActionResult> AwardMedals()
        {
            // Get the identityId from the logged-in user's claims
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized();  // Return 401 if the user is not logged in or has no identity
            }

            if (!await _oauthService.UserHasFitbitToken(identityId))
            {
                return View("ConnectFitbit");
            }


            try
            {
                var awardMedalsResult = await _awardMedalService.AwardUserdMedalsAsync(identityId);

                if (awardMedalsResult.AwardedMedals.Count == 0)
                {
                    return View("NoMedals");  // View for when no medals were awarded today
                }

                return View("AwardedMedals", awardMedalsResult);
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }

    }
}
