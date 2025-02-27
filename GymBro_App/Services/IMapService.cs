using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface IMapService
    {
        Task<string> GetGoogleMapsApiKey();
    }
}