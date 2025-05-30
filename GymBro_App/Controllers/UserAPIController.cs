using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.AspNetCore.Identity;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;

namespace GymBro_App.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserAPIController : Controller
    {
        private readonly ILogger<UserAPIController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGymUserRepository _gymUserRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAPIController(ILogger<UserAPIController> logger, IUserRepository userRepository, IGymUserRepository gymUserRepository,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _gymUserRepository = gymUserRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("updateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string identityId = _userManager.GetUserId(User) ?? "";
            var user = _userRepository.GetUserByIdentityUserId(identityId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await profilePicture.CopyToAsync(memoryStream);
                user.ProfilePicture = memoryStream.ToArray();
            }

            _userRepository.AddOrUpdate(user);

            return Ok("Profile picture updated successfully.");
        }

        [HttpPost]
        [Route("bookmarkGym/{gymPlaceId}")]
        public Task<IActionResult> BookmarkGym(string gymPlaceId)
        {
            _logger.LogInformation($"Bookmarking gym with ID: {gymPlaceId}");
            string identityId = _userManager.GetUserId(User) ?? "";
            var user = _userRepository.GetUserByIdentityUserId(identityId);

            if (user == null)
            {
                return Task.FromResult<IActionResult>(NotFound("User not found."));
            }

            GymUser gymUser = new GymUser
            {
                UserId = user.UserId,
                ApiGymId = gymPlaceId,
            };


            bool isBookmarked = _gymUserRepository.IsGymBookmarked(gymPlaceId, user.UserId);
            if (isBookmarked)
            {
                _logger.LogInformation($"User {user.UserId} has already bookmarked gym: {gymPlaceId}");
                return Task.FromResult<IActionResult>(BadRequest("Gym already bookmarked."));
            }
            
            _logger.LogInformation($"User {user.UserId} is bookmarking gym: {gymPlaceId}");
            _gymUserRepository.AddOrUpdate(gymUser);
            _logger.LogInformation($"User {user.UserId} bookmarked gym: {gymPlaceId}");
            return Task.FromResult<IActionResult>(Ok("Gym bookmarked successfully."));
        }

        [HttpGet]
        [Route("isGymBookmarked/{gymPlaceId}")]
        public Task<IActionResult> IsGymBookmarked(string gymPlaceId)
        {
            string identityId = _userManager.GetUserId(User) ?? "";
            var user = _userRepository.GetUserByIdentityUserId(identityId);
            if (user == null)
            {
                return Task.FromResult<IActionResult>(NotFound("User not found."));
            }

            bool isBookmarked = _gymUserRepository.IsGymBookmarked(gymPlaceId, user.UserId);
            return Task.FromResult<IActionResult>(Ok(isBookmarked));
        }


        // Removed the UserLocation method because it was determined to be too invasive.
        // [HttpPut]
        // public IActionResult UserLocation(UserDTO userDTO)
        // {
        //     string identityId = _userManager.GetUserId(User) ?? "";
        //     Models.User gymBroUser = _userRepository.GetUserByIdentityUserId(identityId);
        //     gymBroUser.Latitude = userDTO.Latitude;
        //     gymBroUser.Longitude = userDTO.Longitude;

        //     try
        //     {
        //         _userRepository.AddOrUpdate(gymBroUser);
        //         return Ok();
        //     }

        //     catch (Exception e)
        //     {
        //         _logger.LogError(e, $"An error occurred while setting the user's coordinates");
        //         return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //     }
        // }
    }
}
