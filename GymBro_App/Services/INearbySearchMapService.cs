using GymBro_App.Models.DTOs;

namespace GymBro_App.Services
{
    public interface INearbySearchMapService
    {
        Task<List<PlaceDTO>> FindNearbyGyms(double latitude, double longitude);
    }
}