using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymBro_App.Services;
using GymBro_App.Models;

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
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();

            if (!await _oauthService.UserHasFitbitToken(identityId))
                return View("ConnectFitbit");


            try
            {
                var awardMedalsResult = await _awardMedalService.AwardUserdMedalsAsync(identityId);

                if (awardMedalsResult.AwardedMedals.Count == 0)
                    return View("NoMedals");

                return View("AwardedMedals", awardMedalsResult);
            }
            catch (TokenExpiredException)
            {
                // Tokens are bad – send them to reconnect Fitbit
                return View("ConnectFitbit");
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }

    }
}
