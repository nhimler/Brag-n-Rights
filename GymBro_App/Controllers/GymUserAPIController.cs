using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using Microsoft.AspNetCore.Identity;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using GymBro_App.Models;

namespace GymBro_App.Controllers
{
    [Route("api/gymuser")]
    [ApiController]
    public class GymUserAPIController : Controller
    {
        private readonly ILogger<GymUserAPIController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGymUserRepository _gymUserRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public GymUserAPIController(ILogger<GymUserAPIController> logger, IUserRepository userRepository, IGymUserRepository gymUserRepository,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _gymUserRepository = gymUserRepository;
            _userManager = userManager;
        }

        // TODO: Remove these methods form UserAPIController and move them this controller. Update the routes and calls in any other files (ex: userLocation.js).
        // [HttpPost]
        // [Route("bookmarkGym/{gymPlaceId}")]
        // public Task<IActionResult> BookmarkGym(string gymPlaceId)
        // {
        //     _logger.LogInformation($"Bookmarking gym with ID: {gymPlaceId}");
        //     string identityId = _userManager.GetUserId(User) ?? "";
        //     var user = _userRepository.GetUserByIdentityUserId(identityId);

        //     if (user == null)
        //     {
        //         return Task.FromResult<IActionResult>(NotFound("User not found."));
        //     }

        //     GymUser gymUser = new GymUser
        //     {
        //         UserId = user.UserId,
        //         ApiGymId = gymPlaceId,
        //     };


        //     bool isBookmarked = _gymUserRepository.IsGymBookmarked(gymPlaceId, user.UserId);
        //     if (isBookmarked)
        //     {
        //         _logger.LogInformation($"User {user.UserId} has already bookmarked gym: {gymPlaceId}");
        //         return Task.FromResult<IActionResult>(BadRequest("Gym already bookmarked."));
        //     }
            
        //     _logger.LogInformation($"User {user.UserId} is bookmarking gym: {gymPlaceId}");
        //     _gymUserRepository.AddOrUpdate(gymUser);
        //     _logger.LogInformation($"User {user.UserId} bookmarked gym: {gymPlaceId}");
        //     return Task.FromResult<IActionResult>(Ok("Gym bookmarked successfully."));
        // }

        // [HttpGet]
        // [Route("isGymBookmarked/{gymPlaceId}")]
        // public Task<IActionResult> IsGymBookmarked(string gymPlaceId)
        // {
        //     string identityId = _userManager.GetUserId(User) ?? "";
        //     var user = _userRepository.GetUserByIdentityUserId(identityId);
        //     if (user == null)
        //     {
        //         return Task.FromResult<IActionResult>(NotFound("User not found."));
        //     }

        //     bool isBookmarked = _gymUserRepository.IsGymBookmarked(gymPlaceId, user.UserId);
        //     return Task.FromResult<IActionResult>(Ok(isBookmarked));
        // }

        [HttpPost]
        [Route("bookmark/delete/{gymPlaceId}")]
        public Task<IActionResult> DeleteGymBookmark(string gymPlaceId)
        {
            _logger.LogInformation($"Deleting bookmark for gym with ID: {gymPlaceId}");
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
            if (!isBookmarked)
            {
                _logger.LogInformation($"User {user.UserId} has not bookmarked gym: {gymPlaceId}");
                return Task.FromResult<IActionResult>(BadRequest("Gym not bookmarked."));
            }

            _logger.LogInformation($"User {user.UserId} is deleting bookmark for gym: {gymPlaceId}");
            _gymUserRepository.Delete(gymUser);
            _logger.LogInformation($"User {user.UserId} deleted bookmark for gym: {gymPlaceId}");
            return Task.FromResult<IActionResult>(Ok("Gym bookmark deleted successfully."));
        }
    }
}
