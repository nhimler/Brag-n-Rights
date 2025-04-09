using System;
using System.ComponentModel.DataAnnotations;
using GymBro_App.Models;
namespace GymBro_App.ViewModels;

public class UserInfoModel
{
    public string Username { get; set; } = "";
    
    public string Email { get; set; } = "";
    
    public string FirstName { get; set; } = "";
    
    public string LastName { get; set; } = "";
    
    [StringLength(20)]
    [RegularExpression("^(Beginner|Intermediate|Advanced)$", ErrorMessage = "Fitness level must be 'Beginner', 'Intermediate', or 'Advanced'.")]
    public string? FitnessLevel { get; set; }
    
    public List<WorkoutPlan> WorkoutPlans { get; set; } = [];
    
    public byte[] ProfilePicture { get; set; } = [];
    
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Other'.")]
    public string? Gender { get; set; }

    [Range(0.01, 999.99, ErrorMessage = "Weight must be between 0.01 and 999.99.")]
    public decimal? Weight { get; set; }

    [Range(0.01, 999.99, ErrorMessage = "Height must be between 0.01 and 999.99.")]
    public decimal? Height { get; set; }

    [StringLength(20)]
    [RegularExpression("^(Morning|Afternoon|Evening)$", ErrorMessage = "Preferred workout time must be 'Morning', 'Afternoon', or 'Evening'.")]
    public string? PreferredWorkoutTime { get; set; }

    [StringLength(255)]
    public string? Fitnessgoals { get; set; }

    public int? Age { get; set; }

    public void SetInfoFromUserModel(User user)
    {
        Username = user.Username ?? "";
        Email = user.Email ?? "";
        FirstName = user.FirstName ?? "";
        LastName = user.LastName ?? "";
        FitnessLevel = user.FitnessLevel ?? "";
        ProfilePicture = user.ProfilePicture ?? [];
        Gender = user.Gender ?? "";
        Age = user.Age;
        Weight = user.Weight;
        Height = user.Height;
        Fitnessgoals = user.Fitnessgoals ?? "";
        PreferredWorkoutTime = user.PreferredWorkoutTime ?? "";
    }
}