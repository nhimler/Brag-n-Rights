using GymBro_App.DTO;

namespace GymBro_App.Services;

public interface IAiService
{
    public enum AiServiceType
    {
        Suggestion,
        Fill
    }

    Task<string> GetResponse(string query, AiServiceType type);
}
