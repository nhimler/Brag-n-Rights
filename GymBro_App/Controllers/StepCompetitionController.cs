using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using Microsoft.AspNetCore.Authorization;  // For [Authorize]
using System.Security.Claims;  // Add this namespace for ClaimsPrincipal and ClaimTypes
using GymBro_App.Services;


namespace GymBro_App.Controllers;

public class StepCompetitionController : Controller
{

        private readonly IOAuthService _oauthService;
       

        public StepCompetitionController(IOAuthService oauthService )
        {
            _oauthService = oauthService;
            
        }
        [Authorize]
        public async Task<IActionResult> Index()
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
            return View();
        }
}