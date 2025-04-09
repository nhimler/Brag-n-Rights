using GymBro_App.Models;
namespace GymBro_App.ViewModels;

public class UserInfoModel
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FitnessLevel { get; set; } = "";
    public List<WorkoutPlan> WorkoutPlans { get; set; } = [];
    public byte[] ProfilePicture { get; set; } = [];
}