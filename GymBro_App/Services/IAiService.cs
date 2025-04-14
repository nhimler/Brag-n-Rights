using GymBro_App.DTO;

namespace GymBro_App.Services;

public interface IAiService
{
    Task<string> GetResponse(string query);
}
