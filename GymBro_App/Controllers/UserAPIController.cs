using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.AspNetCore.Identity;
using GymBro_App.DAL.Abstract;
using GymBro_App.Models;

namespace GymBro_App.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserAPIController : Controller
    {
        private readonly ILogger<UserAPIController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAPIController(ILogger<UserAPIController> logger, IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userRepository = userRepository;
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
