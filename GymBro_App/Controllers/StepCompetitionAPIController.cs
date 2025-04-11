using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;  
using GymBro_App.DAL.Abstract;

namespace GymBro_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepCompetitionAPIController : ControllerBase
    {

        private readonly IStepCompetitionRepository _stepCompetitionRepository;

        public StepCompetitionAPIController(IStepCompetitionRepository stepCompetitionRepository)
        {
            _stepCompetitionRepository = stepCompetitionRepository;
        }


        [Authorize]
        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUser(string username)
        {

            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized();  // Return 401 if the user is not logged in or has no identity
            }

            // Call the repository method to search for users with the given username
            var users = await _stepCompetitionRepository.SearchUsersWithTokenAsync(username,identityId);

            return Ok(users);
            
        }

        [Authorize]
        [HttpPost("StartCompetition")]
        public async Task<IActionResult> StartCompetition([FromForm] List<string> InvitedUsernames)
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
            {
                return Unauthorized();  
            }

            var competition = await _stepCompetitionRepository.CreateCompetitionAsync(identityId);
            if (competition == null)
            {
                return BadRequest("Failed to create competition.");
            }
            var competitions = await _stepCompetitionRepository.GetCompetitionsForUserAsync(identityId);

            return Ok(competitions); // send back all competitions so the UI can update
        }

        [Authorize]
        [HttpGet("UserCompetitions")]
        public async Task<IActionResult> GetUserCompetitions()
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();

            var competitions = await _stepCompetitionRepository.GetCompetitionsForUserAsync(identityId);
            return Ok(competitions);
        }
    }
}