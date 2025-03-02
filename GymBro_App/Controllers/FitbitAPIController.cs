using GymBro_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymBro_App.Controllers
{
    public class FitbitAPIController : Controller
    {
        private readonly IOAuthService _oauthService;
        
        public FitbitAPIController(IOAuthService oauthService)
        {
            _oauthService = oauthService;
        }

        [Authorize]
        public IActionResult RedirectToFitbit()
        {
            var authorizationUrl = _oauthService.GetAuthorizationUrl();
            return Redirect(authorizationUrl);
        }

        [HttpGet("signin-fitbit")]
        public async Task<IActionResult> SigninFitbit(string code, string state)
        {
            // Get the identityId from the logged-in user's claims
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Replace with the actual claim name if needed

            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized();  // Return 401 if the user is not logged in or has no identity
            }

            // Ensure the state parameter matches to prevent CSRF attacks
            var sessionState = HttpContext.Session.GetString("oauth_state");
            if (string.IsNullOrEmpty(sessionState) || sessionState != state)
            {
                return BadRequest("State parameter mismatch.");  // You can throw an error if state doesn't match
            }

            try
            {
                // Now call the ExchangeCodeForToken method to get the token and store it
                await _oauthService.ExchangeCodeForToken(identityId, code);

                return RedirectToAction("AwardMedals", "AwardMedal");  // Redirect to Home or another page
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest("Invalid state parameter.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}