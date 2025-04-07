using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D105341DD18EE9", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(450)]
    public string? IdentityUserId { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(10)]
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Other'.")]
    public string? Gender { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    [Range(0.01, 99999.99, ErrorMessage = "Weight must be between 0.01 and 99999.99.")]
    public decimal? Weight { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    [Range(0.01, 99999.99, ErrorMessage = "Height must be between 0.01 and 99999.99.")]
    public decimal? Height { get; set; }

    [StringLength(20)]
    [RegularExpression("^(Beginner|Intermediate|Advanced)$", ErrorMessage = "Fitness level must be 'Beginner', 'Intermediate', or 'Advanced'.")]
    public string? FitnessLevel { get; set; }

    [StringLength(255)]
    public string? Fitnessgoals { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AccountCreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    public byte[]? ProfilePicture { get; set; }

    [StringLength(20)]
    [RegularExpression("^(Morning|Afternoon|Evening)$", ErrorMessage = "Preferred workout time must be 'Morning', 'Afternoon', or 'Evening'.")]
    public string? PreferredWorkoutTime { get; set; }

    [StringLength(255)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    public virtual TokenEntity? Token { get; set; }
        

    [InverseProperty("User")]
    public virtual ICollection<BiometricDatum> BiometricData { get; set; } = new List<BiometricDatum>();

    [InverseProperty("User")]
    public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

    [InverseProperty("User")]
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

    [InverseProperty("User")]
    public virtual ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();

    [InverseProperty("User")]
    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<FitnessChallenge> Challenges { get; set; } = new List<FitnessChallenge>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Gym> Gyms { get; set; } = new List<Gym>();
}
