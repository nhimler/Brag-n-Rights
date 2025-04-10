using GymBro_App.DTO;
using GymBro_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymBro_App.Controllers;

[Route("api/ai")]
public class AiAPIController : ControllerBase
{

    private readonly IAiService _aiService;
    private readonly ILogger<AiAPIController> _logger;

    public AiAPIController(IAiService aiService, ILogger<AiAPIController> loggger)
    {
        _aiService = aiService;
        _logger = loggger;
    }

    [HttpGet("suggest")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string query)
    {
        var res = await _aiService.GetResponse(query);
        return Ok(res);
    }
}


