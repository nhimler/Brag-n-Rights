using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models;
using Microsoft.AspNetCore.Authorization;  // For [Authorize]
using System.Security.Claims;  // Add this namespace for ClaimsPrincipal and ClaimTypes
using GymBro_App.Services;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.Controllers;

public class StepCompetitionController : Controller
{

        private readonly IOAuthService _oauthService;
        private readonly IStepCompetitionRepository _competitionRepository;  // Inject the repository

        public StepCompetitionController(IOAuthService oauthService, IStepCompetitionRepository competitionRepository)
        {
            _oauthService = oauthService;
            _competitionRepository = competitionRepository;  // Initialize the repository
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StartCompetition()
        {
            // Get the identityId of the current user
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized(); 
            }

            // Create the competition
             await _competitionRepository.CreateCompetitionAsync(identityId);

            // Redirect to a view that shows the created competition details
            return RedirectToAction("Index");  // You can redirect to a competition details page if you have one
        }
}