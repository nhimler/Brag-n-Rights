using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface IEmbedMapService
    {
        Task<string> GetGoogleMapsApiKey();
        Task<string> ReverseGeocode(double latitude, double longitude);
    }
}