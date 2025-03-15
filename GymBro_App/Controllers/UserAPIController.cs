using Microsoft.AspNetCore.Mvc;
using GymBro_App.Models.DTOs;
using GymBro_App.Services;
using System.Threading.Tasks;
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

    }
}
