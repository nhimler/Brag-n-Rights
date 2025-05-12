using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GymBro_App.DAL;
using GymBro_App.DAL.Abstract;

namespace GymBro_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AwardedMedalAPIController : ControllerBase
    {
        private readonly IUserMedalRepository _userMedalRepository;

        public AwardedMedalAPIController(IUserMedalRepository userMedalRepository)
        {
            _userMedalRepository = userMedalRepository;
        }


        [HttpGet("GetAwardedMedals")]
        [Authorize]
        public async Task<IActionResult> GetAwardedMedals()
        {
            // Check if the user is authenticated
            if (User.Identity == null || !User.Identity.IsAuthenticated)
                return Unauthorized();

            // Get the user's identity ID from the claims

            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();
            // Fetch the medals from the repository
            var medals = await _userMedalRepository.GetAllUserMedalsAsync(identityId);
            // Return the medals as a JSON response
            return Ok(medals);
        }

    }
}