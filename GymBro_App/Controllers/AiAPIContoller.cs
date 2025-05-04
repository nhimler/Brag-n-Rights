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
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Suggest([FromQuery(Name = "q")] string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return BadRequest("Query cannot be null or empty.");
        }
        try{
            var res = await _aiService.GetResponse(query, IAiService.AiServiceType.Suggestion);
            return Ok(res);
        }
        catch
        {
            _logger.LogError("Error occurred while getting AI response.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpGet("fill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> Fill([FromQuery(Name = "q")] string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return BadRequest("Query cannot be null or empty.");
        }
        try{
            var res = await _aiService.GetResponse(query, IAiService.AiServiceType.Fill);
            return Ok(res);
        }
        catch
        {
            _logger.LogError("Error occurred while getting AI response.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}


