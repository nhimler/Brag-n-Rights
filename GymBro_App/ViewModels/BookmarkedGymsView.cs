using GymBro_App.Models;
using GymBro_App.Models.DTOs;


namespace GymBro_App.ViewModels
{
    public class BookmarkedGymsView
    {
        public List<PlaceDTO> AllGyms { get; set; } = new List<PlaceDTO>();
    }
}