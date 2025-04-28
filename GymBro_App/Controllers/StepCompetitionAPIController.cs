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

            var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();


            // Call the repository method to search for users with the given username
            var users = await _stepCompetitionRepository.SearchUsersWithTokenAsync(username,identityId);

            return Ok(users);
            
        }

        [Authorize]
        [HttpPost("StartCompetition")]
        public async Task<IActionResult> StartCompetition([FromForm] List<string> InvitedUsernames)
        {
            var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();


            var competition = await _stepCompetitionRepository.CreateCompetitionAsync(identityId);
            if (competition == null)
            {
                return BadRequest("Failed to create competition.");
            }
            // Send invitations to the invited users
            await _stepCompetitionRepository.InviteUsersToCompetitionAsync(identityId, competition, InvitedUsernames);

            var competitions = await _stepCompetitionRepository.GetCompetitionsForUserAsync(identityId);

            return Ok(competitions); // send back all competitions so the UI can update
        }

        [Authorize]
        [HttpGet("UserCompetitions")]
        public async Task<IActionResult> GetUserCompetitions()
        {
            var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();

            var competitions = await _stepCompetitionRepository.GetCompetitionsForUserAsync(identityId);
            return Ok(competitions);
        }

        [Authorize]
        [HttpGet("PastCompetitions")]
        public async Task<IActionResult> GetPastCompetitions()
        {
            var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();

            var competitions = await _stepCompetitionRepository.GetPastCompetitionsForUserAsync(identityId);
            return Ok(competitions);
        }

        [Authorize]
        [HttpDelete("LeaveCompetition/{competitionID}")]
        public async Task<IActionResult> LeaveCompetition(int competitionID)
        {
            var identityId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(identityId))
                return Unauthorized();

            var result = await _stepCompetitionRepository.LeaveCompetitionAsync(identityId, competitionID);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to leave the competition.");
            }
        }
    }
}